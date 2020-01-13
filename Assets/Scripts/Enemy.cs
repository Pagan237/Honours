using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float fov = 70f;
    public bool inSight = false;
    public Vector2 PLS;
    public int ammo;
    public bool reloading;
    public Patrol patrol;
    public float speed;
    private Transform playerpos;

    private EnemyShooting ES;

    private Player player;

    public int health = 1;

    public int maxHealth = 10;
    private float TimeAlive;

    private float TimeSpentHealing = 0;
    public bool isHealing = false;

    private float waitTime;
    private Vector2 direction;
    private Vector3 startPos;

    private float TimeSinceLastShot;
    private float TimeSinceReload;
    private int randSpot;
    // Start is called before the first frame update
    void Start()
    {
        reloading = false;
        ammo = 30;
        patrol = GetComponent<Patrol>();
        ES = GetComponent<EnemyShooting>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerpos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        randSpot = Random.Range(0, patrol.moveSpots.Count);
        direction = patrol.moveSpots[randSpot].position - transform.position;
    }

    // Update is called once per frame
    
    void Update()
    {
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f){
            randSpot = Random.Range(0, patrol.moveSpots.Count);
            waitTime = patrol.startWaitTime;
            if(waitTime < 0)
                startPos = transform.position; 
            TimeSinceLastShot -= Time.deltaTime;
        }
        else
            TimeSinceLastShot -= Time.deltaTime;
        float dirAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(dirAngle, Vector3.forward),10000*Time.deltaTime); 
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f){
            direction = (startPos - patrol.moveSpots[randSpot].position) * -1;
        }
        else
            direction = patrol.moveSpots[randSpot].position - transform.position;      
        Vector2 playerDirection = playerpos.position - transform.position;
        float angle = Vector2.Angle(playerDirection, direction);
        if(playerDirection.x < 5 && playerDirection.y < 5){
            if(angle < fov/2 && angle != 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerpos.position, speed * Time.deltaTime);
                if(TimeSinceLastShot < 0 && !reloading){
                    Instantiate(ES.shot, ES.AIpos.position, Quaternion.identity);
                    TimeSinceLastShot = ES.StartTimeBetweenShots;
                    ammo--;
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        if(ammo == 0){
            reload();
        }
        TimeSinceReload -= Time.deltaTime;
        if(TimeSinceReload < 0){
            reloading = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.health--;
            Destroy(gameObject);
        }
        if(other.CompareTag("Wall"))
        {
            Debug.Log("Wall hit");
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Triggered");
            /* 
            inSight = false;

            Vector2 direction = other.transform.position - transform.position;
            float angle = Vector2.Angle(direction, transform.forward);

            if(angle < fov/2)
            {
                inSight = true;
                PLS = other.transform.position;
            }
            */
        }    
    }

    void heal()
    {
        health += 4;
        if (health > maxHealth)
            health = maxHealth;
    }
    void reload(){
        ammo = 30;
        reloading = true;
        TimeSinceReload = 2.0f;
    }
}
