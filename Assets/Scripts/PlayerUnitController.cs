using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { green, red };

public class PlayerUnitController : MonoBehaviour {
    // Should have an array of player controlled units
    public List<GameObject> units = new List<GameObject> ();
    public GameObject snowball;
    public float throwForce;
    public Material originalMaterial;
    public Material outlinedMaterial;

    Camera cam;
    UnitMotor motor;
    int selectedIndex = 0;
    
	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        motor = units[selectedIndex].GetComponent<UnitMotor>();
        units[selectedIndex].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;

    }
	
	// Update is called once per frame
	void Update () {
        MoveHandler();
        SelectionHandler();
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

    // When we select a unit, we want to gain control over it and outline it
    void SelectionHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int newIndex = units.IndexOf(hit.transform.gameObject);
                // If the object we clicked is one of our units, then remove outline from previously selected unit, 
                // outline the new unit, transfer motor control, and updated the selectedIndex
                if (newIndex >= 0)
                {
                    units[selectedIndex].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                    units[newIndex].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;
                    motor = units[newIndex].GetComponent<UnitMotor>();
                    selectedIndex = newIndex;
                }
            }
        }
    }
}
