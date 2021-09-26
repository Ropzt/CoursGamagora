using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    Dijkstra shortPath;
    GameObject playerObj;
    NodeGenerator grid;
    Node noeud;
    public float speed = 100f;
    private Vector3 nextNodePosition;
    List<GameObject> path;
    int pathIndex;

    // Start is called before the first frame update
    void Awake()
    {
        
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        shortPath = GetComponent<Dijkstra>();
        noeud = GetComponent<Node>();

        LinkToNearNeighbours();

        playerObj = GameObject.FindGameObjectWithTag("Player");


        path = shortPath.findShortestPath(gameObject, playerObj);
        pathIndex=0;
        nextNodePosition = path[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nearestNodePosition = getNearestNode();
        float distanceToNextNode = Vector3.Distance(nearestNodePosition, nextNodePosition);
        if(distanceToNextNode < 0.2f)
        {
            if((pathIndex+1) < path.Count)
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

    void MoveToNextNode(Vector3 nextNodePosition)
    {
        Vector3 direction = nextNodePosition - transform.position;
        direction.Normalize();
        transform.Translate(direction*speed*0.2f);
        transform.forward=direction;
    }

    void LinkToNearNeighbours()
    {
        //Find nearest node
        Vector3 nearestNodePosition = getNearestNode();
        float nearestNodeIndex = ((nearestNodePosition.z+grid.zMax)*((grid.xMax*2)+1))+(nearestNodePosition.x+grid.xMax);

        //Link to nearest node
        GameObject nearestNode = grid.nodeList[(int)nearestNodeIndex];
        noeud.addNeighbourNode(nearestNode);
        Node nearestNodeNode = nearestNode.GetComponent<Node>();
        nearestNodeNode.addNeighbourNode(gameObject);
        
        //Link to nearest node's neighbours
        foreach(GameObject neighbour in nearestNodeNode.getNeighbourNode())
        {
            noeud.addNeighbourNode(neighbour);
            Node neighbourNode = neighbour.GetComponent<Node>();
            neighbourNode.addNeighbourNode(gameObject);
        }
    }

    Vector3 getNearestNode()
    {
        Vector3 nearestNodePosition = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),Mathf.Round(transform.position.z));
        
        return nearestNodePosition;
    }
    void Attack()
    {

    }
}
