using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Algorithm : MonoBehaviour
{
    private int population;
    private int activeIndex;
    public List<Individual> Individuals;
    private int generation;
    private int mutateFactor;
    private int ID;
    public Text gen;
    public Text fit;
    public Text state;
    // Start is called before the first frame update
    void Start()
    {
        ID = 0;
        population = 10;
        activeIndex = 0;
        Individuals = new List<Individual>();
        generation = 1;
        for (int i = 0; i < population; i++)
        {
            Individual individual = gameObject.AddComponent(typeof(Individual)) as Individual;
            for (int c = 0; c < 15; c++)
            {
                int rand = Random.Range(1, 6);
                individual.chromosomes.Add(rand);
            }
            individual.ID = ID;
            individual.generationEntry = generation;
            ID++;
            Individuals.Add(individual);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float round = (float) System.Math.Round(Individuals[activeIndex].player.fitness, 1);
        fit.text = "Individual Fitness: " + round;
        gen.text = "Generation: " + generation;
        state.text = "State: " + Individuals[activeIndex].state;
        Individuals[activeIndex].active = true;
        if(Individuals[activeIndex].player.timeAlive >= 30 || Individuals[activeIndex].player.dead == true){
            Individuals[activeIndex].fitness = Individuals[activeIndex].player.fitness;
            Debug.Log("Individual: " +Individuals[activeIndex].ID + " Fitness: " + Individuals[activeIndex].fitness);
            Individuals[activeIndex].player.reset();
            Individuals[activeIndex].active = false;
            activeIndex++;
            //Move on to selection process once all individuals have been evaluated
            if(activeIndex == population){
                //Calculate average fitness
                float averageFitness = 0;
                for(int i = 0; i < population; i++){
                    averageFitness += Individuals[i].fitness;
                }
                averageFitness = averageFitness/10;
                Debug.Log("Average Fitness: " + averageFitness);
                /* ****************** SELECT TWO FITTEST PARENTS STRATEGY ****************
                int parentOneIndex = SelectFittestParent();
                int parentTwoIndex = SelectSecondFittestParent(parentOneIndex);
                
                Debug.Log("Parent One: " + Individuals[parentOneIndex].ID + " Fitness: " + Individuals[parentOneIndex].fitness);
                Debug.Log("Parent Two: " + Individuals[parentTwoIndex].ID + " Fitness: " + Individuals[parentTwoIndex].fitness);
                */
                // ******************************* TOURNAMENT SELECTION STRATEGY **************************
                int parentOneIndex = Tournament(1);
                int parentTwoIndex = Tournament(2);
                Debug.Log("Parent One: " + Individuals[parentOneIndex].ID + " Fitness: " + Individuals[parentOneIndex].fitness);
                Debug.Log("Parent Two: " + Individuals[parentTwoIndex].ID + " Fitness: " + Individuals[parentTwoIndex].fitness);
                // *************************************************************************************************
                //Create new individual using genes from fittest parents
                //**********************************UNIFORM CROSSOVER ****************************************** 
                //Individual individual = uniformCrossover(parentOneIndex, parentTwoIndex);
                //**********************************************************************************************
                //**********************************ONE POINT CROSSOVER*****************************************
                Individual individual = onePointCrossover(parentOneIndex, parentTwoIndex);
                //Find and remove weakest individual from population
                
                float lowestFitness = 0;
                int lowestFitnessIndex = 0;
                for(int i = 0; i < Individuals.Count; i++){
                    if(Individuals[i].fitness <= lowestFitness){
                        lowestFitnessIndex = i;
                        lowestFitness = Individuals[i].fitness;
                    }
                }
                Destroy(Individuals[lowestFitnessIndex]);
                Individuals.RemoveAt(lowestFitnessIndex);
                //Add new individual to population and move to next generation
                individual.ID = ID;
                ID++;
                individual.generationEntry = generation + 1;
                Individuals.Add(individual);
                activeIndex = 0;
                generation++; 
            }
        }
    }

    private Individual uniformCrossover(int index1, int index2)
    {
        Individual ind = gameObject.AddComponent(typeof(Individual)) as Individual;
        for(int c = 0; c < 15; c++){
            //Choose gene from parent depending on rng
            float rand = Random.Range(0f, 1f);
            if(rand > 0.5)
                ind.chromosomes.Add(Individuals[index1].chromosomes[c]);
            else
                ind.chromosomes.Add(Individuals[index2].chromosomes[c]);
        }
        // 1/10 chance of mutation, causing all genes to change
        float mutate = Random.Range(0f, 1f);
        if(mutate >= 0.9f)
            Mutate(ind);
        return ind;
    }

    private Individual onePointCrossover(int index1, int index2){
        Individual ind = gameObject.AddComponent(typeof(Individual)) as Individual;
        int rand = Random.Range(0, 15);
        for(int c = 0; c < 15; c++){
            if(c < rand)
                ind.chromosomes.Add(Individuals[index1].chromosomes[c]);
            else
                ind.chromosomes.Add(Individuals[index2].chromosomes[c]);
        }
        return ind;
    }

    private int SelectFittestParent()
    {
        int bestIndex = 0;
        float highestFitness = 0;
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

    private int SelectSecondFittestParent(int index)
    {
        int secondBestIndex = 0;
        float secondHighestFitness = 0;
        for(int i = 0; i < population; i++)
        {
            if(Individuals[i].fitness <= Individuals[index].fitness && Individuals[i].fitness > secondHighestFitness && i != index){
                secondHighestFitness = Individuals[i].fitness;
                secondBestIndex = i;
            }
        }
        return secondBestIndex;
    }

    int Tournament(int parent){
        float parentFitness = 0;
        int index = 0;
        for(int i = 0; i < population/2; i++){
            if(parent == 1){
                if(parentFitness < Individuals[i].fitness){
                    index = i;
                    parentFitness = Individuals[i].fitness;
                }
            }
            else
            {
                if(parentFitness < Individuals[i +(population/2)].fitness){
                    index = i+(population/2);
                    parentFitness = Individuals[i +(population/2)].fitness;
                }
            }
        }
        return index;
    }

    void Mutate(Individual i)
    {
        for(int c = 0; c < i.chromosomes.Count; c++){
            i.chromosomes[c] = Random.Range(1, 6);
        }
        Debug.Log("Mutated");
    }
}
