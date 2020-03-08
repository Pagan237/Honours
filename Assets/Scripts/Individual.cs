using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public Player player;
    public float fitness;
    public bool active;
    public List<int> chromosomes;
    public int state;
    public bool mutated;
    public List<int> parentIDs;
    public int ID;
    public int generationEntry;
    private enum states{
        health = 1 << 0,
        ammo = 1 << 1,
        sight = 1 << 2,
        lastHit = 1 << 3
    };
    // Start is called before the first frame update

    private void Awake()
    {
        mutated = false;
        parentIDs = new List<int>();
        fitness = 0;
        active = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        chromosomes = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            int highHealth = player.health < 2 ? 0 : (int)states.health;
            int highAmmo = player.ammo < 10 ? 0 : (int)states.ammo;
            int sight = !player.inSight ? 0 : (int)states.sight;
            int lastHit = player.lastHit < 2 ? 0 : (int)states.lastHit;
            state = highHealth + highAmmo + sight + lastHit;
            action(chromosomes[state]);
        }
    }

    void action(int i)
    {
        if (i == 1)
            player.Shoot();
        if (i == 2)
            player.heal();
        if (i == 3)
            player.Retreat();
        if (i == 4)
            player.Move();
        if (i == 5)
            player.reload();
    }
}
