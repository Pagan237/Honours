using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private List<GameObject> AIs = new List<GameObject>();
    public Transform[] spawnSpots;

    public GameObject AI;
    private float timeBtwSpawns;

    public float startTimeBtwSpawns;
    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwSpawns <=0)
        {
            for (int i = 0; i < spawnSpots.Length; i++)
            {
                if(AIs.Count >= 2)
                {
                    AIs.Clear();
                }
                AIs.Add(AI);
                Instantiate(AIs[i], spawnSpots[i].position, Quaternion.identity);
                timeBtwSpawns = startTimeBtwSpawns;
            }
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
