using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private float repeatRate;
    [SerializeField] private float repeatRateMin = 2.0f;
    [SerializeField] private float repeatRateMax = 8.0f;

    // Start is called before the first frame update
    void Awake()
    {
        repeatRate = Random.Range(repeatRateMin,repeatRateMax);
        InvokeRepeating("SpawnZombie",startDelay,repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnZombie()
    {
        GameObject zombieObj = GameObject.Instantiate(zombiePrefab,gameObject.transform);
    }
}
