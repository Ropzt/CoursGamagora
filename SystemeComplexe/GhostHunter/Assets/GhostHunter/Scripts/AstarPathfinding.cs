using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathfinding : MonoBehaviour
{


    public List<GameObject> findShortestPath(GameObject start, GameObject end)
    {
        List<GameObject> result = new List<GameObject>();
        GameObject node = DijkstrasAlgo(start, end);

        // While there's still previous node, we will continue.
        while (node != null)
        {
            result.Add(node);
            Node currentNode = node.GetComponent<Node>();
            node = currentNode.getParentNode();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    private GameObject DijkstrasAlgo(GameObject start, GameObject end)
    {
        NodeGenerator grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();

        // Nodes that are unexplored
        List<GameObject> unexplored = new List<GameObject>();

        // We add all the nodes we found into unexplored.
        foreach (GameObject obj in grid.nodeList)
        {
            Node n = obj.GetComponent<Node>();
            n.resetNode();
            if (n.isWalkable())
            {
                unexplored.Add(obj);
            }
        }

        //Reset end node, set start node's weight to 0 and add both to unexplored
        Node endNode = end.GetComponent<Node>();
        endNode.resetNode();
        Node startNode = start.GetComponent<Node>();
        startNode.setWeight(0);
        unexplored.Add(start);
        unexplored.Add(end);

        while (unexplored.Count > 0)
        {
            // Sort the unexplored by their weight 
            //unexplored.Sort((x, y) => x.GetComponent<Node>().getWeight().CompareTo(y.GetComponent<Node>().getWeight()));
            //faire le sorting par poids ponderés
            unexplored.Sort((x, y) => x.GetComponent<Node>().getPonderedWeight().CompareTo(y.GetComponent<Node>().getPonderedWeight()));

            // Get the lowest weight in unexplored.
            GameObject current = unexplored[0];

            
            //A* stops when a path to end is found
            if (current == end)
            {
                return end;
            }
            
            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            Node currentNode = current.GetComponent<Node>();
            List<GameObject> neighbours = currentNode.getNeighbourNode();
            foreach (GameObject neighNode in neighbours)
            {
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.isWalkable())
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(node.getPosition(), currentNode.getPosition());
                    distance = currentNode.getWeight() + distance;

                    // Get the distance of the destination.
                    float distance2End = Vector3.Distance(node.getPosition(),endNode.getPosition());
                    float ponderedDistance = distance+distance2End;

                    // If the added distance is less than the current weight.
                    if (distance < node.getWeight())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.setWeight(distance);
                        node.setPonderedWeight(ponderedDistance);
                        node.setParentNode(current);
                    }
                }
            }
        }
        return end;
    }
}
