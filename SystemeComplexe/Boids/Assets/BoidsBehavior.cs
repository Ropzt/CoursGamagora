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

    Movement mov;
    CollisionRange colRange;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<Movement>();
        colRange = GetComponent<CollisionRange>();
    }

    // Update is called once per frame
    void Update()
    {
        //Reset acceleration
        Vector3 acceleration = Vector3.zero;

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
