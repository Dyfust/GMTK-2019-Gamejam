using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private static Sound m_instance = null;
    public static Sound GetInstance()
    {
        return m_instance;
    }

    [SerializeField] private AudioClip[] m_shootingSounds;
    [Range(0f, 1f)] [SerializeField] private float m_shootingVolume = 0f;

    [SerializeField] private AudioClip[] m_explosionSounds;
    [Range(0f, 1f)] [SerializeField] private float m_explosionVolume = 0f;

    [SerializeField] private AudioClip[] m_landingSounds;
    [Range(0f, 1f)] [SerializeField] private float m_landingVolume = 0f;

    private AudioSource m_source = null;

    private void Awake()
    {
        m_instance = this;
        m_source = (AudioSource)GetComponent(typeof(AudioSource));
    }

    public void PlayShootingOneshot()
    {
        m_source.PlayOneShot(m_shootingSounds[Random.Range(0, m_shootingSounds.Length)], m_shootingVolume);
    }

    public void PlayExplosionOneshot()
    {
        m_source.PlayOneShot(m_explosionSounds[Random.Range(0, m_explosionSounds.Length)], m_shootingVolume);
    }

    public void PlayLandingOneshot()
    {
        m_source.PlayOneShot(m_landingSounds[Random.Range(0, m_landingSounds.Length)], m_landingVolume);
    }
}