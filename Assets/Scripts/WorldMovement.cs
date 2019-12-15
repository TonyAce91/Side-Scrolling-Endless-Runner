using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
