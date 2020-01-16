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
    private Shooting shooting;
    private float timeAlive;
    public float speed;
    public int health;
    private Rigidbody2D rb;

    public GAPatrol patrol;
    private Vector2 moveVelocity;
    public int ammo;
    private int randSpot;
    private Vector2 direction;
    private float timeSinceLastShot;
    // Start is called before the first frame update
    void Start()
    {
        ammo = 30;
        randSpot = Random.Range(0, 9);
        rb = GetComponent<Rigidbody2D>();
        health = 2;
        timeAlive = 0;
        enemy = GameObject.FindGameObjectWithTag("AI").GetComponent<Enemy>();
        enemyPos = GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>();
        patrol = GetComponent<GAPatrol>();
        shooting = GetComponent<Shooting>();
        direction = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f)
            randSpot = Random.Range(0, 9);
        float dirAngle = Mathf.Atan2(direction.x, direction.y) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(dirAngle, Vector3.forward),10000*Time.deltaTime);
        direction = patrol.moveSpots[randSpot].position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        Vector2 enemyDirection = enemyPos.position - transform.position;
        float angle = Vector2.Angle(enemyDirection, direction);
        timeSinceLastShot -= Time.deltaTime;
        if(enemyDirection.x < 5 && enemyDirection.y < 5){
            if(angle < fieldOfView/2 && angle != 0){
                transform.position = Vector2.MoveTowards(transform.position, enemyPos.position, speed * Time.deltaTime);
                if(timeSinceLastShot < 0){
                    Instantiate(shooting.shot, shooting.playerPos.position, Quaternion.identity);
                    timeSinceLastShot = shooting.fireRate;
                    ammo--;
                }
            }
        }
    }

    public Vector2 getPosition()
    {
        return rb.position;
    }
}
