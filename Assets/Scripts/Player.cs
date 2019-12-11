using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] private float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = new Vector2(0, 0);

        moveDirection.Set(speed, speed);
        rb.velocity = moveDirection * Time.fixedDeltaTime;
    }
}
