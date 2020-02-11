using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public Projectile shot;
    private float TimeSinceLastShot;

    public float StartTimeBetweenShots;
    public Transform AIpos;

    // Start is called before the first frame update
    void Start()
    {
        AIpos = GetComponent<Transform>();  
        StartTimeBetweenShots = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
