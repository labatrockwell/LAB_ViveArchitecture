using UnityEngine;
using System.Collections;

public class DrawLineManager : MonoBehaviour {

	public GameObject mObj;
	//public GameObject mLinePrefab ;
	private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;
	public bool drawingOn;




	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (drawingOn) {
			if (mObj.GetComponent<DrawTool> ().isSelected) {

				if (mObj.GetComponent<DrawTool> ().clickDown) {

					//GameObject line  = Instantiate(mLinePrefab);
//					ObjectIdentifier id = line.AddComponent<ObjectIdentifier> ();
//					id.prefabName = "lineobject";
					Debug.Log("New Object");
					GameObject go = new GameObject ();
					go.AddComponent<MeshFilter> ();
					go.AddComponent<MeshRenderer> ();
					go.AddComponent<MeshCollider> ();
					currLine = go.AddComponent<MeshLineRenderer> ();
					go.AddComponent<Interaction> ();
					currLine.setWidth (.1f);
					//currLine = mObj.GetComponent<MeshLineRenderer> ();
					//currLine.setWidth (.1f);
				} else if (mObj.GetComponent<DrawTool> ().clickMove) {
					Debug.Log("Object Move");

					currLine.AddPoint (mObj.transform.position);
					float dis = Vector3.Distance (prevPos, mObj.transform.position);	
					float width = .15f -  dis;
					width = Mathf.Clamp(width, .1f, 1.0f);
					prevPos = mObj.transform.position;
					currLine.setWidth (width);
				}
			}
		}
			
	}
}
