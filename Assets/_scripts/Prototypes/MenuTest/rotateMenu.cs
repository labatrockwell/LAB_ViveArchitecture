using UnityEngine;
using System.Collections;

public class rotateMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (transform.position, Vector3.up, Mathf.Sin (Time.time/3) * .125f);
//		transform.rotate
	}
}
