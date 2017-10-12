using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitHealth : MonoBehaviour {

    int health = 3;
    public bool active = true;

    NavMeshAgent nav;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        if(!active)
        {
            Vector3 temp = transform.rotation.eulerAngles;
            temp.x = 90.0f;
            transform.rotation = Quaternion.Euler(temp);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(transform.rotation);
        if (collision.gameObject.CompareTag("Snowball"))
            health--;

        if (health <= 0)
        {
            active = false;
        }
    }
}
