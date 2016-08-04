using UnityEngine;
using System.Collections;

public class PlaneManager : MonoBehaviour {
	public GameObject mObj;
	private Vector3 prevPos = new Vector3(0,0,0);
	GameObject cube;

	public bool drawingOn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (drawingOn) {
			if (mObj.GetComponent<DrawTool> ().isSelected) {
				if (mObj.GetComponent<DrawTool> ().clickDown) {
					cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.GetComponent<Renderer>().material = new Material (Shader.Find("Standard"));
					cube.AddComponent<Interaction> ();

					cube.transform.position = mObj.transform.position;
					cube.transform.localScale.Set (.1f, .1f, .1f);
					prevPos = mObj.transform.position;
//					cube.transform.
//						.SetNormalAndPosition (Camera.main.transform.right,mObj.transform.position);
				} else if (mObj.GetComponent<DrawTool> ().clickMove) {
					Vector3 posDif = mObj.transform.position - prevPos;
					cube.transform.localScale += posDif;
					prevPos = mObj.transform.position;
				}
			}
		}
	}
		
//	Vector3 getCornerOne(Vector3 v1, Vector3 v2){
//		
//	}
//
//	Vector3 getCornerTwo(Vector3 v1, Vector3 v2){
//		
//	}
}