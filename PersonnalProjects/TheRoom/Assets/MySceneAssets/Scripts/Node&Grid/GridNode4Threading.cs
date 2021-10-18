using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GridNode4Threading : MonoBehaviour
{
    
    private int startIndex;
    private int endIndex;
    private bool pathFound = false;
    private List<Vector3> path;

    [HideInInspector] public GridNodeData[] gridNodeDataOriginal;
    [HideInInspector] public GridNodeData[] gridNodeData;

    GameObject playerObj;
    ZombieBehaviorAstar zba;

    public void Init() 
    {
        gridNodeDataOriginal = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>().gridNodeData;
        gridNodeData = gridNodeDataOriginal;
        zba = GetComponent<ZombieBehaviorAstar>();
        
        //Init for pathfinding
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //Subscribe to events
        zba.dieEvent += Disconnect;
        playerObj.GetComponent<PlayerController>().moveEvent += Reset;

        if (playerObj != null)
        {
            // Launch A* Thread
            findShortestPath();
        }
    }

    private void Reset()
    {
        gridNodeData = gridNodeDataOriginal;

        if (playerObj != null)
        {
            zba.isPathReady = false;
            Debug.Log("Astar Launching");
            // Launch A* Thread
            findShortestPath();
        }
    }

    public void findShortestPath() //maybe dont need to retrieve playerObj as param and just get it here in awake()
    {
        startIndex = zba.getNearestNodeIndex();
        endIndex = playerObj.GetComponent<PlayerController>().getNearestNodeIndex();

        Thread myNewThread = new Thread(AstarAlgo);
        myNewThread.Start();        
    }

    public void GeneratePath()
    {
        path = new List<Vector3>();

        int x = endIndex;

        // While there's still previous node, we will continue.
        while (x != startIndex & x != int.MaxValue)
        {
            path.Add(gridNodeData[x].position);
            x = gridNodeData[x].parentNodeIndex;
        }

        // Reverse the list so that it will be from start to end.
        path.Reverse();
        
        //path.Add(playerObj.transform.position); 
        zba.InitPath(path);
        pathFound = false;
    }

    private void Update()
    {
        if(pathFound)
        {
            GeneratePath();
        }
    }

    private void AstarAlgo()
    {
        // Nodes that are unexplored
        List<int> unexploredIndex = new List<int>();

        // We add all the nodes we found into unexplored.
        for (int i = 0; i < gridNodeData.Length; i++)
        {
            gridNodeData[i].weight = int.MaxValue;
            gridNodeData[i].ponderedWeight = int.MaxValue;
            gridNodeData[i].parentNodeIndex = int.MaxValue;

            if(i == startIndex)
            {
                gridNodeData[startIndex].weight = 0;
                gridNodeData[startIndex].ponderedWeight = Vector3.Distance(gridNodeData[startIndex].position, gridNodeData[endIndex].position);
            }

            if (gridNodeData[i].walkable)
            {
                unexploredIndex.Add(i);
            }
        }

        
        while (unexploredIndex.Count > 0)
        {
            //faire le sorting par poids ponderés
            unexploredIndex.Sort((x, y) => gridNodeData[x].ponderedWeight.CompareTo(gridNodeData[y].ponderedWeight));

            // Get the lowest weight in unexplored.
            int currentIndex = unexploredIndex[0];

            //A* stops when a path to end is found
            if (currentIndex == endIndex)
            {
                pathFound = true;
                return;
            }

            //Remove the node, since we are exploring it now.
            unexploredIndex.Remove(currentIndex);

            List<int> neighboursIndex = gridNodeData[currentIndex].neighbourNodeIndex;

            foreach (int x in neighboursIndex)
            {
                // We want to avoid those that had been explored and is not walkable.
                if (unexploredIndex.Contains(x) && gridNodeData[x].walkable)
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(gridNodeData[x].position, gridNodeData[currentIndex].position);
                    distance = gridNodeData[currentIndex].weight + distance;

                    // Get the distance of the destination.
                    float distance2End = Vector3.Distance(gridNodeData[x].position, gridNodeData[endIndex].position);
                    float ponderedDistance = distance + distance2End;

                    // If the added distance is less than the current weight.
                    if (distance < gridNodeData[x].weight)
                    {
                        // We update the new distance as weight and update the new path now.
                        gridNodeData[x].weight = distance;
                        gridNodeData[x].ponderedWeight = ponderedDistance;
                        gridNodeData[x].parentNodeIndex = currentIndex;
                    }
                }
            }
        }
        pathFound = true;
        return;
    }

    void Disconnect()
    {
        playerObj.GetComponent<PlayerController>().moveEvent -= Reset;
        zba.dieEvent -= Disconnect;
    }
}

public struct GridNodeData
{
    public int index; // pas besoin 
    public float weight;
    public float ponderedWeight;
    public Vector3 position;
    public int parentNodeIndex;
    public List<int> neighbourNodeIndex;
    public bool walkable;
    public bool zombie; // pas besoin
    public bool player; // pas besoin
}

