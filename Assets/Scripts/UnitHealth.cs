using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitHealth : MonoBehaviour {

    int health = 3;
    public bool active = true;
    public GameObject healthbar;

    private void Start()
    {
    }


    private void Update()
    {
        // Lay down if dead
        if(!active)
        {
            Vector3 temp = transform.rotation.eulerAngles;
            temp.x = 90.0f;
            transform.rotation = Quaternion.Euler(temp);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Snowball"))
        {
            health--;
            Vector3 updatedHealthSize = healthbar.transform.localScale;
            Vector3 updatedHealthPosition = healthbar.transform.position;
            updatedHealthSize.x -= 0.33f;
            updatedHealthPosition.x = updatedHealthSize.x/2 - 0.5f;
            healthbar.transform.localScale = updatedHealthSize;
            healthbar.transform.position = updatedHealthPosition;
        }

        if (health <= 0)
        {
            active = false;
        }
    }
}
