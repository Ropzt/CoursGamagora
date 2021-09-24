using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Grid grid = transform.parent.GetComponent<Grid>();
        NodeGenerator nodeGen = grid.GetComponent<NodeGenerator>();
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.position = cellPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeRandomPosition(NodeGenerator nodeGen)
    {
        float newX = Random.Range(-nodeGen.xMax, nodeGen.xMax);
        float newY = Random.Range(-nodeGen.yMax, nodeGen.yMax);
        transform.position = new Vector3(newX, newY, 0f);
    }
}
