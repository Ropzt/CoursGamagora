using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _boidNumber;
    [SerializeField] GameObject _boidPrefab;
    // Start is called before the first frame update
    
    public void CreateBoids()
    {
        for (int i = 0; i < _boidNumber; i++)
        {
            GameObject boid = Instantiate(_boidPrefab);
            boid.transform.parent = transform;
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, -1f);
            float z = Random.Range(-5f, 5f);
            boid.transform.position = new Vector3(x, y, z);
        }
    }
      
    // Update is called once per frame
    void Update()
    {
        
    }
}
