using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float m_shootForce = 0f;
    [SerializeField] private Projectile m_projectile = null;
    private Projectile instantiatedProjectile = null;
    private Vector2 m_mousePosition = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        base.Update();
    }

    public void Shoot()
    {
        instantiatedProjectile = Instantiate(m_projectile, transform.position, Quaternion.identity);
        Vector2 direction = (m_mousePosition - (Vector2)transform.position).normalized;

        instantiatedProjectile.Shoot(direction * m_shootForce, this);
    }
}