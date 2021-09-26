using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    private float startDelay = 2.0f;
    private float repeatRate;
    
    // Start is called before the first frame update
    void Awake()
    {
        repeatRate = Random.Range(2f,8f);
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
