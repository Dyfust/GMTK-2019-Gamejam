using UnityEngine;
using FSM;
using FSM.States;

public class Actor : MonoBehaviour, IStateMachine
{
    [Header("General movement properties")]
    [SerializeField] protected float m_defaultGravity = 9.81f;

    [Header("Idle properties")]
    [SerializeField] protected float m_friction = 0f;

    [Header("Airborne properties")]
    [SerializeField] protected float m_drag = 0f;

    [Header("Wall sliding properties")]
    [SerializeField] protected float m_wallSlideSpeed = 0f;

    private float m_acceleration;
    private float m_bufferedAcceleration = 0f;

    private float m_currentResistance = 0f;
    private float m_currentGravity = 0f;

    private Vector2 m_bufferedKnockbackForce = Vector2.zero;
    private float m_knockbackDeadzone = 0f;
    private float m_knockbackDeadzoneTime = 0f;

    private Vector2 m_velocity = Vector2.zero;

    protected Body m_body = null;
    [SerializeField] private BodyConfig m_bodyConfig = null;

    protected StateMachine m_actorSM = null;

    private BoxCollider2D m_coll = null;

    protected virtual void Awake()
    {
        m_coll = (BoxCollider2D)GetComponent(typeof(BoxCollider2D));
        m_body = new Body(transform, m_coll, m_bodyConfig);

        InitializeStatemachine();
    }

    protected virtual void Start()
    {
        // temp
        m_currentGravity = m_defaultGravity;
    }

    #region Platforming physics.
    protected virtual void FixedUpdate()
    {
        ComputeVelocity();
    }

    private void InitializeStatemachine()
    {
        m_actorSM = new StateMachine();

        // Parameters.
        m_actorSM.CreateParameter("Idle");
        m_actorSM.CreateParameter("Airborne");
        m_actorSM.CreateParameter("Wall Sliding");

        // States.
        State idleState = new FSM.States.ActorS.IdleState(this, m_friction, "Idle");
        State airborneState = new FSM.States.ActorS.AirborneState(this, m_drag, "Airborne");
        State wallSlidingState = new FSM.States.ActorS.WallSlideState(this, m_wallSlideSpeed, "Wall Sliding");
        m_actorSM.AddState(idleState);
        m_actorSM.AddState(airborneState);
        m_actorSM.AddState(wallSlidingState);

        // Transitions.
        idleState.AddTransition(new Transition(airborneState, m_actorSM.GetParameter("Airborne"), true));
        airborneState.AddTransition(new Transition(wallSlidingState, m_actorSM.GetParameter("Wall Sliding"), true));
        airborneState.AddTransition(new Transition(idleState, m_actorSM.GetParameter("Idle"), true));
        wallSlidingState.AddTransition(new Transition(idleState, m_actorSM.GetParameter("Idle"), true));
        wallSlidingState.AddTransition(new Transition(airborneState, m_actorSM.GetParameter("Airborne"), true));

        m_actorSM.SetDefaultState(idleState);
    }

    private void ComputeVelocity()
    {
        m_body.FixedUpdate();
        ApplyGravity();

        m_actorSM.SetBool("Wall Sliding", m_body.collisionInfo.touchingWall && m_velocity.y < 0f);
        m_actorSM.SetBool("Idle", m_body.collisionInfo.bottom);
        m_actorSM.SetBool("Airborne", m_velocity.y != 0f && !m_body.collisionInfo.bottom && !m_actorSM.GetBool("Wall Sliding"));

        if (Time.fixedTime >= m_knockbackDeadzoneTime)
        {
            // Apply acceleration.
            // Sidenote: No acceleration in this game.;
            m_velocity.x += m_bufferedAcceleration * Time.fixedDeltaTime;
            m_bufferedAcceleration = 0f;

            // Deceleration.
            float resistanceDirection = m_velocity.x == 0f ? 0 : -Mathf.Sign(m_velocity.x);
            m_currentResistance = m_body.collisionInfo.bottom ? m_friction : m_drag;
            m_velocity.x += m_currentResistance * resistanceDirection * Time.fixedDeltaTime;

            if (Mathf.Sign(m_velocity.x) == resistanceDirection)
            {
                m_velocity.x = 0f;
            }
        }

        // Apply any buffered knockback forces for this frame.
        if (m_bufferedKnockbackForce != Vector2.zero)
        {
            float dot = Vector2.Dot(m_velocity.normalized, m_bufferedKnockbackForce.normalized);

            if (m_bufferedKnockbackForce.x != 0)
                m_velocity.x = dot < 0 ? m_bufferedKnockbackForce.x : m_velocity.x + m_bufferedKnockbackForce.x;

            m_velocity.y = m_bufferedKnockbackForce.y;
            m_bufferedKnockbackForce = Vector2.zero;

            m_knockbackDeadzoneTime = Time.fixedTime + m_knockbackDeadzone;
        }

        m_body.Move(ref m_velocity, Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        m_velocity += Vector2.down * m_currentGravity * Time.fixedDeltaTime;
    }
    #endregion

    public void Knockback(Vector2 force, float knockbackDeadzone)
    {
        m_knockbackDeadzone = knockbackDeadzone;
        m_bufferedKnockbackForce += force;
    }

    // Overwrites everything. Use only if you know what you're doing.
    public void SetVelocity(Vector2 velocity)
    { m_velocity = velocity; }

    // Overwrites everything. Use only if you know what you're doing.
    public void SetVelocity(float x, float y)
    {
        m_velocity.x = x;
        m_velocity.y = y;
    }

    public void SetResistance(float resistance)
    { m_currentResistance = resistance; }

    public Vector2 GetVelocity()
    { return m_velocity; }

    StateMachine IStateMachine.GetStateMachine()
    { return m_actorSM; }
}