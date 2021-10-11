using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private int _boidNumber;
    [SerializeField] GameObject _boidPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _boidNumber; i++)
        {
            GameObject boid = Instantiate(_boidPrefab);
            boid.transform.parent = transform;
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            float z = Random.Range(-5f, 5f);
            boid.transform.position = new Vector3(x, y, z);
        }
    }
}
