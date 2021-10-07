using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

    const int threadGroupSize = 1024;

    public float perceptionRadius = 2.5f;
    public float avoidanceRadius = 1;

    //public BoidSettings settings;
    public ComputeShader compute;
    GameObject[] boids;

    void Start () 
    {
        UpdateBoidList();
    }

    void Update () 
    {
        if (boids.Length != 0) {

            int numBoids = boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++) {
                Movement boidMov = boids[i].GetComponent<Movement>();
                boidData[i].position = boidMov.position;
                boidData[i].direction = boidMov.forward;
            }

            var boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
            boidBuffer.SetData (boidData);

            compute.SetBuffer (0, "boids", boidBuffer);
            compute.SetInt ("numBoids", boids.Length);
            compute.SetFloat ("viewRadius", perceptionRadius);
            compute.SetFloat ("avoidRadius", avoidanceRadius);

            int threadGroups = Mathf.CeilToInt (numBoids / (float) threadGroupSize);
            compute.Dispatch (0, threadGroups, 1, 1);

            boidBuffer.GetData (boidData);

            for (int i = 0; i < boids.Length; i++) {
                BoidsBehavior boidBe = boids[i].GetComponent<BoidsBehavior>();
                boidBe.avgFlockHeading = boidData[i].flockHeading;
                boidBe.centreOfFlockmates = boidData[i].flockCentre;
                boidBe.avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boidBe.numPerceivedFlockmates = boidData[i].numFlockmates;

                boidBe.UpdateBoid();
            }

            boidBuffer.Release ();
        }
        
    }

    public void UpdateBoidList()
    {
        boids = GameObject.FindGameObjectsWithTag("Boid");
    }

    public struct BoidData {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;
        //ComputeBuffer needs the bit size of one line of BoidData, and a struct cannot have initialized values, so we put a getter
        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int);
            }
        }  
    }
}