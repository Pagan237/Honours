using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private float waitTime;
    public float startWaitTime;
//positions AI can move to
    public List<Transform> moveSpots;

    //private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float y = Random.Range(-5, 5);
            float x = Random.Range(-10, 10);
            moveSpots[i].position = new Vector2(x, y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
