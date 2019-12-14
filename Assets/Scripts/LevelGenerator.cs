using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<WorldMovement> m_worldPrefabs = new List<WorldMovement>();
    [SerializeField] private Transform m_spawnerTransform = null;
    [SerializeField] private float m_minSpawnTimer = 0;
    [SerializeField] private float m_maxSpawnTimer = 0;
    [SerializeField] private WorldMovement m_firstWorld = null;
    [SerializeField] private Player m_player = null;
    private bool m_playerAlive = false;

    [SerializeField] private int totalSpeed = 10;
    [SerializeField] private float distance = 0;

    [SerializeField] private Text distanceText = null;

    private Dictionary<int, List<GameObject>> m_worldList = new Dictionary<int, List<GameObject>>();
    private List<WorldMovement> activeWorlds = new List<WorldMovement>();

    private float m_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_timer = m_minSpawnTimer;

        m_player = FindObjectOfType<Player>();

        // Maybe later down the track, this can be automated as well by creating the first prefab at (0, 0, 0) point

        m_worldList.Add(0, new List<GameObject>());
        m_worldList[0].Add(m_firstWorld.gameObject);
        activeWorlds.Add(m_firstWorld);
        m_playerAlive = true;
        //m_timer = Random.Range(m_minSpawnTimer, m_maxSpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerAlive)
        {
            distance += totalSpeed * Time.fixedDeltaTime;
            distanceText.text = "Distance: " + distance.ToString("0") + "m";
        }

    }

    private void FixedUpdate()
    {
        //if (m_timer > 0)
        //    m_timer -= Time.fixedDeltaTime;
        //else
        //{
        //    m_timer = m_minSpawnTimer;
        //    int prefabNumber = Random.Range(0, m_prefab.Count - 1);
        //    //if (m_worldList[prefabNumber].Count > 0)
        //    //{
        //    //    foreach (GameObject obj in m_worldList[prefabNumber])
        //    //    {
        //    //        if (!obj.activeSelf)
        //    //            obj.SetActive(true);
        //    //    }
        //    //}
        //    GameObject go = Instantiate(m_prefab[0], m_spawnerPos.position, Quaternion.identity);
        //}
    }

    public void SpawnChunk()
    {
        // Spawns a random world/level from a list of prefabs
        int prefabNumber = Random.Range(0, m_worldPrefabs.Count);

        Debug.Log("Prefab Number is: " + prefabNumber);

        // Checks if the game already has a copy of the chosen prefabb
        if (m_worldList.ContainsKey(prefabNumber) && m_worldList[prefabNumber].Count > 0)
        {
            // Goes through the list of copies and check for any inactive gameobject
            foreach (GameObject obj in m_worldList[prefabNumber])
            {
                // Once an inactive copy is found, activate and set it on the spawner position
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    obj.transform.position = m_spawnerTransform.position;

                    // Adds the world in the list of active worlds
                    // this list is useful for modifying speed later in development
                    activeWorlds.Add(obj.GetComponent<WorldMovement>());

                    // Activates all the children of that world
                    // This means activate all the coins collected
                    foreach(Transform child in obj.transform)
                    {
                        child.gameObject.SetActive(true);
                    }

                    return;
                }
            }
        }
        GameObject go = Instantiate(m_worldPrefabs[prefabNumber].gameObject, m_spawnerTransform.position, Quaternion.identity);

        if (!m_worldList.ContainsKey(prefabNumber))
            m_worldList.Add(prefabNumber, new List<GameObject>());

        m_worldList[prefabNumber].Add(go);
        activeWorlds.Add(go.GetComponent<WorldMovement>());
    }

    public void Deactivate(WorldMovement world)
    {
        activeWorlds.Remove(world);
    }

    public void PlayerDied()
    {
        m_playerAlive = false;
        foreach (WorldMovement world in activeWorlds)
            world.speed = 0;
    }

}
