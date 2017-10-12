using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour {

    public bool isMoving = false;
    // 1 for green and -1 for red
    public int direction = 1;

    NavMeshAgent nav;
    Vector3 destination;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
        // When we reach destination, face forward
        if (nav.velocity.magnitude == 0 && GetComponent<UnitHealth>().active)
        {
            transform.eulerAngles = new Vector3(0, 360 * direction, 0);
            isMoving = false;
        }
    }

    public void Move(Vector3 point)
    {
        isMoving = true;
        nav.SetDestination(point);
        destination = point;
    }

    public void Attack()
    {
       // if(isMoving)
            //stop moving
        
        //generate snowball
    }
}
