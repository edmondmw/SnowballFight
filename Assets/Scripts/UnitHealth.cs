using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitHealth : MonoBehaviour {
    int maxHealth = 3;
    public int currentHealth = 3;
    public GameObject frontHealth;
    public GameObject backHealth;

    private void Update()
    {
        // Lay down if dead
        if(!isAlive())
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
            Destroy(collision.gameObject);

            if (currentHealth > 0)
            {
                currentHealth--;
                float healthProportion = (float)currentHealth / maxHealth;
                Vector3 updatedHealthSize = frontHealth.transform.localScale;
                Vector3 updatedHealthPosition = frontHealth.transform.localPosition;
                Vector3 updatedBackHealthPosition = backHealth.transform.localPosition;
                updatedHealthSize.x = healthProportion;
                updatedHealthPosition.x = updatedHealthSize.x / 2 - 0.5f;
                updatedBackHealthPosition.x = 0.5f - updatedHealthSize.x / 2;
                frontHealth.transform.localScale = updatedHealthSize;
                frontHealth.transform.localPosition = updatedHealthPosition;
                backHealth.transform.localScale = updatedHealthSize;
                backHealth.transform.localPosition = updatedBackHealthPosition;
            }
        }   
    }

    public bool isAlive()
    {
        return currentHealth > 0;
    }
}
