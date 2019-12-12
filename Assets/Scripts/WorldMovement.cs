using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMovement : MonoBehaviour
{

    Rigidbody2D rb = null;
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
        rb.velocity = new Vector2(speed, 0);
        //rb.velocity += new Vector2(speed, 0) * Time.fixedDeltaTime;
    }
}
