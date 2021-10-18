using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float minSpeed = 2;
    [HideInInspector] public float maxSpeed;
    public float maxSteerForce = 3;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // Cached
    public Transform cachedTransform;
    
    // Start is called before the first frame update
    public void Init()
    {
        //Separate transform.position from computation by using temp values (cachedTransform,position,forward)
        cachedTransform = transform;
        position = cachedTransform.position;
        forward = cachedTransform.forward;

        //Set initial movement
        velocity = transform.forward * minSpeed;

        //Set Speed decided by evolution
        Genome genome = GetComponent<Genome>();
        maxSpeed = genome.geneVector.y;
    }

    public void Move(Vector3 acceleration)
    {
        velocity += acceleration * Time.deltaTime;
        
        //Offset and normalization (Fancy things by Sebastian Lague
        //but it appears to introduce randomness in the directions, preventing gradient descent-like behavior)
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        velocity = dir * speed;

        
        cachedTransform.position += velocity * Time.deltaTime;
        
        
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
    }

    public Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, maxSteerForce);
    }
}
