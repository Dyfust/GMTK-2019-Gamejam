using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_explosionForce = 0f;
    [SerializeField] private float m_explosionRadius = 0f;
    [SerializeField] private GameObject m_impactEffect = null;
    [SerializeField] private ParticleSystem m_particles = null;
    private float m_explosionRadiusSqrd { get { return m_explosionRadius * m_explosionRadius; } }

    private bool m_exploded = false;

    private Actor m_shooter = null;
    private Rigidbody2D rb = null;

    public void Shoot(Vector2 force, Actor shooter)
    {
        rb = (Rigidbody2D)GetComponent(typeof(Rigidbody2D));
        m_shooter = shooter;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Surface") && !m_exploded)
        {
            Vector2 thisToActor = m_shooter.transform.position - transform.position;

            Vector2 direction = thisToActor.normalized;
            float actorDistSqrd = thisToActor.sqrMagnitude;

            if (actorDistSqrd <= m_explosionRadiusSqrd)
            {
                float force = (1f - actorDistSqrd / m_explosionRadiusSqrd) * m_explosionForce;

                m_shooter.Knockback(direction * force, 0f);
                m_exploded = true;
            }

            Instantiate(m_impactEffect, transform.position, Quaternion.identity);
            //Instantiate(m_particles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}