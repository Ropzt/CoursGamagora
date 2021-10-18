using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private NodeGenerator grid;
    private Node noeud;

    [SerializeField] private float health;
    [SerializeField] private float range;
    [SerializeField] private float attack;
    [HideInInspector] public Vector3 nearestNodePosition;
    [HideInInspector] private Vector3 previousNearestNodePosition;

    public delegate void MoveDelegate();
    public event MoveDelegate moveEvent;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        noeud = GetComponent<Node>();
        noeud.setPlayer(true);
        nearestNodePosition = InitNearestNode();
    }

    private void Update()
    {
        nearestNodePosition = getNearestNode();
        if(nearestNodePosition != previousNearestNodePosition)
        {
            Debug.Log("Need to update path");
            NodeChange();
        }
    }

    void NodeChange()
    {
        if(moveEvent != null)
        {
            moveEvent();
        }
    }

    public void LinkToNearNeighbours()
    {
        //Find nearest node
        float nearestNodeIndex = getNearestNodeIndex();

        //Link to nearest node
        GameObject nearestNode = grid.nodeList[(int)nearestNodeIndex];
        noeud.addNeighbourNode(nearestNode);
        Node nearestNodeNode = nearestNode.GetComponent<Node>();
        nearestNodeNode.addNeighbourNode(gameObject);

        
        //Link to nearest node's neighbours
        foreach(GameObject neighbour in nearestNodeNode.getNeighbourNode())
        {
            Node neighbourNode = neighbour.GetComponent<Node>();
            if(!neighbourNode.isPlayer())
            {
                noeud.addNeighbourNode(neighbour);
                neighbourNode.addNeighbourNode(gameObject);
            }
        }
        
    }

    public int getNearestNodeIndex()
    {
        //Find nearest node
        nearestNodePosition = getNearestNode();
        int nearestNodeIndex = (int)(((nearestNodePosition.z + grid.zMax) * ((grid.xMax * 2) + 1)) + (nearestNodePosition.x + grid.xMax));
        return nearestNodeIndex;
    }

    Vector3 InitNearestNode()
    {
        nearestNodePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        return nearestNodePosition;
    }

    Vector3 getNearestNode()
    {
        previousNearestNodePosition = nearestNodePosition;
        nearestNodePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        return nearestNodePosition;
    }


    public void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range)) //10 is Enemy LayerMask
        {
            if (hit.transform.tag == "Enemy")
            {
                Debug.Log(hit.transform.name);
                hit.transform.GetComponent<ZombieBehaviorAstar>().TakeDamage(attack);
            }
        }   
    }
    
    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
    }
    
}
