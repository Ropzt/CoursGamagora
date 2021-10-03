using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsBehavior : MonoBehaviour
{
    [Header("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = .27f;
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;

    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = 1;

    Movement mov;
    CollisionRange colRange;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<Movement>();
        colRange = GetComponent<CollisionRange>();
    }

    // Update is called once per frame
    public void UpdateBoid()
    {
        //Reset acceleration
        Vector3 acceleration = Vector3.zero;

        if (numPerceivedFlockmates != 0)
        {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - mov.position);

            var alignmentForce = mov.SteerTowards(avgFlockHeading) * alignWeight;
            var cohesionForce = mov.SteerTowards(offsetToFlockmatesCentre) * cohesionWeight;
            var seperationForce = mov.SteerTowards(avgAvoidanceHeading) * seperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision())
        {
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 collisionAvoidForce = mov.SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }
        
        mov.Move(acceleration);
    }

    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(mov.position, boundsRadius, mov.forward, out hit, collisionAvoidDst, obstacleMask))
        {
            return true;
        }
        else { }
        return false;
    }

    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = colRange.rayDir.ToArray();

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = mov.cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(mov.position, dir);
            if (!Physics.SphereCast(ray, boundsRadius, collisionAvoidDst, obstacleMask))
            {
                return dir;
            }
        }
        return mov.forward;
    }
}
