using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public int health = 10;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //normalized for diagonal movement
        moveVelocity = move.normalized * speed;
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    public Vector2 getPosition()
    {
        return rb.position;
    }
}
