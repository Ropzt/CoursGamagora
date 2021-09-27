using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private Vector3 position = new Vector3();
    [SerializeField] private GameObject parentNode = null;
    [SerializeField] private List<GameObject> neighbourNode;
    [SerializeField] private bool walkable = true;
    [SerializeField] private bool zombie = false;

    // Use this for initialization
    void Start () {
        this.resetNode();
    }

  
    /// Reset all the values in the nodes.
    public void resetNode()
    {
        weight = int.MaxValue;
        parentNode = null;
    }

    // -------------------------------- Setters --------------------------------

   
    /// Set the parent node.
    public void setParentNode(GameObject node)
    {
        this.parentNode = node;
    }

    /// Set the position of gameobject.
    public void setPosition(Vector3 posVec)
    {
        this.position = posVec;
    }

    
    /// Set the weight value.
    public void setWeight(float value)
    {
        this.weight = value;
    }

    
    /// Set is node is walkable.
    public void setWalkable(bool value)
    {
        this.walkable = value;
    }

    /// Set is node is a zombie.
    public void setZombie(bool value)
    {
        this.zombie = value;
    }


    /// Adding neighbour node object.
    public void addNeighbourNode(GameObject node)
    {
        this.neighbourNode.Add(node);
    }

    // Getters

    
    /// Get neighbour node.
    public List<GameObject> getNeighbourNode()
    {
        List<GameObject> result = this.neighbourNode;
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
    public GameObject getParentNode()
    {
        GameObject result = this.parentNode;
        return result;
    }


    // -------------------------------- Checkers --------------------------------

    
    /// Get if the node is walkable.
    public bool isWalkable()
    {
        bool result = walkable;
        return result;
    }

    /// Get if the node is a zombie.
    public bool isZombie()
    {
        bool result = zombie;
        return result;
    }

    // -------------------------------- Removers --------------------------------

    /// Remove neighbour node object.
    public void removeNeighbourNode(GameObject node)
    {
        this.neighbourNode.Remove(node);
    }

}
