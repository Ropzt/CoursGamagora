using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private float repeatRate;
    [SerializeField] private float repeatRateMin = 2.0f;
    [SerializeField] private float repeatRateMax = 8.0f;
    GameObject boidManagerObj;
    SpawnManager spBoidManager;
    BoidManager boidManager;
    private bool hasSpawnedBoids = false;

    // Start is called before the first frame update
    void Awake()
    {
        repeatRate = Random.Range(repeatRateMin, repeatRateMax);
        InvokeRepeating("SpawnZombie", startDelay, repeatRate);
        boidManagerObj = GameObject.FindGameObjectWithTag("BoidManager");
        spBoidManager = boidManagerObj.GetComponent<SpawnManager>();
        boidManager = boidManagerObj.GetComponent<BoidManager>();
    }

    
    void SpawnZombie()
    {
        GameObject zombieObj = GameObject.Instantiate(zombiePrefab, gameObject.transform);
        if(!hasSpawnedBoids)
        {
            spBoidManager.CreateBoids();
            boidManager.UpdateBoidList();
            hasSpawnedBoids = true;
        }
    }
}
