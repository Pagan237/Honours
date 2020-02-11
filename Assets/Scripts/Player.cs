using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float fitness;
    public float fieldOfView = 70f;
    public bool inSight = false;
    private Rigidbody2D rb;
    public GAPatrol patrol;
    private Vector2 enemyLastSeen;
    private Transform enemyPos;
    private Vector2 enemyDirection;
    private float angle;
    private Enemy enemy;
    private Shooting shooting;
    public int ammo;
    private float timeSinceLastShot;
    public bool isReloading;
    private float timeSinceReload;
    public float timeAlive;
    public float speed;
    public float lastHit;
    public int health;
    private int maxHealth = 3;
    private float timeSpentHealing;
    public bool isHealing;
    private int randSpot;
    private Vector2 direction;
    private Vector2 spawnPoint;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        lastHit = 2;
        fitness = 0;
        isReloading = false;
        timeSpentHealing = 0;
        isHealing = false;
        timeSinceReload = 0;
        ammo = 30;
        randSpot = Random.Range(0, 9);
        rb = GetComponent<Rigidbody2D>();
        health = 3;
        timeAlive = 0;
        enemy = GameObject.FindGameObjectWithTag("AI").GetComponent<Enemy>();
        enemyPos = GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>();
        patrol = GetComponent<GAPatrol>();
        shooting = GetComponent<Shooting>();
        direction = Vector3.forward;
        spawnPoint = transform.position;
        enemyDirection = enemyPos.position - transform.position;
        inSight = false;
    }

    // Update is called once per frame
    void Update()
    {
        lastHit += Time.deltaTime;
        timeAlive += Time.deltaTime;
        timeSpentHealing -= Time.deltaTime;
        if(timeSpentHealing < 0)
            isHealing = false;
        timeSinceReload -= Time.deltaTime;
        if(timeSinceReload < 0)
            isReloading = false;
        if(Vector2.Distance(transform.position, patrol.moveSpots[randSpot].position) < 0.2f)
            randSpot = Random.Range(0, 9);
        float dirAngle = Mathf.Atan2(direction.x, direction.y) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(dirAngle, Vector3.forward),10000*Time.deltaTime);
        direction = patrol.moveSpots[randSpot].position - transform.position;
        enemyDirection = enemyPos.position - transform.position;
        angle = Vector2.Angle(enemyDirection, direction);
        timeSinceLastShot -= Time.deltaTime;
        if(Vector2.Distance(enemyPos.position, transform.position) < 5){
            if(angle < fieldOfView/2 && angle != 0){
                inSight = true;      
            }
        }
        else
            inSight = false;
        if(health <= 0){
            dead = true;
        }
    }

    void OnTriggerEnter2D(Collider2D proj)
    {
        if(proj.tag == "Projectile")
        {
            Projectile pro = proj.GetComponent<Projectile>();
            if(pro.ownerTag != gameObject.tag){
                health--;
                lastHit = 0;
            }
        }
    }

        public void reset(){
        isReloading = false;
        timeSpentHealing = 0;
        isHealing = false;
        timeSinceReload = 0;
        ammo = 30;
        health = 3;
        timeAlive = 0;
        transform.position = spawnPoint;
        fitness = 0;
        enemy.reset();
        dead = false;
        lastHit = 2;
    }

    public void heal(){
        if(health == maxHealth || lastHit < 2){
            fitness -= 0.5f;
        }
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0, 1, 0, 1);
        if(!isHealing && !isReloading){
            if(!inSight){
                if(health < 2 && lastHit > 2)
                    fitness += 5;
                else if(health == 2 && lastHit > 2)
                    fitness += 3;
            }
            health++;
            timeSpentHealing = 2f;
            isHealing = true;
            if (health > maxHealth)
                health = maxHealth;
        }
    }
    

    public void reload(){
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0, 1, 1, 1);
        if(ammo == 30 || lastHit < 2){
            fitness -= 0.5f;
        }
        if(!isReloading && !isHealing)
        {
            if(!inSight){
                if(ammo < 10 && lastHit > 2)
                    fitness += 5;
                else if(ammo> 10 && ammo < 30 && lastHit > 2)
                    fitness += 2;
            }

            ammo = 30;
            isReloading = true;
            timeSinceReload = 2.0f;
        }
    }

    public void Move(){
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 1, 1);
        if(inSight){
                transform.position = Vector2.MoveTowards(transform.position, enemyPos.position, speed * Time.deltaTime);
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        if(ammo > 10 && health > 2 && !inSight)
            fitness += 0.75f;
    }

    public void Retreat(){
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 0, 1, 1);
        if(lastHit < 2 && !inSight){
            fitness += 2.5f;
        }
        else if(lastHit< 2){
            fitness += 1.25f;
        }
        int furthestIndex = 0;
        float furthestDistance = 0;
        for (int i = 0; i < patrol.moveSpots.Count; i++){
            float dist = Vector2.Distance(patrol.moveSpots[i].position, enemyPos.position);
            if(dist > furthestDistance){
                furthestDistance = dist;
                furthestIndex = i;
            }
        }
        randSpot = furthestIndex;
        transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);

    }
    public void Shoot(){
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 0, 1);

        if (timeSinceLastShot > 0 || isReloading || isHealing || ammo <= 0 )
        {
            return;
        }
        if(inSight){
            shooting.shot.target = enemyPos.position;
            if(ammo > 10 && health > 2){
                fitness += 5;
            }
            else if (ammo > 10 || health > 2)
                fitness += 3;
            else
                fitness += 2;
        }
        else{
            shooting.shot.target = patrol.moveSpots[randSpot].position;
        }    
        Instantiate(shooting.shot, shooting.playerPos.position, Quaternion.identity);
        timeSinceLastShot = shooting.fireRate;
        ammo--;
    }
}
