using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : MonoBehaviour {
    // Should have an array of player controlled units
    public GameObject[] units;
    public GameObject snowball;

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
                Debug.Log(hit.point);
                motor.Move(hit.point);
            }
        }
    }

    void AttackHandler()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject aSnowball = Instantiate(snowball, units[selectedIndex].transform.position, units[selectedIndex].transform.rotation);
            aSnowball.GetComponent<Rigidbody>().AddForce(units[selectedIndex].transform.forward * 1000f);
        }
    }
}
