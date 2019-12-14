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

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        Vector2 moveDirection = new Vector2(0, 0);

        moveDirection.Set(speed, speed);
        rb.velocity = new Vector2(speed, 0);
        //rb.velocity += new Vector2(speed, 0) * Time.fixedDeltaTime;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spawn Trigger" && m_generator)
            m_generator.SpawnChunk();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spawn Trigger" && m_generator)
        {
            m_generator.Deactivate(this);
            gameObject.SetActive(false);

        }
    }
}
