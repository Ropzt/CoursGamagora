using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneManager : MonoBehaviour
{
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseAttack;

    [HideInInspector] public Vector3 baseGenome;

    /*
    [SerializeField] private float timeWeight;
    [SerializeField] private float damageWeight;
    private float bestTime;
    private float bestDamage;
    */

    private float bestScore;


    void Start()
    {
        Init();
    }

    
    void Update()
    {
        
    }

    void Init()
    {
        baseGenome = new Vector3(baseHealth,baseSpeed, baseAttack);
        bestScore = 0;
    }

    public void CheckForNewBest(float damage, Vector3 geneVector)
    {
        if(damage > bestScore)
        {
            bestScore = damage;
            UpdateBaseGenome(geneVector);
        }
    }

    void UpdateBaseGenome(Vector3 geneVector)
    {
        baseGenome = geneVector;
        Debug.Log("New Best Genome = " + baseGenome);
    }
}
