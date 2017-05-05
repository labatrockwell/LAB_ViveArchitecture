using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	public bool isClicked = false;
	public bool clickDown = false;
	public bool clickMove= false;
	public bool clickUp = false;
	public bool isSelected = false;
	public Renderer material;
	// Use this for initialization
	void Start () {
//		parent s= transform.parent.gameObject;
	}

	void OnMouseDown(){
		isClicked = true;
	}

	// Update is called once per frame
	void Update ()
	{
//		GameObject.Find("DrawTool").
		Ray ray = new Ray (GameObject.Find ("DrawTool").transform.position, Camera.main.transform.forward);
		RaycastHit hit; 
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if (Physics.Raycast (ray, out hit)) {
			//Debug.Log (hit.transform.gameObject);
			if (hit.transform.gameObject == gameObject) {
				isClicked = true;			
			}
		}
//		if(gameObject.GetComponent<Collider>().bounds.Contains(GameObject.Find("DrawTool").transform.position))
//		{
//				isClicked = true;
//		}



		if (!Input.GetMouseButton (0)) 
			isClicked = false;
		
		if (isClicked) {
			if (!clickDown && !clickMove) {
				clickDown = true;
				isSelected =! isSelected;	
//				Destroy (gameObject);
			} else if (clickDown || clickMove) {
				clickMove = true;
				clickDown = false;
			} else {


			}
		} else {
			if (clickMove) {
				clickMove = false;
				clickUp = true;
				//isSelected = false;
			} else if (clickUp) {
				clickUp = false;
			}
		}
			

//		var thisMat = gameObject.GetComponent<Material> ();

//		Debug.Log (parent);
		material = gameObject.GetComponent<Renderer>();

		//if (isSelected) {
		//	material.material.color = new Color (1f, 0, 0);

		//} else {
		//	material.material.color = new Color (0, 1f, 0);

		//}

	}
}
