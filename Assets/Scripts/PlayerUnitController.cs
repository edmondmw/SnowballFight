using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { green, red };

public class PlayerUnitController : MonoBehaviour {
    // Should have an array of player controlled units
    public GameObject[] units;
    public GameObject snowball;
    public float throwForce;

    Camera cam;
    UnitMotor motor;
    int selectedIndex = 0;
    
	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        motor = units[selectedIndex].GetComponent<UnitMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveHandler();
	}

    void FixedUpdate()
    {
        AttackHandler();
    }

    void MoveHandler()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                motor.Move(hit.point);
            }
        }
    }

    void AttackHandler()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            // Gets the throw position 
            Vector3 snowballPosition = units[selectedIndex].transform.GetChild(1).transform.position;

            GameObject aSnowball = Instantiate(snowball, snowballPosition, units[selectedIndex].transform.rotation);
            aSnowball.GetComponent<Rigidbody>().AddForce(units[selectedIndex].transform.forward * throwForce);
            aSnowball.layer = gameObject.layer;
        }
    }
}
