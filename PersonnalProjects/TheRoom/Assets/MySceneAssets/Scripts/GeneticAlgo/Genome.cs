using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
    GeneManager geneManager;
    private Vector3 baseGenome;

    [HideInInspector] public Vector3 geneVector;
    [HideInInspector] public float damageDone = 0f;

    [SerializeField] private float mutationOffset;

    public void Init()
    {
        //Get base from GeneManager
        geneManager = GameObject.FindGameObjectWithTag("GeneManager").GetComponent<GeneManager>();
        baseGenome = geneManager.baseGenome;

        //Apply Mutation
        geneVector = GenerateMutation();
    }

    Vector3 GenerateMutation()
    {
        //Apply random proportional mutations to genes
        float healthGene = Random.Range(baseGenome.x - (mutationOffset * baseGenome.x), baseGenome.x + (mutationOffset * baseGenome.x));
        float speedGene  = Random.Range(baseGenome.y - (mutationOffset * baseGenome.y), baseGenome.y + (mutationOffset * baseGenome.y));
        float attackGene = Random.Range(baseGenome.z - (mutationOffset * baseGenome.z), baseGenome.z + (mutationOffset * baseGenome.z));

        return new Vector3(healthGene, speedGene, attackGene);
    }

    public void SendGenomeData()
    {
        //send record
        geneManager.CheckForNewBest(damageDone, geneVector);
    }
}
