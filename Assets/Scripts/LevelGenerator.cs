using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_prefab = null;
    [SerializeField] private Transform m_spawnerPos = null;
    [SerializeField] private float m_minSpawnTimer = 0;
    [SerializeField] private float m_maxSpawnTimer = 0;
    private float m_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = Random.Range(m_minSpawnTimer, m_maxSpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_timer <= 0)
        {
            m_timer = Random.Range(m_minSpawnTimer, m_maxSpawnTimer);
            GameObject go = Instantiate(m_prefab, m_spawnerPos.position, Quaternion.identity);
        }
        else
            m_timer -= Time.fixedDeltaTime;
    }
}
