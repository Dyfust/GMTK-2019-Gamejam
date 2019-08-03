using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float m_shootForce = 0f;
    [SerializeField] private Projectile m_projectile = null;
    private Projectile instantiatedProjectile = null;

    private Vector2 m_direction = Vector2.zero;
    private float m_aimAngle = 0f;

    private Vector2 m_mousePosition = Vector2.zero;

    private SpriteRenderer m_sr = null;
    private Animator m_anim = null;

    protected override void Awake()
    {
        m_sr = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        m_anim = (Animator)GetComponent(typeof(Animator));
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Aim();
        Orientate();

        m_anim.SetFloat("AimX", m_direction.x);
        m_anim.SetFloat("AimY", m_direction.y);
        m_anim.SetBool("Grounded", m_body.collisionInfo.bottom);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Orientate()
    {
        if (m_direction.x < -0.1f)
        { m_sr.flipX = true; }
        else if (m_direction.x > -0.1f)
        { m_sr.flipX = false; }
    }

    private void Aim()
    {
        m_direction = (m_mousePosition - (Vector2)transform.position).normalized;
    }

    public void Shoot()
    {
        instantiatedProjectile = Instantiate(m_projectile, transform.position, Quaternion.identity);
        instantiatedProjectile.Shoot(m_direction * m_shootForce, this);
    }
}