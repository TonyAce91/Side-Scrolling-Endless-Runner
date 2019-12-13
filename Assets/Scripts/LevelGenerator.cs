using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_prefab = new List<GameObject>();
    [SerializeField] private Transform m_spawnerPos = null;
    [SerializeField] private float m_minSpawnTimer = 0;
    [SerializeField] private float m_maxSpawnTimer = 0;

    private Dictionary<int, List<GameObject>> m_worldList = new Dictionary<int, List<GameObject>>();

    private float m_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = m_minSpawnTimer;
        //m_timer = Random.Range(m_minSpawnTimer, m_maxSpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_timer > 0)
            m_timer -= Time.fixedDeltaTime;
        else
        {
            m_timer = m_minSpawnTimer;
            int prefabNumber = Random.Range(0, m_prefab.Count - 1);
            //if (m_worldList[prefabNumber].Count > 0)
            //{
            //    foreach (GameObject obj in m_worldList[prefabNumber])
            //    {
            //        if (!obj.activeSelf)
            //            obj.SetActive(true);
            //    }
            //}
            GameObject go = Instantiate(m_prefab[0], m_spawnerPos.position, Quaternion.identity);
        }
    }
}
