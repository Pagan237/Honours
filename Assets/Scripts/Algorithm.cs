using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class Algorithm : MonoBehaviour
{
    private string selection;
    private string crossover;
    private int population;
    private int activeIndex;
    public List<Individual> Individuals;
    private int generation;
    private float mutateFactor;
    private int ID;
    public Text gen;
    public Text fit;
    public Text state;
    public Text cross;
    public Text select;
    public Text mutate;
    private string filepath;
    private string convergence;
    private List<int> record = new List<int>();
    private float averageSurvivalTime;
    private List<int> printed = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        convergence = "converge.csv";
        selection = "tournament";
        crossover = "uniform";
        mutateFactor = 0.9f;
        averageSurvivalTime = 0;
        filepath = "results.csv";
        ID = 0;
        population = 10;
        activeIndex = 0;
        Individuals = new List<Individual>();
        generation = 1;
        for (int i = 0; i < population; i++)
        {
            Individual individual = gameObject.AddComponent(typeof(Individual)) as Individual;
            for (int c = 0; c < 16; c++)
            {
                int rand = Random.Range(1, 6);
                individual.chromosomes.Add(rand);
            }
            individual.ID = ID;
            individual.generationEntry = generation;
            ID++;
            Individuals.Add(individual);
        }
        for(int i = 0; i < 3; i++)
            record.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
            selection = "tournament";
        if(Input.GetKeyDown("f"))
            selection = "fittest";
        if(Input.GetKeyDown("u"))
            crossover = "uniform";
        if(Input.GetKeyDown("o"))
            crossover = "single point";
        if(Input.GetKeyDown("up") && mutateFactor < 0.99f)
            mutateFactor = mutateFactor + 0.01f;
        if(Input.GetKeyDown("down") && mutateFactor > 0.01f)
            mutateFactor = mutateFactor - 0.01f;
        float round = (float) System.Math.Round(Individuals[activeIndex].player.fitness, 1);
        mutate.text = "Mutation rate: " + System.Math.Round(mutateFactor, 2);
        cross.text = crossover;
        select.text = selection;
        fit.text = "Individual Fitness: " + round;
        gen.text = "Generation: " + generation;
        state.text = "State: " + Individuals[activeIndex].state;
        Individuals[activeIndex].active = true;
        if(Individuals[activeIndex].player.timeAlive >= 30 || Individuals[activeIndex].player.dead || Individuals[activeIndex].player.enemy.dead){
            if(Individuals[activeIndex].player.timeAlive >= 30){
                record[1]++;
            }
            else if(Individuals[activeIndex].player.dead){
                record[2]++;
            }
            else{
                record[0]++;
            }
            averageSurvivalTime += Individuals[activeIndex].player.timeAlive;
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
                averageFitness = averageFitness/population;
                averageSurvivalTime = averageSurvivalTime/population;
                Debug.Log("Average Fitness: " + averageFitness);
                int parentOneIndex;
                int parentTwoIndex;
                /* ****************** SELECT TWO FITTEST PARENTS STRATEGY ****************/
                if(selection == "fittest"){
                    parentOneIndex = SelectFittestParent();
                    parentTwoIndex = SelectSecondFittestParent(parentOneIndex);
                }
                // ******************************* TOURNAMENT SELECTION STRATEGY **************************//
                else{
                    parentOneIndex = Tournament(1);
                    parentTwoIndex = Tournament(2);
                }
                // *************************************************************************************************
                //Create new individual using genes from fittest parents
                Individual individual;
                //**********************************UNIFORM CROSSOVER ****************************************** 
                if(crossover == "uniform")
                    individual = uniformCrossover(parentOneIndex, parentTwoIndex);
                //**********************************************************************************************
                //**********************************ONE POINT CROSSOVER*****************************************
                else
                    individual = onePointCrossover(parentOneIndex, parentTwoIndex);
                //Find and remove weakest individual from population
                
                float lowestFitness = 0;
                int lowestFitnessIndex = 0;
                for(int i = 0; i < Individuals.Count; i++){
                    if(Individuals[i].fitness <= lowestFitness){
                        lowestFitnessIndex = i;
                        lowestFitness = Individuals[i].fitness;
                    }
                }
                using(TextWriter sw = File.AppendText(filepath)){
                    string ratio = record[0].ToString() + " - " + record[1].ToString() + " - " + record[2].ToString();
                    string survival = averageSurvivalTime.ToString("F2");
                    string g = generation.ToString();
                    string avg = averageFitness.ToString("F2");
                    string best = Individuals[SelectFittestParent()].fitness.ToString("F2");
                    sw.WriteLine("{0},{1},{2},{3},{4}", g, avg, best, survival, ratio);
                    sw.NewLine = "\n";
                }
                using(TextWriter sw = File.AppendText(convergence)){
                    string genes = null;
                    for(int i = 0; i < population; i++){
                        if(!printed.Contains(Individuals[i].ID)){
                            for(int j = 0; j < 16; j++){
                                if(j == 0)
                                    genes = Individuals[i].chromosomes[j].ToString();
                                else
                                    genes = genes + ", " + Individuals[i].chromosomes[j].ToString(); 
                            }
                            sw.WriteLine("{0}, {1}", Individuals[i].ID.ToString(), genes);
                            sw.NewLine = "\n";
                            printed.Add(Individuals[i].ID);
                        }
                    }
                }
                averageSurvivalTime = 0;
                for(int i = 0; i < record.Count; i++)
                    record[i] = 0;
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
        for(int c = 0; c < 16; c++){
            //Choose gene from parent depending on rng
            float rand = Random.Range(0f, 1f);
            if(rand > 0.5)
                ind.chromosomes.Add(Individuals[index1].chromosomes[c]);
            else
                ind.chromosomes.Add(Individuals[index2].chromosomes[c]);
        }
        // 1/10 chance of mutation, causing all genes to change
        float mutate = Random.Range(0f, 1f);
        if(mutate >= mutateFactor)
            Mutate(ind);
        return ind;
    }

    private Individual onePointCrossover(int index1, int index2){
        Individual ind = gameObject.AddComponent(typeof(Individual)) as Individual;
        int rand = Random.Range(0, 16);
        for(int c = 0; c < 16; c++){
            if(c < rand)
                ind.chromosomes.Add(Individuals[index1].chromosomes[c]);
            else
                ind.chromosomes.Add(Individuals[index2].chromosomes[c]);
        }
        float mutate = Random.Range(0f, 1f);
        if(mutate >= mutateFactor)
            Mutate(ind);
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
