using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private List<Individual> Individuals;
    private int generation;
    private int mutateFactor;
    // Start is called before the first frame update
    void Start()
    {
        generation = 1;
        for(int i = 0; i < 10; i++)
        {
            Individual individual = new Individual();
            for (int c = 0; c < 8; c++){
                int rand = Random.Range(1, 5);
                individual.chromosomes.Add(rand);
            }
            Individuals.Add(individual);
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (generation < 20){
            foreach(Individual individual in Individuals){
                individual.active = true;
                while(individual.active == true){
                    if(individual.player.timeAlive >= 30)
                        individual.active = false;
                }
            }
            SelectParents();
            Crossover();
            Mutate();
            generation++;
        }
    }

    void Crossover(){

    }

    void SelectParents(){

    }

    void Mutate(){

    }
}
