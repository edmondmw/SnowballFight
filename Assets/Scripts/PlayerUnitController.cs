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
    public float fireRate = 0.25f;

    LineRenderer aimLine;
    Camera cam;
    UnitActions actions;
    UnitHealth health;
    int selectedIndex = 0;
    bool attackMode = false;
    float nextFire;

    private void Awake()
    {
        cam = Camera.main;
        actions = units[selectedIndex].GetComponent<UnitActions>();
        health = units[selectedIndex].GetComponent<UnitHealth>();
        // Get the selected unit's throw point's line renderer
        aimLine = units[selectedIndex].GetComponent<LineRenderer>();

        units[selectedIndex].transform.Find("Body").GetComponent<Renderer>().material = outlinedMaterial;
    }

    // Use this for initialization
    void Start()
    {
    }
        

	// Update is called once per frame
	void Update ()
    {
        if (health.active)
        {
            AttackHandler();
            MoveHandler();
        }
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
        if(Input.GetKeyDown(KeyCode.A) && Time.time >= nextFire)
        {
            attackMode = !attackMode;
        }

        if (attackMode)
        {
            // In Unity, mouse position is measured in x and y, with a z of 0. Since this is an isometric game, in order to get the x and z coordinates
            // of the mouse cursor, you need to use a raycast from the cursor to the ground. The collision point is the (x,z) coordinate of your mouse.
            aimLine.SetPosition(0, units[selectedIndex].transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground")))
            {
                aimLine.SetPosition(1, hit.point);
                aimLine.enabled = true;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                actions.Attack(hit.point);
                attackMode = false;
                nextFire = Time.time + fireRate;
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
               
                // Update member variables to the newly selected unit
                if (newIndex >= 0 && newIndex != selectedIndex && units[newIndex].GetComponent<UnitHealth>().active)
                {
                    // Change outlined unit
                    attackMode = false;
                    units[selectedIndex].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                    units[newIndex].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;
                    actions = units[newIndex].GetComponent<UnitActions>();
                    health = units[newIndex].GetComponent<UnitHealth>();
                    aimLine.enabled = false;
                    aimLine = units[newIndex].GetComponent<LineRenderer>();
                    // When switching units should be able to fire immediately
                    nextFire = Time.time;
                    selectedIndex = newIndex;
                }
            }
        }
    }
}
