﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D rb = null;
    [SerializeField] private float speed = 0;
    [SerializeField] private float m_jumpSpeed = 0;
    public int m_coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            rb.velocity += new Vector2(0, m_jumpSpeed);
    }

    private void FixedUpdate()
    {
        //Vector2 moveDirection = new Vector2(0, 0);

        //moveDirection.Set(speed, speed);
        rb.velocity = new Vector2(speed, rb.velocity.y);
        ////rb.velocity += new Vector2(speed, 0) * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            m_coins++;
            collision.gameObject.SetActive(false);
        }
        if (collision.tag == "Fall Limit")
        {
            gameObject.SetActive(false);
        }
    }
}
