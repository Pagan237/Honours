using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    public List<Individual> Individuals;
    private int generation;
    private int mutateFactor;
    // Start is called before the first frame update
    void Start()
    {
        Individuals = new List<Individual>();
        generation = 1;
        for (int i = 0; i < 10; i++)
        {
            Individual individual = gameObject.AddComponent(typeof(Individual)) as Individual;
            individual.chromosomes = new List<int>();
            for (int c = 0; c < 8; c++)
            {
                int rand = Random.Range(1, 5);
                individual.chromosomes.Add(rand);
                Debug.Log(individual.chromosomes);
            }
            Individuals.Add(individual);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Crossover()
    {

    }

    void SelectParents()
    {

    }

    void Mutate()
    {

    }
}
