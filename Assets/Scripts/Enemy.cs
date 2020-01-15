using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float fov = 70f; // field of view
    public bool inSight = false; //is player visible
    public Vector2 PLS; //Position player was last seen
    public int ammo;
    public bool reloading; 
    public EnemyPatrol patrol;
    public float speed;
    private Transform playerpos; //Position of player

    private EnemyShooting ES;

    private Player player;

    public int health = 1;

    public int maxHealth = 10;
    private float TimeAlive;

    private float TimeSpentHealing = 0;
    public bool isHealing = false; //is AI healing

    private float waitTime; //Time to wait until next patrol spot to be generated
    private Vector2 direction; //Direction AI is facing
    private Vector3 startPos; //Position of AI when beginning to travel to new spot
    
    private float TimeSinceLastShot;
    private float TimeSinceReload;
    private int randSpot;

    // Start is called before the first frame update
    void Start()
    {
        reloading = false;
        ammo = 30;
        patrol = GetComponent<EnemyPatrol>();
        ES = GetComponent<EnemyShooting>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerpos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        randSpot = Random.Range(0, patrol.moveSpots.Count - 1);
        direction = Vector3.forward;
    }

    // Update is called once per frame
    
    void Update()
    {
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f){
            randSpot = Random.Range(0, patrol.moveSpots.Count);
            waitTime = patrol.startWaitTime;
            if(waitTime <= 0)
                startPos = transform.position;
            TimeSinceLastShot -= Time.deltaTime;
        }
        else
            TimeSinceLastShot -= Time.deltaTime;
        //Calculate angle AI needs to rotate to face new Position
        float dirAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Rotate to face that position
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(dirAngle, Vector3.forward),10000*Time.deltaTime); 
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f){
            //If at new position, face of original position from new position
            direction = (startPos - patrol.moveSpots[randSpot].position) * -1;
        }
        else
            //face position travelling to
            direction = patrol.moveSpots[randSpot].position - transform.position;
        //Direction of player from AI      
        Vector2 playerDirection = playerpos.position - transform.position;
        //Angle between direction of player and direction AI is travelling
        float angle = Vector2.Angle(playerDirection, direction);
        if(playerDirection.x < 5 && playerDirection.y < 5){
            if(angle < fov/2 && angle != 0)
            {
                //If angle is less than field of view and player isn't too far away
                transform.position = Vector2.MoveTowards(transform.position, playerpos.position, speed * Time.deltaTime);
                if(TimeSinceLastShot < 0 && !reloading){
                    //If AI isn't reloading, shoot at player
                    Instantiate(ES.shot, ES.AIpos.position, Quaternion.identity);
                    TimeSinceLastShot = ES.StartTimeBetweenShots;
                    ammo--;
                }
            }
            else
                //if player is within range but outside of field of view, move to designated position
                transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        }
        else
            //if player is too far away move to designated position
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
