using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldMovement : MonoBehaviour
{

    Rigidbody2D rb = null;
    public float speed = 0;
    private LevelGenerator m_generator = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_generator = FindObjectOfType<LevelGenerator>();
        NullChecker();
    }

    private void FixedUpdate()
    {
        // Moves the world according to their speed set by level generator
        Vector2 moveDirection = new Vector2(0, 0);

        moveDirection.Set(speed, speed);
        rb.velocity = new Vector2(speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Used to trigger when to generate worlds
        if (collision.tag == "Spawn Trigger" && m_generator)
            m_generator.SpawnChunk();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Turns of the world once it has passed a certain limit
        if (collision.tag == "Spawn Trigger" && m_generator)
        {
            m_generator.Deactivate(this);
            gameObject.SetActive(false);

        }
    }

    // This is used to check for any null reference that could break the game
    private void NullChecker()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            bool errorOccurred = false;

            // Checks if any of the texts needed for this script are not set
            if (rb == null)
            {
                EditorUtility.DisplayDialog("Error", "One of the world chunks doesn't have a rigidbody 2D", "Exit");
                errorOccurred = true;
            }
            if (m_generator == null)
            {
                EditorUtility.DisplayDialog("Error", "Level Generator can't be found. Make sure you have one in the scene.", "Exit");
                errorOccurred = true;
            }

            // Turns off the application if any error occurs
            if (errorOccurred)
                EditorApplication.isPlaying = false;
        }
#endif
    }


}
