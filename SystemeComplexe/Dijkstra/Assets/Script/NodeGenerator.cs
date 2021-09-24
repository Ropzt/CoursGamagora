using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public float xMax = 10f;
    public float yMax = 4f;
    // Start is called before the first frame update
    void Start()
    {
        // X = -10/10
        // Y = -4/4
        List<Node> nodeList = new List<Node>();
        GameObject collectible = GameObject.FindGameObjectWithTag("Collectible");

        for (float y = -yMax; y < (yMax+1); y++)
        {
            for(float x = -xMax; x < (xMax+1); x++)
            {

                Node noeud = new Node();
                noeud.setPosition(new Vector3(x, y, 0f));
                noeud.setWalkable(true);
                nodeList.Add(noeud);
                GameObject nodeObj = GameObject.Instantiate(nodePrefab);
                nodeObj.transform.parent = gameObject.transform;
                nodeObj.transform.position = noeud.getPosition();

                Debug.Log("Node Pos = " + nodeObj.transform.position);
                Debug.Log("Collectible Pos = " + collectible.transform.position);

                if (nodeObj.transform.position == collectible.transform.position)
                {
                    Debug.Log("Its a match");
                    SpriteRenderer nodeObjRend = nodeObj.GetComponent<SpriteRenderer>();
                    nodeObjRend.color = Color.green;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
