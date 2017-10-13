using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    Transform health;
    Transform damage;

	// Use this for initialization
	void Start () {
        health = transform.GetChild(0);
        damage = transform.GetChild(1);	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("health: " + health.position.z + " damage: " + damage.position.z);
		if(health.position.z >= damage.position.z)
        {
            Vector3 temp = damage.position;
            damage.position = health.position;
            health.position = temp;
        }
	}
}
