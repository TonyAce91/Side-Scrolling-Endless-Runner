using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    Rigidbody2D rb = null;
    [SerializeField] private float m_jumpSpeed = 0;
    [SerializeField] private float m_gravityMultiplier = 2f;
    private LevelGenerator m_generator = null;

    public UnityEvent onDeath;
    private Vector2 originalPosition = new Vector2(-10f, -4.5f);

    public int m_coins = 0;
    [SerializeField] private Text m_coinText = null;
    [SerializeField] private EndGame endScript = null;
    private bool m_grounded = false;
    private bool m_airborne = false;
    private int m_jumpNumber = 0;

    private void OnEnable()
    {
        // Resets player
        transform.position = originalPosition;
        m_coins = 0;
        m_coinText.text = "Coins: 0";
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_generator = FindObjectOfType<LevelGenerator>();
        if (!endScript)
            endScript = FindObjectOfType<EndGame>();
    }

    // Update is called once per frame
    void Update()
    {

        // Jump sequence depending on which platform. Spacebar for PC and tap for mobile
#if (UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_EDITOR)
        if (Input.GetButtonDown("Jump") && Time.timeScale > 0)
            JumpSequence();
#elif (UNITY_IOS || UNITY_ANDROID)
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0)
        {
            JumpSequence();
        }
#endif
    }

    private void FixedUpdate()
    {
        // If player face smash the side of a cliff. Velocity will become negative
        if (rb.velocity.x < -3f)
            DeathSequence();

        // If player is falling, increase the effect of gravity to decrease air time and simulate game gravity
        if (rb.velocity.y < 0f)
            rb.velocity += Physics2D.gravity * m_gravityMultiplier * Time.fixedDeltaTime;
    }

    public void JumpSequence()
    {
        // Checks whether player jumping ground or double jumping but more than double jump
        if (m_grounded || (m_airborne && m_jumpNumber < 2))
        {
            rb.velocity = new Vector2(0, m_jumpSpeed);
            m_jumpNumber++;
        }
    }

    private void DeathSequence()
    {
        // Turns off the character and calls event system for death related events
        gameObject.SetActive(false);
        onDeath.Invoke();

        // turns of movement in all world chunks
        m_generator.PlayerDied();

        // Sends the amount of coin collected to the end game script
        endScript.CollectedCoins(m_coins);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks for coin collection
        if (collision.tag == "Coin")
        {
            m_coins++;
            collision.gameObject.SetActive(false);
            m_coinText.text = "Coins: " + m_coins;
        }

        // Checks whether player fell from the world
        if (collision.tag == "Fall Limit")
        {
            DeathSequence();
        }

        // Goes through death sequence when player hit an obstacle
        if (collision.tag == "Obstacle")
        {
            DeathSequence();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks collision with ground and reset jump counter
        if (collision.collider.tag == "Ground")
        {
            m_grounded = true;
            m_airborne = false;
            m_jumpNumber = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Turns of grounded and set airborne after jumping off ground
        if (collision.collider.tag == "Ground")
        {
            m_grounded = false;
            m_airborne = true;
        }
    }
}
