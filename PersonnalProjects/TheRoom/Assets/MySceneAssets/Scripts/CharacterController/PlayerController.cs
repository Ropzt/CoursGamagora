using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private NodeGenerator grid;
    private Node noeud;
    
    //[SerializeField]  private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        noeud = GetComponent<Node>();
        noeud.setPlayer(true);
    }

    public void LinkToNearNeighbours()
    {
        //Find nearest node
        Vector3 nearestNodePosition = new Vector3(Mathf.Floor(transform.position.x),Mathf.Floor(transform.position.y),Mathf.Floor(transform.position.z));
        float nearestNodeIndex = ((nearestNodePosition.z+grid.zMax)*((grid.xMax*2)+1))+(nearestNodePosition.x+grid.xMax);

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

    /*
    public void Shoot()
    {
        GameObject projectileObj = GameObject.Instantiate(projectilePrefab, transform);
    }
    */
    
}
