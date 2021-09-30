using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private NodeGenerator grid;
    private Node noeud;
    private GameObject camObj;
    private Camera cam;
    [SerializeField]  private float RotationSpeed = 200f;
    [SerializeField]  private GameObject projectilePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        noeud = GetComponent<Node>();
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camObj.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
        


        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
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
            noeud.addNeighbourNode(neighbour);
            Node neighbourNode = neighbour.GetComponent<Node>();
            neighbourNode.addNeighbourNode(gameObject);
        }
    }

    public void UpdateCamera()
    {
        float mousePos = Input.GetAxisRaw("Mouse X");
        transform.Rotate(new Vector3(0f, ((mousePos * 0.02f) * RotationSpeed), 0f), Space.Self);
    }

    public void Shoot()
    {
        GameObject projectileObj = GameObject.Instantiate(projectilePrefab, transform);
    }
}
