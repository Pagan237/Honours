using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float fieldOfView = 70f;
    private bool inSight = false;
    private Vector2 enemyLastSeen;
    private bool isReloading = false;
    private Transform enemyPos;
    private Enemy enemy;
    private float timeAlive;
    public float speed;
    public int health;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = 2;
        timeAlive = 0;
        enemy = GameObject.FindGameObjectWithTag("AI").GetComponent<Enemy>();
        enemyPos = GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>();
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
