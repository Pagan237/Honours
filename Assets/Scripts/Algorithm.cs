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
        //    individual.chromosomes = new List<int>();
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
        Individuals[activeIndex].active = true;
        if(Individuals[activeIndex].player.timeAlive >= 1){
            Individuals[activeIndex].fitness = Individuals[activeIndex].player.fitness;
            Debug.Log("Fitness: " + Individuals[activeIndex].fitness);
            Individuals[activeIndex].player.reset();
            Individuals[activeIndex].active = false;
            activeIndex++;
            if(activeIndex == population){
                int bestIndex = SelectFittestParent();
                int secondBest = SelectSecondFittestParent();
                Individual individual = Crossover(bestIndex, secondBest);
                int lowestFitness = 0;
                int lowestFitnessIndex = 0;
                for(int i = 0; i < Individuals.Count; i++){
                    if(Individuals[i].fitness <= lowestFitness){
                        lowestFitnessIndex = i;
                        lowestFitness = Individuals[i].fitness;
                    }
                }
                Destroy(Individuals[lowestFitnessIndex]);
                Individuals.RemoveAt(lowestFitnessIndex);
                Individuals.Add(individual);
                activeIndex = 0;
            }
        }
    }

    private Individual Crossover(int index1, int index2)
    {
        Individual ind = gameObject.AddComponent(typeof(Individual)) as Individual;
        for(int i = 0; i < 8; i++){
            float rand = Random.Range(0f, 1f);
            if(rand > 0.5)
                ind.chromosomes.Add(Individuals[index1].chromosomes[0+i]);
            else
                ind.chromosomes.Add(Individuals[index2].chromosomes[0+i]);
        }
        return ind;
    }

    private int SelectFittestParent()
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

    private int SelectSecondFittestParent()
    {
        int highestFitness = 0;
        int secondBestIndex = 0;
        int secondHighestFitness = 0;
        for(int i = 0; i < population; i++)
        {
            if(Individuals[i].fitness >= highestFitness)
                highestFitness = Individuals[i].fitness;
            if(Individuals[i].fitness < highestFitness && Individuals[i].fitness >= secondHighestFitness){
                secondHighestFitness = Individuals[i].fitness;
                secondBestIndex = i;
            }
        }
        return secondBestIndex;
    }
    void Mutate()
    {

    }
}
