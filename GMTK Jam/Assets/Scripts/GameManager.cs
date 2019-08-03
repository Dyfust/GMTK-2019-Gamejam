using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform m_crosshair = null;
    private Animator m_crosshairAnim = null;

    private Vector2 m_mousePosition = Vector2.zero;

    private void Awake()
    {
        m_crosshairAnim = (Animator)m_crosshair.GetComponent(typeof(Animator));

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_crosshair.position = m_mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            m_crosshairAnim.Play("Click");
        }
    }
}
