using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Vector2 target;
    public float speed;
    private float TimeAlive = 0;
    private Transform enemyPos;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = enemyPos.position;
        if(CompareTag("Player"))
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if(CompareTag("AI"))
            target = enemyPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        TimeAlive += Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 1f)
            Destroy(gameObject);
    }
}
