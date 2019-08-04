using UnityEngine;
using UnityEngine.UI;
using FSM.States;
using FSM.States.ActorS;
using FSM;

public class Player : Actor
{
    private static Player m_instance = null;
    public static Player GetInstance()
    {
        return m_instance;
    }

    [SerializeField] private Transform m_origin = null;
    [SerializeField] private int m_maxClipSize = 0;
    [SerializeField] private float m_shootForce = 0f;
    [SerializeField] private float m_recoil = 0f;
    [SerializeField] private float m_recoilDeadzone = 0f;
    [SerializeField] private Projectile m_projectile = null;
    [SerializeField] private Animator m_muzzle = null;

    [SerializeField] private Transform m_clipUI = null;
    [SerializeField] private Sprite m_AmmoIndicator = null;
    [SerializeField] private Sprite m_EmptyAmmoIndicator = null;

    private int m_clipSize = 0;
    private Projectile instantiatedProjectile = null;

    private Vector2 m_direction = Vector2.zero;
    private Vector2 m_mousePosition = Vector2.zero;

    private SpriteRenderer m_sr = null;
    private Animator m_anim = null;

    protected override void Awake()
    {
        m_instance = this;

        base.Awake();
        m_sr = (SpriteRenderer)GetComponent(typeof(SpriteRenderer));
        m_anim = (Animator)GetComponent(typeof(Animator));
    }

    protected override void Start()
    {
        base.Start();

        Reload();
    }

    private void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Aim();
        Orientate();

        if (Input.GetKeyDown(KeyCode.R))
        { Reload(); }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        m_anim.SetFloat("AimX", m_direction.x);
        m_anim.SetFloat("AimY", m_direction.y);
        m_anim.SetFloat("VelY", GetVelocity().y);
        m_anim.SetBool("Grounded", m_body.collisionInfo.bottom);

        m_actorSM.UpdateFSM();
    }

    private void Orientate()
    {
        if (m_direction.x < -0.1f)
        { this.transform.localScale = new Vector2(-3, 3); }
        else if (m_direction.x > -0.1f)
        { this.transform.localScale = new Vector2(3, 3); }
    }

    private void Aim()
    {
        m_direction = (m_mousePosition - (Vector2)transform.position).normalized;
    }

    private void UpdateClipUI()
    {
        for (int index = 0; index < m_clipUI.childCount; index++)
        {
            if (index < m_clipSize)
            { m_clipUI.GetChild((m_clipUI.childCount - 1) - index).GetComponent<Image>().sprite = m_AmmoIndicator; }
            else
            { m_clipUI.GetChild((m_clipUI.childCount - 1) - index).GetComponent<Image>().sprite = m_EmptyAmmoIndicator; }
        }
    }

    public void Reload()
    {
        if (m_actorSM.GetCurrentState() == m_actorSM.GetState("Idle"))
        {
            m_clipSize = m_maxClipSize;

            UpdateClipUI();
        }
    }

    public void Shoot()
    {
        if (m_clipSize > 0)
        {
            m_clipSize--;
            m_muzzle.transform.position = (Vector2)m_origin.position + m_direction * 1.25f;
            instantiatedProjectile = Instantiate(m_projectile, m_muzzle.transform.position, Quaternion.identity);
            instantiatedProjectile.Shoot(m_direction * m_shootForce, this);
            m_muzzle.Play("Muzzle_Flash");
            Knockback(-1 * m_direction * m_recoil, m_recoilDeadzone);

            UpdateClipUI();
        }
    }
}

[System.Serializable]
public struct DodgeRollConfig
{
    public float force;
    public float deadzoneTime;
}