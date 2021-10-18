using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ZombieBehaviorAstar : MonoBehaviour
{
    
    private GridNode4Threading shortPathA;
    private Movement mov;
    private NodeGenerator grid;
    private Node noeud;
    private Genome genome;

    private GameObject playerObj;
    private PlayerController playerCtrl;
    
    [HideInInspector] public List<Vector3> path;

    [SerializeField] private float steerWeight = 10f;
    [SerializeField] private float health;
    [SerializeField] private float attack;
    [SerializeField] private float range;
    private bool isAttacking = false;

    private Vector3 nextNodePosition;
    private int pathIndex;
    public bool isPathReady = false;

    public delegate void DieDelegate();
    public event DieDelegate dieEvent;

    /*
    private void Start()
    {
        InvokeRepeating("Init", 5f, 300f);
    }
    */

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Un ennemi apparait");
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        shortPathA = GetComponent<GridNode4Threading>();
        mov = GetComponent<Movement>();
        noeud = GetComponent<Node>();
        noeud.setZombie(true);

        //LinkToNearNeighbours();

        //Awake genome and retrieve values
        genome = GetComponent<Genome>();
        genome.Init();
        health = genome.geneVector.x;
        attack = genome.geneVector.z;

        pathIndex = 0;
        shortPathA.Init();
    }

    /*
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
    */
    // Update is called once per frame
    void Update()
    {
        if(isPathReady)
        {
            Vector3 nearestNodePosition = getNearestNode();
            float distanceToNextNode = Vector3.Distance(nearestNodePosition, nextNodePosition);
            if (distanceToNextNode < 0.05f)
            {
                if ((pathIndex + 1) < path.Count)
                {
                    pathIndex++;
                    nextNodePosition = path[pathIndex];
                }
                else if (!isAttacking)
                {
                    Attack();
                    StartCoroutine(Attackdelay());
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

    public int getNearestNodeIndex()
    {
        //Find nearest node
        Vector3 nearestNodePosition = getNearestNode();
        int nearestNodeIndex = (int)(((nearestNodePosition.z + grid.zMax) * ((grid.xMax * 2) + 1)) + (nearestNodePosition.x + grid.xMax));
        return nearestNodeIndex;
    }

    public void InitPath(List<Vector3> pathCompleted)
    {
        path = pathCompleted;
        nextNodePosition = path[0];
        isPathReady = true;
    }

    void Attack()
    {
        isAttacking = true;
        //if distance to endNode < X, then start coroutine Attack : reduce health of player by K and then wait for T seconds before repeating, if not dead while waiting
        RaycastHit hit;
        Vector3 direction = playerObj.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if(hit.transform.tag == "Player")
            {
                Debug.Log("ENEMY ATTACK");
                playerCtrl.TakeDamage(attack);
                genome.damageDone += attack;
            }
        }
    }

    IEnumerator Attackdelay()
    {
        yield return new WaitForSecondsRealtime(10f);
        isAttacking = false;
    }

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        if(health<0)
        {
            //Send data to gene manager
            genome.SendGenomeData();

            if(dieEvent != null)
            {
                dieEvent();
            }

            //Die
            foreach (GameObject neighbour in noeud.getNeighbourNode())
            {
                Node neighbourNode = neighbour.GetComponent<Node>();
                neighbourNode.removeNeighbourNode(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
