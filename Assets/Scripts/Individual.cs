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
    // Start is called before the first frame update

    private void Awake()
    {
        fitness = 0;
        active = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            if (player.health < 2 && player.ammo < 10 && player.inSight == true)
                state = 0;
            if (player.health < 2 && player.ammo < 10 && player.inSight == false)
                state = 1;
            if (player.health >= 2 && player.ammo < 10 && player.inSight == true)
                state = 2;
            if (player.health >= 2 && player.ammo < 10 && player.inSight == false)
                state = 3;
            if (player.health < 2 && player.ammo >= 10 && player.inSight == true)
                state = 4;
            if (player.health < 2 && player.ammo >= 10 && player.inSight == false)
                state = 5;
            if (player.health >= 2 && player.ammo >= 10 && player.inSight == true)
                state = 6;
            if (player.health >= 2 && player.ammo >= 10 && player.inSight == false)
                state = 7;
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
