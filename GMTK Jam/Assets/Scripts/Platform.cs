using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private static float m_harmlessThresholdH = 7f;
    private static float m_harmlessThresholdV = 12f;

    [SerializeField] private PlatformType m_type;

    private float m_harmlessThreshold = 0f;

    private bool m_playerEntered = false;
    private bool m_isHostile = false;

    private Animator m_anim = null;

    private void Awake()
    {
        m_anim = (Animator)GetComponent(typeof(Animator));

        m_harmlessThreshold = m_type == PlatformType.HORIZONTAL ? m_harmlessThresholdH : m_harmlessThresholdV;
    }

    private void Update()
    {
        if (m_playerEntered && !m_isHostile)
        {
            if ((Player.GetInstance().transform.position.y - transform.position.y) >= m_harmlessThreshold)
            {
                m_isHostile = true;
                m_anim.Play("Spikes_Incoming");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { m_playerEntered = true; }
    }

    public enum PlatformType { HORIZONTAL, VERTICAL }
}