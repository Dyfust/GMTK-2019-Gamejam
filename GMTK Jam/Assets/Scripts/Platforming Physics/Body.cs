#define DEBUG_MODE
using UnityEngine;

public class Body
{
    // Config.
    private float m_skinWidth = 0.015f;
    private int m_amountOfVerticalRays = 3;
    private int m_amountOfHorizontalRays = 3;
    private float m_wallDetectionDistance = 0f;
    private LayerMask m_collisionMask;

    // Non config.
    private Bounds m_bounds;
    private RayOrigins m_rayOrigins;
    private float m_verticalRaySpacing = 0f;
    private float m_horizontalRaySpacing = 0f;
    private float m_facingDirection = 1f;
    private CollisionInfo m_collisionInfo;
    public CollisionInfo collisionInfo { get { return m_collisionInfo; } }

    private BoxCollider2D m_collider = null;
    private Transform m_transform = null;

    public Body(Transform transform, BoxCollider2D collider, BodyConfig config)
    {
        m_transform = transform;
        m_collider = collider;

        m_skinWidth = config.skinWidth;
        m_amountOfVerticalRays = config.amountOfVerticalRays;
        m_amountOfHorizontalRays = config.amountOfHorizontalRays;
        m_collisionMask = config.collisionMask;
        m_wallDetectionDistance = config.wallDetectionDistance;

        ComputeRaycastOrigins();
        ComputeRaySpacing();
    }

    public void FixedUpdate()
    {
        DetectWall();
    }

    public void Move(ref Vector2 velocity, float deltaTime)
    {
        ComputeRaycastOrigins();
        m_collisionInfo.Reset();
        Vector2 referencedVelocity = velocity * deltaTime;
        sbyte directionX = (sbyte)Mathf.Sign(referencedVelocity.x);
        sbyte directionY = (sbyte)Mathf.Sign(referencedVelocity.y);

        HandleVerticalCollisions(ref referencedVelocity, directionY);
        HandleHorizontalCollisions(ref referencedVelocity, directionX);

        m_transform.Translate(referencedVelocity);

        if (m_collisionInfo.bottom || m_collisionInfo.above)
        { velocity.y = 0f; }

        if (m_collisionInfo.left || m_collisionInfo.right)
        { velocity.x = 0f; }

        if (velocity.x > 0f)
        { m_facingDirection = 1f; }

        if (velocity.x < 0f)
        { m_facingDirection = -1f; }

    }

