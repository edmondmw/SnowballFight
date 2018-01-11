using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { green, red };

// Allows the player to interact with/control the units
public class PlayerUnitController : MonoBehaviour {
    // Should have an array of player controlled units
    public List<GameObject> units = new List<GameObject> ();
    public GameObject snowball;
    public Material originalMaterial;
    public Material outlinedMaterial;
    public float fireRate = 0.25f;

    List<int> selectedIndices = new List<int>();
    //LineRenderer aimLine;
    Camera cam;
    //UnitActions actions;
    //UnitHealth health;
    //int selectedIndex = 0;
    bool attackMode = false;
    bool isSelecting = false;
    bool somethingSelected = false;
    float nextFire;
    Vector3 startMousePosition;

    private void Awake()
    {
        cam = Camera.main;
        // Make default selected unit the one at the 0th index
        selectedIndices.Add(0);
        units[0].transform.Find("Body").GetComponent<Renderer>().material = outlinedMaterial;
    }

	// Update is called once per frame
	void Update ()
    {
        DeselectDead();
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
                foreach(int index in selectedIndices)
                {
                    units[index].GetComponent<UnitActions>().Move(hit.point);
                }
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
            foreach (int index in selectedIndices)
            {
                LineRenderer aimLine = units[index].GetComponent<LineRenderer>();
                aimLine.SetPosition(0, units[index].transform.position);
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground")))
                {
                    aimLine.SetPosition(1, hit.point);
                    aimLine.enabled = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    units[index].GetComponent<UnitActions>().Attack(hit.point);
                    attackMode = false;
                    nextFire = Time.time + fireRate;
                }
            }
        }
        else
        {
            foreach (int index in selectedIndices)
            {
                units[index].GetComponent<LineRenderer>().enabled = false;
            }
        }
    }

    // When we select a unit, we want to gain control over it and outline it
    void SelectionHandler()
    {
     
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startMousePosition = Input.mousePosition;

            // For single click selection
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int newIndex = units.IndexOf(hit.transform.gameObject);
                // Update member variables to the newly selected unit
                if (newIndex >= 0  && units[newIndex].GetComponent<UnitHealth>().isAlive())
                {                    
                    foreach(int index in selectedIndices)
                    {
                        if (index != newIndex)
                        {
                            units[index].GetComponent<LineRenderer>().enabled = false;
                            units[index].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                        }
                    }
                    // TODO: might need to change this
                    // When switching units should be able to fire immediately 
                    nextFire = Time.time;
                    attackMode = false;
                    units[newIndex].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;
                    selectedIndices.Clear();
                    selectedIndices.Add(newIndex);
                }
            }
        }

        // For drag selection
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            somethingSelected = false;
        }

        if(isSelecting)
        {
            //Iterate through the units and check if they're in the bounds. If not, make sure they 
            for( int i = 0; i < units.Count; ++i)
            {
                if(IsWithinSelectionBounds(units[i]) && units[i].GetComponent<UnitHealth>().isAlive())
                {
                    if (!selectedIndices.Contains(i))
                    {
                        // Highlight the units

                        units[i].transform.GetChild(0).GetComponent<Renderer>().material = outlinedMaterial;
                        selectedIndices.Add(i);
                    }

                    if (!somethingSelected)
                    {
                        somethingSelected = true;
                        i = -1;
                    }
                }
                // Only want to deselect other units when actually selecting other units
                else if(somethingSelected)
                {
                    units[i].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                    selectedIndices.Remove(i);
                }
            }
        }
    }

    // If any units have been killed deselect them
    private void DeselectDead()
    {
        foreach(int index in selectedIndices)
        {
            if(!units[index].GetComponent<UnitHealth>().isAlive())
            {
                units[index].transform.GetChild(0).GetComponent<Renderer>().material = originalMaterial;
                units[index].GetComponent<LineRenderer>().enabled = false;
                selectedIndices.Remove(index);
                
            }
        }
    }

    // For handling GUI events. May be called several times a frame
    private void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Rectangle.GetScreenRect(startMousePosition, Input.mousePosition);
            Rectangle.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Rectangle.DrawScreenRectBorder(rect, 2, Color.green);
        }
    }

    // Returns true if the unit is within the selection box
    private bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
        {
            return false;
        }

        var camera = Camera.main;
        var viewportBounds = Rectangle.GetViewportBounds(camera, startMousePosition, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }
}
