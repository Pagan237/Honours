using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private int population;
    private int activeIndex;
    public List<Individual> Individuals;
    private int generation;
    private int mutateFactor;
    // Start is called before the first frame update
    void Start()
    {
        population = 10;
        activeIndex = 0;
        Individuals = new List<Individual>();
        generation = 1;
        for (int i = 0; i < population; i++)
        {
            Individual individual = gameObject.AddComponent(typeof(Individual)) as Individual;
            individual.chromosomes = new List<int>();
            for (int c = 0; c < 8; c++)
            {
                int rand = Random.Range(1, 5);
                individual.chromosomes.Add(rand);
            }
            Individuals.Add(individual);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(activeIndex);
        Individuals[activeIndex].active = true;
        if(Individuals[activeIndex].player.timeAlive >= 5){
            Individuals[activeIndex].fitness = Individuals[activeIndex].player.fitness;
            Debug.Log("Fitness: " + Individuals[activeIndex].fitness);
            Individuals[activeIndex].player.reset();
            Individuals[activeIndex].active = false;
            activeIndex++;
            if(activeIndex == population){
                int bestIndex = SelectParents();
                int secondBest = SelectParents();
                Crossover();
                activeIndex = 0;
            }
        }
    }

    void Crossover()
    {

    }

    private int SelectParents()
    {
        int bestIndex = 0;
        int highestFitness = 0;
        for(int i = 0; i < population; i++)
        {
            if(Individuals[i].fitness >= highestFitness)
            {
                highestFitness = Individuals[i].fitness;
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    void Mutate()
    {

    }
}
