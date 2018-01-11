using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {
    float destroyTime = 4f;
    float currentTime;
    float endTime;

	void Awake ()
    {
        currentTime = Time.time;
        endTime = currentTime + destroyTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time >= endTime)
        {
            Destroy(gameObject);
        }
	}
}
