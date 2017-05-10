using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanCamera : MonoBehaviour {

    private GameObject mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(0, mainCamera.transform.position.y + 4.5f,0);
	}
}
