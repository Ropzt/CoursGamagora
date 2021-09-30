using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spawnManagerPrefab;
    public float xMax;
    public float zMax;
    public List<GameObject> nodeList;
    [SerializeField] private int nb_Spawn = 3;

    // Start is called before the first frame update
    void Start()
    {
        
        Transform groundTrans = GameObject.FindGameObjectWithTag("Ground").GetComponent<Transform>();
        xMax = (groundTrans.localScale.x)*5f;
        zMax = (groundTrans.localScale.z)*5f;
        nodeList = new List<GameObject>();

        GenerateNodeGrid();
        GenerateNeighbours();

        GeneratePlayer();
        GenerateSpawns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNeighbours()
    {
        int nodeIndex = 0;

        for (float z = -zMax; z < (zMax+1); z++)
        {
            for(float x = -xMax; x < (xMax+1); x++)
            {

                Node noeud = nodeList[nodeIndex].GetComponent<Node>();
                Node upperNode = new Node();
                Node lowerNode = new Node();
                Node rightNode = new Node();
                Node leftNode = new Node();
                //Upper neighbour
                if ( (nodeIndex + (2*zMax)) < ((2*xMax)*(2*zMax)) )
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex + (2*zMax)+1)]);
                    upperNode = nodeList[(int)(nodeIndex + (2 * zMax) + 1)].GetComponent<Node>();
                }

                //Lower neighbour
                if ( (nodeIndex - (2*zMax)) >= 0)
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex - (2*zMax))]);
                    lowerNode = nodeList[(int)(nodeIndex - (2 * zMax))].GetComponent<Node>();
                }

                //Right neighbour
                if ( (nodeIndex + 1) < ((2*xMax)*(2*zMax)) )
                {
                    noeud.addNeighbourNode(nodeList[(nodeIndex + 1)]);
                    rightNode = nodeList[(nodeIndex + 1)].GetComponent<Node>();
                }

                //Left neighbour
                if ( (nodeIndex - 1) >= 0)
                {
                    noeud.addNeighbourNode(nodeList[(nodeIndex - 1)]);
                    leftNode = nodeList[(nodeIndex - 1)].GetComponent<Node>();
                }


                //Need to prevent links between the node and diagonal neighbours that trespass a wall (Diagonal UpperLeft is ok if Upper OR Left aren't walls)

                //Upper Right neighbour
                if ( ((nodeIndex + (2 * zMax)+1) < ((2 * xMax) * (2 * zMax))) & (upperNode.isWalkable() & rightNode.isWalkable()) )
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex + (2 * zMax) + 2)]);
                }
                //Upper Left neighbour
                if (((nodeIndex + (2 * zMax)-1) < ((2 * xMax) * (2 * zMax))) & (upperNode.isWalkable() & leftNode.isWalkable()))
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex + (2 * zMax))]);
                }
                //Lower Right neighbour
                if (((nodeIndex - (2 * zMax)+1) >= 0) & (lowerNode.isWalkable() & rightNode.isWalkable()))
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex - (2 * zMax))+1]);
                }
                //Lower Left neighbour
                if (((nodeIndex - (2 * zMax)-1) >= 0) & (lowerNode.isWalkable() & leftNode.isWalkable()))
                {
                    noeud.addNeighbourNode(nodeList[(int)(nodeIndex - (2 * zMax))-1]);
                }

                nodeIndex++;              
            }
        }
    }

    void GenerateNodeGrid()
    {
        for (float z = -zMax; z < (zMax + 1); z++)
        {
            for (float x = -xMax; x < (xMax + 1); x++)
            {
                GameObject nodeObj = GameObject.Instantiate(nodePrefab);
                nodeObj.transform.parent = gameObject.transform;
                Node noeud = nodeObj.GetComponent<Node>();
                noeud.setPosition(new Vector3(x, 0f, z));
                nodeObj.transform.position = noeud.getPosition();

                if (nodeObj.transform.position.z == zMax |
                    nodeObj.transform.position.z == -zMax |
                    nodeObj.transform.position.x == xMax |
                    nodeObj.transform.position.x == -xMax |
                    Random.value < 0.2f)
                {
                    noeud.setWalkable(false);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = nodeObj.transform;
                    cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    cube.transform.localScale = new Vector3(1f, 5f, 1f);
                }
                else
                {
                    noeud.setWalkable(true);
                }

                nodeList.Add(nodeObj);
            }
        }
    }

    void GeneratePlayer()
    {
        bool foundPosPlayer = false;
        Vector3 playerPos = new Vector3();

        while (!foundPosPlayer)
        {
            int index = (int)Mathf.Round(Random.Range((float)(nodeList.Count / 1.2f), (float)nodeList.Count));
            Node nodePosition = nodeList[index].GetComponent<Node>();
            if (nodePosition.isWalkable())
            {
                foundPosPlayer = true;
                playerPos = nodePosition.getPosition();
            }
        }
        GameObject playerObj = GameObject.Instantiate(playerPrefab, playerPos, new Quaternion());
        PlayerController pCtrl = playerObj.GetComponent<PlayerController>();
        pCtrl.LinkToNearNeighbours();
    }

    void GenerateSpawns()
    {
        
        bool foundPosSpawn = false;
        Vector3 spawnPos = new Vector3();

        for (int i = 0; i < nb_Spawn; i++)
        {
            while (!foundPosSpawn)
            {
                int index = (int)Mathf.Round(Random.Range(0f, (float)(nodeList.Count / 5)));
                Node nodePosition = nodeList[index].GetComponent<Node>();
                if (nodePosition.isWalkable())
                {
                    foundPosSpawn = true;
                    spawnPos = nodePosition.getPosition();
                    GameObject spawnObj = GameObject.Instantiate(spawnManagerPrefab, spawnPos, new Quaternion());
                }
            }
            foundPosSpawn = false;
        }
    }
}
