using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<WorldMovement> m_worldPrefabs = new List<WorldMovement>();
    [SerializeField] private Transform m_spawnerTransform = null;
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
        m_player = FindObjectOfType<Player>();
    }
    private void FixedUpdate()
    {
        if (m_playerAlive)
        {
            distance += totalSpeed * Time.fixedDeltaTime;
            distanceText.text = "Distance: " + distance.ToString("0") + "m";
        }
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

        // Instantiate certain prefab that has been created
        GameObject go = Instantiate(m_worldPrefabs[prefabNumber].gameObject, m_spawnerTransform.position, Quaternion.identity, m_levelParent);

        // Adds the copy of a prefab to list of all worlds
        if (!m_worldList.ContainsKey(prefabNumber))
            m_worldList.Add(prefabNumber, new List<GameObject>());

        m_worldList[prefabNumber].Add(go);

        // Adds it to list of active worlds
        WorldMovement world = go.GetComponent<WorldMovement>();
        activeWorlds.Add(world);

        // Set the movement speed
        world.speed = -totalSpeed;
    }

    // Used to deactivate certain world chunk in game
    public void Deactivate(WorldMovement world)
    {
        if (activeWorlds.Contains(world))
            activeWorlds.Remove(world);
    }

    // Used to tell level generator that the player died and turns off speed of all current worlds
    public void PlayerDied()
    {
        m_playerAlive = false;
        foreach (WorldMovement world in activeWorlds)
            world.speed = 0;
    }
}
