using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    Rigidbody2D rb = null;
    [SerializeField] private float speed = 0;
    [SerializeField] private float m_jumpSpeed = 0;
    [SerializeField] private float m_gravityMultiplier = 2f;

    public UnityEvent onDeath;

    public int m_coins = 0;
    private bool jumping = false;
    private bool m_grounded = false;
    private bool m_airborne = false;
    private int m_jumpNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            JumpSequence();
    }

    private void FixedUpdate()
    {
        //Vector2 moveDirection = new Vector2(0, 0);

        // If player face smash the side of a cliff. Velocity will become negative
        if (rb.velocity.x < -3f)
            gameObject.SetActive(false);

        // If player is falling, increase the effect of gravity to decrease air time and simulate game gravity
        if (rb.velocity.y < 0f)
            rb.velocity += Physics2D.gravity * m_gravityMultiplier * Time.fixedDeltaTime;
        //if (jumping)
        //    rb.velocity += new Vector2(0, m_jumpSpeed) * Time.fixedDeltaTime;

        //moveDirection.Set(speed, speed);
        //rb.velocity = new Vector2(speed, rb.velocity.y);
        ////rb.velocity += new Vector2(speed, 0) * Time.fixedDeltaTime;
    }

    private void JumpSequence()
    {
        //rb.AddForce(new Vector2(0f, m_jumpSpeed));
        if (m_grounded || (m_airborne && m_jumpNumber < 2))
        {
            rb.velocity = new Vector2(0, m_jumpSpeed);
            m_jumpNumber++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            m_coins++;
            //Debug.Log("Coins detected");
            collision.gameObject.SetActive(false);
        }
        if (collision.tag == "Fall Limit")
        {
            gameObject.SetActive(false);
            onDeath.Invoke();
        }

        if (collision.tag == "Obstacle")
        {
            gameObject.SetActive(false);
            //Debug.Log("Obstacle detected");
            onDeath.Invoke();
            // Maybe create a 2D particle effect
        }

        //if (collision.tag == "Ground")
        //{
        //    Debug.Log("Ground detected");
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected: " + collision.gameObject.tag);
        //collision.collider.tag = "Ground";
        //collision.transform.tag = "Ground";
        //collision.transform.is
        //collision.gameObject.tag = "Ground";
        if (collision.collider.tag == "Ground")
        {
            m_grounded = true;
            m_airborne = false;
            m_jumpNumber = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            m_grounded = false;
            m_airborne = true;
        }
    }
}
