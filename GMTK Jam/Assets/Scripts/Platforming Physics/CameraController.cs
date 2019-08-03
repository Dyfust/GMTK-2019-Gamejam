using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2[] m_anchors = new Vector2[2];
    [SerializeField] private Transform m_target = null;
    private Vector3 m_targetPosition = Vector2.zero;

    private Vector2 m_mousePosition = Vector2.zero;

    private void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        m_anchors[0] = m_target.position;
        m_anchors[1] = m_mousePosition;

        Vector2 sum = Vector2.zero;
        for (int index = 0; index < m_anchors.Length; index++)
        {
            sum += m_anchors[index];
        }

        m_targetPosition = sum / m_anchors.Length;
        m_targetPosition.z = -10f;

        transform.position = m_targetPosition;
    }
}
