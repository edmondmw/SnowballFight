using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitActions : MonoBehaviour {
    // 1 for green and -1 for red
    public int direction = 1;
    public GameObject snowball;
    public float throwForce;

    NavMeshAgent nav;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
      
    }

    public void Move(Vector3 point)
    {
        nav.SetDestination(point);
    }

    public void Attack(Vector3 direction)
    {
        // If moving then stop
        if (nav.velocity.magnitude != 0f) 
        {
            nav.enabled = false;
            //StopAndFaceForward();
        }

        // Rotate the unit to face the direction.
        Vector3 lookDirection = (direction - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;

        // Gets the throw position 
        Vector3 snowballPosition = transform.Find("ThrowPoint").transform.position;
        // Throw a snowball
        GameObject aSnowball = Instantiate(snowball, snowballPosition, transform.rotation);
        aSnowball.layer = gameObject.layer;
        aSnowball.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);    
        nav.enabled = true;
    }

    void StopAndFaceForward()
    {
        transform.eulerAngles = new Vector3(0, 360 * direction, 0);
    }

}
