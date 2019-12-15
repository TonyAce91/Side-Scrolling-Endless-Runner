using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<WorldMovement> m_worldPrefabs = new List<WorldMovement>();
    [SerializeField] private Transform m_spawnerTransform = null;
    //[SerializeField] private float m_minSpawnTimer = 0;
    //[SerializeField] private float m_maxSpawnTimer = 0;
    [SerializeField] private WorldMovement m_firstWorld = null;
    [SerializeField] private Player m_player = null;
    private bool m_playerAlive = false;
    [SerializeField] private Transform m_levelParent = null;

    [SerializeField] private int totalSpeed = 10;
    public float distance = 0;

    [SerializeField] private Text distanceText = null;

    private Dictionary<int, List<GameObject>> m_worldList = new Dictionary<int, List<GameObject>>();
    private List<WorldMovement> activeWorlds = new List<WorldMovement>();

    private void OnEnable()
    {
        // Resets the game
        foreach (WorldMovement world in activeWorlds)
        {
            world.gameObject.SetActive(false);
        }
        activeWorlds.Clear();
        distance = 0;
        distanceText.text = "Distance: " + distance.ToString("0") + "m";

        // Checks if there is already an existing dictionary entry for the first prefab
        if (!m_worldList.ContainsKey(0) || m_worldList[0].Count <= 0)
        {
            // Create an entry in the dictionary for the first prefab
            m_worldList.Add(0, new List<GameObject>());

            // Instantiate a copy of the prefab
            GameObject go = Instantiate(m_worldPrefabs[0].gameObject, Vector3.zero, Quaternion.identity, m_levelParent);

            // Add it to the list in the dictionary
            m_worldList[0].Add(go);

            // Add it to the list of active worlds
            WorldMovement wm = go.GetComponent<WorldMovement>();
            activeWorlds.Add(wm);

            // Set the movement speed
            wm.speed = -totalSpeed;
        }
        else
        {
            // Goes through the list of copies and check for any inactive gameobject
            foreach (GameObject obj in m_worldList[0])
            {
                // Once an inactive copy is found, activate and set it on the zero position
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    obj.transform.position = Vector3.zero;

                    // Adds the world in the list of active worlds
                    // this list is useful for modifying speed later in development
                    WorldMovement wm = obj.GetComponent<WorldMovement>();
                    activeWorlds.Add(wm);

                    // Set the movement speed
                    wm.speed = -totalSpeed;

                    // Activates all the children of that world
                    // This means activate all the coins collected
                    foreach (Transform child in obj.transform)
                    {
                        child.gameObject.SetActive(true);
                    }

                    break;
                }

            }

        }
        m_playerAlive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_timer = m_minSpawnTimer;

        m_player = FindObjectOfType<Player>();

        // Maybe later down the track, this can be automated as well by creating the first prefab at (0, 0, 0) point

        //m_worldList.Add(0, new List<GameObject>());
        //m_worldList[0].Add(m_firstWorld.gameObject);
        //activeWorlds.Add(m_firstWorld);
        //m_timer = Random.Range(m_minSpawnTimer, m_maxSpawnTimer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (m_playerAlive)
        {
            distance += totalSpeed * Time.fixedDeltaTime;
            distanceText.text = "Distance: " + distance.ToString("0") + "m";
        }

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
                    WorldMovement wm = obj.GetComponent<WorldMovement>();
                    activeWorlds.Add(wm);

                    // Set the movement speed
                    wm.speed = -totalSpeed;

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

        GameObject go = Instantiate(m_worldPrefabs[prefabNumber].gameObject, m_spawnerTransform.position, Quaternion.identity, m_levelParent);

        if (!m_worldList.ContainsKey(prefabNumber))
            m_worldList.Add(prefabNumber, new List<GameObject>());

        m_worldList[prefabNumber].Add(go);


        WorldMovement world = go.GetComponent<WorldMovement>();
        activeWorlds.Add(world);

        // Set the movement speed
        world.speed = -totalSpeed;
    }

    public void Deactivate(WorldMovement world)
    {
        if (activeWorlds.Contains(world))
            activeWorlds.Remove(world);
    }

    public void PlayerDied()
    {
        m_playerAlive = false;
        foreach (WorldMovement world in activeWorlds)
            world.speed = 0;
    }
}
