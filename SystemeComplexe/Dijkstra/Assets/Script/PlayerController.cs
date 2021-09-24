using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     
     Goal of the game : get all the collectibles
     */

    // Start is called before the first frame update
    void Start()
    {
        Grid grid = transform.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.position = cellPosition;

        Debug.Log("cellPos = " + cellPosition);
        GetMinDistance();
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //float speed = 0.001f;
        
        //Vector3 _vDirection = collectible.transform.localPosition - transform.localPosition;
        //transform.Translate(_vDirection * speed);

        //3 moves : Horizontal, Vertical, Diagonal
    }

    void GetMinDistance()
    {
        GameObject[] collectible = GameObject.FindGameObjectsWithTag("Collectible");

        for(int i=0; i<collectible.Length;i++)
        {
            Vector3 distanceFromPlayer = collectible[i].transform.localPosition - transform.localPosition;
            Debug.Log("Distance =" + distanceFromPlayer);
        }


    }
}
