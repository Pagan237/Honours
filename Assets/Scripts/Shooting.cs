using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Projectile shot;
    public Transform playerPos;
    public float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
        fireRate = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
