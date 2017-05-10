using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanHUD : MonoBehaviour {

    private GameObject mainCamera;
    public float distance;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //transform.position = mainCamera.transform.position + (mainCamera.transform.forward * distance);
        transform.LookAt(mainCamera.transform.position);
        transform.Rotate(new Vector3(0, 180, 0));

	}
}
