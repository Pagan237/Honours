using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float fieldOfView = 70f;
    private bool inSight = false;
    private Rigidbody2D rb;
    public GAPatrol patrol;
    private Vector2 enemyLastSeen;
    private Transform enemyPos;
    private Enemy enemy;
    private Shooting shooting;
    public int ammo;
    private float timeSinceLastShot;
    private bool isReloading;
    private float timeSinceReload;
    private float timeAlive;
    public float speed;
    public int health;
    private int maxHealth = 3;
    private float timeSpentHealing;
    private bool isHealing;
    private int randSpot;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector2 enemyDirection = enemyPos.position - transform.position;
        float angle = Vector2.Angle(enemyDirection, direction);
        timeSinceLastShot -= Time.deltaTime;
        if(enemyDirection.x < 5 && enemyDirection.y < 5){
            if(angle < fieldOfView/2 && angle != 0){      
                Shoot(enemyDirection, angle);
            }
            else
                Move(enemyDirection, angle);
        }
        else
            Move(enemyDirection, angle);
        if(health < 2)
            heal();
        if(ammo == 0){
            reload();
        }
    }
    void heal(){
        health++;
        timeSpentHealing = 2f;
        isHealing = true;
        if (health > maxHealth)
            health = maxHealth;
    }

    void reload(){
        ammo = 30;
        isReloading = true;
        timeSinceReload = 2.0f;
    }

    void Move(Vector2 eD, float a){
        eD = enemyPos.position - transform.position;
        a = Vector2.Angle(eD, direction);
        if(eD.x < 5 && eD.y < 5){
            if(a < fieldOfView/2 && a != 0){
                transform.position = Vector2.MoveTowards(transform.position, enemyPos.position, speed * Time.deltaTime);
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, patrol.moveSpots[randSpot].position, speed * Time.deltaTime);

    }

    void Shoot(Vector2 eD, float a){
        if(eD.x < 5 && eD.y < 5){
            if(a < fieldOfView/2 && a != 0){
                shooting.shot.target = enemyPos.position;
                Instantiate(shooting.shot, shooting.playerPos.position, Quaternion.identity);
                timeSinceLastShot = shooting.fireRate;
                ammo--;
            }
            else{
                shooting.shot.target = patrol.moveSpots[randSpot].position;
                Instantiate(shooting.shot, shooting.playerPos.position, Quaternion.identity);
                timeSinceLastShot = shooting.fireRate;
                ammo--;
            }
        }
        else{
            shooting.shot.target = patrol.moveSpots[randSpot].position;
            Instantiate(shooting.shot, shooting.playerPos.position, Quaternion.identity);
            timeSinceLastShot = shooting.fireRate;
            ammo--;
        }    
    }
}
