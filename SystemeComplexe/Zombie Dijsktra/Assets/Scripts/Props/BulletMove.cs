using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private NodeGenerator  grid;
    [SerializeField]  private float speed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        //If going of grid, destroy
        if(transform.position.x > grid.xMax | transform.position.x < -(grid.xMax) | transform.position.z > grid.zMax | transform.position.z < -(grid.zMax) | transform.position.y > 10f | transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            Destroy(gameObject);
        }
    }
}
