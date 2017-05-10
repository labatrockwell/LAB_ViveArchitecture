using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanViewDirectionIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0.0f;
        rotation.z = 0.0f;
        transform.eulerAngles = rotation;
	}
}
