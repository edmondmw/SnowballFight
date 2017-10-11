using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour {

    NavMeshAgent nav;
    Vector3 destination;
    bool isMoving = false;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
        // When we reach destination, face forward
        // TODO: Find a better way to determine when finished moving, This won't work for out of bounds clicks
        if (transform.position.x == destination.x && transform.position.z == destination.z)
            transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    public void Move(Vector3 point)
    {
        isMoving = true;
        nav.SetDestination(point);
        destination = point;
    }
}
