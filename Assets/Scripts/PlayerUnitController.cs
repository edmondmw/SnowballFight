using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { green, red };

public class PlayerUnitController : MonoBehaviour {
    // Should have an array of player controlled units
    public List<GameObject> units = new List<GameObject> ();
    public GameObject snowball;
    public Material originalMaterial;
    public Material outlinedMaterial;
    public float fireRate;

    LineRenderer aimLine;
    Camera cam;
    UnitActions actions;
    Transform currentThrowPoint;
    int selectedIndex = 0;
    bool attackMode = false;
    float nextFire;


    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        actions = units[selectedIndex].GetComponent<UnitActions>();
        units[selectedIndex].transform.Find("Body").GetComponent<Renderer>().material = outlinedMaterial;
        // Get the selected unit's throw point's line renderer
        currentThrowPoint = units[selectedIndex].transform.Find("ThrowPoint");
        aimLine = units[selectedIndex].GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        AttackHandler();
        MoveHandler();
        SelectionHandler();
	}


    void MoveHandler()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                actions.Move(hit.point);
            }
        }
    }

    void AttackHandler()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            attackMode = !attackMode;
        }

        if (attackMode)
        {
            // In Unity, mouse position is measured in x and y, with a z of 0. Since this is an isometric game, in order to get the x and z coordinates
            // of the mouse cursor, you need to use a raycast from the cursor to the ground. The collision point is the (x,z) coordinate of your mouse.
            aimLine.SetPosition(0, currentThrowPoint.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                aimLine.SetPosition(1, hit.point);
                aimLine.enabled = true;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                actions.Attack(hit.point);
                attackMode = false;
            }
        }
        else
        {
            aimLine.enabled = false;
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
                // outline the new unit, transfer action control, update aim line, and update the selectedIndex
                if (newIndex >= 0 && newIndex != selectedIndex)
                {
                    units[selectedIndex].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                    units[newIndex].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;
                    actions = units[newIndex].GetComponent<UnitActions>();
                    currentThrowPoint = units[newIndex].transform.Find("ThrowPoint");
                    aimLine.enabled = false;
                    aimLine = units[newIndex].GetComponent<LineRenderer>();
                    selectedIndex = newIndex;
                }
            }
        }
    }
}
