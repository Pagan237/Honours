using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public Player player;
    public int fitness;
    public bool active;
    public List<int> chromosomes;
    private int state;
    public int ID;
    public int generationEntry;
    private enum states{
        health = 1 << 0,
        ammo = 1 << 1,
        sight = 1 << 2
    };
    // Start is called before the first frame update

    private void Awake()
    {
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
            /* 
            if (player.health < 2 && player.ammo < 10 && player.inSight == true)
                state = 0;
            else if (player.health < 2 && player.ammo < 10 && player.inSight == false)
                state = 1;
            else if (player.health >= 2 && player.ammo < 10 && player.inSight == true)
                state = 2;
            else if (player.health >= 2 && player.ammo < 10 && player.inSight == false)
                state = 3;
            else if (player.health < 2 && player.ammo >= 10 && player.inSight == true)
                state = 4;
            else if (player.health < 2 && player.ammo >= 10 && player.inSight == false)
                state = 5;
            else if (player.health >= 2 && player.ammo >= 10 && player.inSight == true)
                state = 6;
            else if (player.health >= 2 && player.ammo >= 10 && player.inSight == false)
                state = 7;
            */
            int highHealth = player.health < 2 ? 0 : (int)states.health;
            int highAmmo = player.ammo < 10 ? 0 : (int)states.ammo;
            int sight = !player.inSight ? 0 : (int)states.sight;
            state = highHealth + highAmmo + sight;
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
