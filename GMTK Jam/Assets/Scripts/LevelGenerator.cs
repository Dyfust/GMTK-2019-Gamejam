using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int m_minLevelHeight = 0;
    [SerializeField] private int m_maxLevelHeight = 0;
    [SerializeField] private Transform[] m_hPlatforms = new Transform[] { };
    [SerializeField] private Transform[] m_vPlatforms = new Transform[] { };
    [SerializeField] private Transform m_exitPlatform = null;

    [SerializeField] private Vector2 m_minDist = Vector2.zero;
    [SerializeField] private Vector2 m_maxDist = Vector2.zero;

    private int m_levelHeight = 0;

    [SerializeField] private Transform m_wallTreshold = null;
    [SerializeField] private Transform m_walls = null;
    private float currentPosY = 9.5f;

    private Transform m_currentPlatform = null;
    private Vector2 m_currentPosition = Vector2.zero;

    private int dir = 1;

    private void Start()
    {
        m_levelHeight = Random.Range(m_minLevelHeight, m_maxLevelHeight);

        for (int index = 0; index < m_levelHeight; index++)
        {
            GeneratePlatform(index);
        }
    }

    private void Update()
    {
        if (currentPosY < m_wallTreshold.position.y)
        { GenerateWalls(); }
    }

    private void GeneratePlatform(int level)
    {
        Transform platform = null;
        int rng = Random.Range(0, 10);

        if (rng < 3)
        { platform = m_vPlatforms[Random.Range(0, m_vPlatforms.Length)]; }
        else
        { platform = m_hPlatforms[Random.Range(0, m_hPlatforms.Length)]; }

        if (level == m_levelHeight - 1)
        {
            platform = m_exitPlatform;
            m_currentPosition.x = 0f;
        }

        m_currentPlatform = Instantiate(platform, m_currentPosition, Quaternion.identity, this.transform);
        m_currentPosition.x = dir * Random.Range(m_minDist.x, m_maxDist.x);
        m_currentPosition.y += Random.Range(m_minDist.y, m_maxDist.y);

        dir = -1 * dir;
    }

    private void GenerateWalls()
    {
        Vector2 pos = Vector2.zero;
        currentPosY += 23f;
        pos.x = 0f;
        pos.y = currentPosY;

        Instantiate(m_walls, pos, Quaternion.identity, this.transform);
    }
}
