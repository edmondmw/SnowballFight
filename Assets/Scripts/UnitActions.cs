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
    UnitHealth unitHealth;
	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
        unitHealth = GetComponent<UnitHealth>();
	}

    void Update()
    {
      /*  // When we reach destination, face forward
        // Check if we've reached the destination
        if (unitHealth.active &&
            !nav.pathPending &&
            nav.remainingDistance <= nav.stoppingDistance &&
            (!nav.hasPath || nav.velocity.sqrMagnitude == 0f))
        {
            StopAndFaceForward();
        } */
    }

    public void Move(Vector3 point)
    {
        nav.SetDestination(point);
    }

    public void Attack()
    {
        // If moving then stop
        if (nav.velocity.magnitude != 0f) 
        {
            nav.enabled = false;
            //StopAndFaceForward();
        }

        // Gets the throw position 
        Vector3 snowballPosition = transform.GetChild(1).transform.position;
        // Throw a snowball
        GameObject aSnowball = Instantiate(snowball, snowballPosition, transform.rotation);
        aSnowball.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        aSnowball.layer = gameObject.layer;
        nav.enabled = true;
    }

    void StopAndFaceForward()
    {
        transform.eulerAngles = new Vector3(0, 360 * direction, 0);
    }

}