    private void DetectWall()
    {
        float rayLength = m_wallDetectionDistance + m_skinWidth;
        Vector2 rayOrigin = m_facingDirection == 1 ? m_rayOrigins.bottomRight : m_rayOrigins.bottomLeft;
        m_collisionInfo.touchingWall = false;

        if (rayOrigin == m_rayOrigins.bottomRight)
        {
            rayOrigin = (m_rayOrigins.bottomRight + m_rayOrigins.topRight) / 2;
        }
        else if (rayOrigin == m_rayOrigins.bottomLeft)
        {
            rayOrigin = (m_rayOrigins.bottomLeft + m_rayOrigins.topLeft) / 2;
        }

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * m_facingDirection, rayLength, m_collisionMask);

#if DEBUG_MODE
        Debug.DrawRay(rayOrigin, Vector2.right * m_facingDirection * rayLength, Color.blue);
#endif
        if (hit)
        {
            m_collisionInfo.touchingWall = true;
            m_collisionInfo.againstWallTimestamp = Time.fixedTime;
            m_collisionInfo.wallNormal = hit.normal;
        }
    }

    private void HandleVerticalCollisions(ref Vector2 velocity, sbyte directionY)
    {
        float rayLength = Mathf.Abs(velocity.y) + m_skinWidth;
        Vector2 rayOrigin = directionY == -1 ? m_rayOrigins.bottomLeft : m_rayOrigins.topLeft;
        RaycastHit2D hit;

        for (int i = 0; i < m_amountOfVerticalRays; i++)
        {
            //                                                                           If velocity.x is too big it might offset the ray by too much and in effect it wont detect a collision
            hit = Physics2D.Raycast(rayOrigin + Vector2.right * (i * m_verticalRaySpacing /*+ velocity.x*/), Vector2.up * directionY, rayLength, m_collisionMask);
#if DEBUG_MODE
            Debug.DrawRay(rayOrigin + Vector2.right * (i * m_verticalRaySpacing + velocity.x), Vector2.up * directionY * rayLength, Color.yellow);
#endif

            if (hit)
            {
                // Will become positive when direction is negative and distance is 0 due to skin width.
                velocity.y = (hit.distance - m_skinWidth) * directionY;
                rayLength = hit.distance;

                m_collisionInfo.bottom = directionY == -1;

                if (m_collisionInfo.bottom)
                    m_collisionInfo.groundedTimestamp = Time.fixedTime;

                m_collisionInfo.above = directionY == 1;
                break;
            }
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity, sbyte directionX)
    {
        float rayLength = Mathf.Abs(velocity.x) + m_skinWidth;
        Vector2 rayOrigin = directionX == -1 ? m_rayOrigins.bottomLeft : m_rayOrigins.bottomRight;
        RaycastHit2D hit;
        for (int i = 0; i < m_amountOfHorizontalRays; i++)
        {
            //                                                                           If velocity.y is too big it might offset the ray by too much and in effect it wont detect a collision
            hit = Physics2D.Raycast(rayOrigin + Vector2.up * (i * m_horizontalRaySpacing /*+ velocity.y*/), Vector2.right * directionX, rayLength, m_collisionMask);
#if DEBUG_MODE
            Debug.DrawRay(rayOrigin + Vector2.up * (i * m_horizontalRaySpacing + velocity.y), Vector2.right * directionX * rayLength, Color.yellow);
#endif

            if (hit)
            {
                // Will become positive when direction is negative and distance is 0 due to skin width.
                velocity.x = (hit.distance - m_skinWidth) * directionX;
                rayLength = hit.distance;

                m_collisionInfo.left = directionX == -1;
                m_collisionInfo.right = directionX == 1;

                break;
            }
        }
    }

    private void ComputeRaycastOrigins()
    {
        m_bounds.center = m_collider.bounds.center;
        m_bounds.size = m_collider.bounds.size - Vector3.one * (m_skinWidth * 2f);

        m_rayOrigins.topLeft.x = m_bounds.min.x; m_rayOrigins.topLeft.y = m_bounds.max.y;
        m_rayOrigins.topRight = m_bounds.max;

        m_rayOrigins.bottomLeft = m_bounds.min;
        m_rayOrigins.bottomRight.x = m_bounds.max.x; m_rayOrigins.bottomRight.y = m_bounds.min.y;
    }

    private void ComputeRaySpacing()
    {
        m_amountOfVerticalRays = Mathf.Clamp(m_amountOfVerticalRays, 2, int.MaxValue);
        m_amountOfHorizontalRays = Mathf.Clamp(m_amountOfHorizontalRays, 2, int.MaxValue);

        m_verticalRaySpacing = m_bounds.size.x / (m_amountOfVerticalRays - 1);
        m_horizontalRaySpacing = m_bounds.size.y / (m_amountOfHorizontalRays - 1);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_rayOrigins.topLeft, 0.05f);
        Gizmos.DrawWireSphere(m_rayOrigins.topRight, 0.05f);
        Gizmos.DrawWireSphere(m_rayOrigins.bottomLeft, 0.05f);
        Gizmos.DrawWireSphere(m_rayOrigins.bottomRight, 0.05f);
    }
}

struct RayOrigins
{
    public Vector2 topLeft, topRight;
    public Vector2 bottomLeft, bottomRight;
}

public struct CollisionInfo
{
    public bool bottom, above, left, right;

    public bool touchingWall;
    public Vector2 wallNormal;

    public float groundedTimestamp;
    public float againstWallTimestamp;

    public void Reset()
    {
        left = right = bottom = above = false;
    }
}