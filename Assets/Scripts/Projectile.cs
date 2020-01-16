using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector2 target;
    public float speed;
    private float TimeAlive = 0;
    private Transform enemyPos;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyPos = GameObject.FindGameObjectWithTag("AI").GetComponent<Transform>();
        //target = enemyPos.position;
        if(transform.position == playerPos.position)
            target = enemyPos.position;
        else
            target = playerPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        TimeAlive += Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.1f)
            Destroy(gameObject);
    }
}
