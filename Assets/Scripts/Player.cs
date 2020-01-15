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

    public GAPatrol patrol;
    private Vector2 moveVelocity;

    private int randSpot;
    // Start is called before the first frame update
    void Start()
    {
        randSpot = Random.Range(0, 9);
        rb = GetComponent<Rigidbody2D>();
        health = 2;
        timeAlive = 0;
        enemy = GameObject.FindGameObjectWithTag("AI").GetComponent<Enemy>();
        enemyPos = GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>();
        patrol = GetComponent<GAPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f)
            randSpot = Random.Range(0, 9);
        transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
    }

    public Vector2 getPosition()
    {
        return rb.position;
    }
}
