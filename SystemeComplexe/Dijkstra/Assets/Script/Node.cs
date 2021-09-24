using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private Vector3 position = new Vector3();
    [SerializeField] private Transform parentNode = null;
    [SerializeField] private List<Transform> neighbourNode;
    [SerializeField] private bool walkable = true;

	// Use this for initialization
	void Start () {
        this.resetNode();
    }

  
    /// Reset all the values in the nodes.
    public void resetNode()
    {
        position = new Vector3();
        weight = int.MaxValue;
        parentNode = null;
    }

    // -------------------------------- Setters --------------------------------

   
    /// Set the parent node.
    public void setParentNode(Transform node)
    {
        this.parentNode = node;
    }

    ///
    public void setPosition(Vector3 posVec)
    {
        this.position = posVec;
    }

    
    /// Set the weight value
    public void setWeight(float value)
    {
        this.weight = value;
    }

    
    /// Set is node is walkable.
    public void setWalkable(bool value)
    {
        this.walkable = value;
    }

    
    /// Adding neighbour node object.
    public void addNeighbourNode(Transform node)
    {
        this.neighbourNode.Add(node);
    }

    // -------------------------------- Getters --------------------------------

    
    /// Get neighbour node.
    public List<Transform> getNeighbourNode()
    {
        List<Transform> result = this.neighbourNode;
        return result;
    }

    
    /// Get weight
    public float getWeight()
    {
        float result = this.weight;
        return result;

    }

    public Vector3 getPosition()
    {
        Vector3 posVec = this.position;
        return posVec;
    }

    
    /// Get the parent Node.
    public Transform getParentNode()
    {
        Transform result = this.parentNode;
        return result;
    }


    // -------------------------------- Checkers --------------------------------

    
    /// Get if the node is walkable.
    public bool isWalkable()
    {
        bool result = walkable;
        return result;
    }
}
