using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_player = null;
    [SerializeField] private float m_deadzoneRadius = 0f;
    [SerializeField] private float m_damping = 0f;

    private Vector2 m_mousePosition = Vector2.zero;
    private Vector3 m_targetPosition = Vector2.zero;
    private Vector3 m_currentPosition = Vector2.zero;
    private Vector3 m_currentVelocity = Vector2.zero;

    [SerializeField] private float m_verticalBoundary = 0f;

    private void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        m_currentPosition = transform.position;

        m_targetPosition.x = m_currentPosition.x;
        m_targetPosition.y = (m_player.position.y + m_mousePosition.y) / 2f;
        m_targetPosition.z = -10f;

        m_currentPosition = Vector3.SmoothDamp(m_currentPosition, m_targetPosition, ref m_currentVelocity, m_damping);

        if (m_currentPosition.y < m_verticalBoundary)
        { m_currentPosition.y = m_verticalBoundary; }

        transform.position = m_currentPosition;
    }
}