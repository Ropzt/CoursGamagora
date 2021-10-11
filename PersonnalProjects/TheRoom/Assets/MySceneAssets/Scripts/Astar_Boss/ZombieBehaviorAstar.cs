using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviorAstar : MonoBehaviour
{
    
    private AstarPathfinding shortPathA;
    private Movement mov;
    private GameObject playerObj;
    private NodeGenerator grid;
    private Node noeud;

    [SerializeField] private float steerWeight = 10f;
    private Vector3 nextNodePosition;
    private List<GameObject> path;
    private int pathIndex;
    public bool found = false;

    private void Start()
    {
        InvokeRepeating("Init", 5f, 300f);
    }
    /*
    // Start is called before the first frame update
    void Awake()
    {

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        shortPathA = GetComponent<AstarPathfinding>();
        mov = GetComponent<Movement>();
        noeud = GetComponent<Node>();
        noeud.setZombie(true);

        LinkToNearNeighbours();

        playerObj = GameObject.FindGameObjectWithTag("Player");

        
        // Run A*
        path = shortPathA.findShortestPath(gameObject, playerObj);
        pathIndex = 0;
        nextNodePosition = path[0].transform.position;

    }
    */
    void Init()
    {

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        shortPathA = GetComponent<AstarPathfinding>();
        mov = GetComponent<Movement>();
        noeud = GetComponent<Node>();
        noeud.setZombie(true);

        LinkToNearNeighbours();

        playerObj = GameObject.FindGameObjectWithTag("Player");


        // Run A*
        path = shortPathA.findShortestPath(gameObject, playerObj);
        pathIndex = 0;
        nextNodePosition = path[0].transform.position;
        found = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(found)
        {
            Vector3 nearestNodePosition = getNearestNode();
            float distanceToNextNode = Vector3.Distance(nearestNodePosition, nextNodePosition);
            if (distanceToNextNode < 0.05f)
            {
                if ((pathIndex + 1) < path.Count)
                {
                    pathIndex++;
                    nextNodePosition = path[pathIndex].transform.position;
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                MoveToNextNode(nextNodePosition);
            }
        }
        
    }

    void MoveToNextNode(Vector3 nextNodePosition)
    {
        Vector3 acceleration = Vector3.zero;
        Vector3 direction = nextNodePosition - transform.position;
        Vector3 steeringForce = mov.SteerTowards(direction) * steerWeight;
        acceleration += steeringForce;
        mov.Move(acceleration);
    }

    void LinkToNearNeighbours()
    {
        //Find nearest node
        Vector3 nearestNodePosition = getNearestNode();
        float nearestNodeIndex = ((nearestNodePosition.z + grid.zMax) * ((grid.xMax * 2) + 1)) + (nearestNodePosition.x + grid.xMax);

        
        //Link to nearest node
        GameObject nearestNode = grid.nodeList[(int)nearestNodeIndex];
        noeud.addNeighbourNode(nearestNode);
        Node nearestNodeNode = nearestNode.GetComponent<Node>();
        nearestNodeNode.addNeighbourNode(gameObject);

        //Link to nearest node's neighbours
        foreach (GameObject neighbour in nearestNodeNode.getNeighbourNode())
        {

            Node neighbourNode = neighbour.GetComponent<Node>();
            if (!neighbourNode.isZombie())
            {
                noeud.addNeighbourNode(neighbour);
                neighbourNode.addNeighbourNode(gameObject);
            }
        }
    }

    Vector3 getNearestNode()
    {
        Vector3 nearestNodePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

        return nearestNodePosition;
    }

    void Attack()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {

            foreach (GameObject neighbour in noeud.getNeighbourNode())
            {
                Node neighbourNode = neighbour.GetComponent<Node>();
                neighbourNode.removeNeighbourNode(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
