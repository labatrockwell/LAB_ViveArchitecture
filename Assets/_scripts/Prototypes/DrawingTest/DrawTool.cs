using UnityEngine;
using System.Collections;

public class DrawTool : MonoBehaviour {

	public bool isClicked = false;
	public bool clickDown = false;
	public bool clickMove= false;
	public bool clickUp = false;
	public bool isSelected = true;
	Vector3 prevPos = new Vector3 (0,0,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			//RaycastHit hit; 
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
//			Ray ray = new Ray();
		
//			if (Physics.Raycast (Camera.main.transform.forward, out hit)) {
//				Debug.Log (hit.transform.gameObject);
//				if (hit.transform.gameObject == gameObject) {
					isSelected = true;
//				}
//			}
			isClicked = true;
		} else {
			isClicked = false;
		}



//		if (isSelected) {
		    gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
			prevPos = Camera.main.transform.forward;
				
//			prevMousePos = mousePos;
//		}
			if (isClicked) {
				if (!clickDown && !clickMove) {
					clickDown = true;
				Debug.Log ("CLICKDONW");
				} else if (clickDown || clickMove) {
					clickMove = true;
					clickDown = false;
				} else {
					
				
				}
			} else {
				if (clickMove) {
					clickMove = false;
					clickUp = true;
//					isSelected = false;
				} else if (clickUp) {
					clickUp = false;
				}
			}
	}
		

}
