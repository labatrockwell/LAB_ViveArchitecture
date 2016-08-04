using UnityEngine;
using System.Collections;

public class DrawLineManager : MonoBehaviour {

	public GameObject mObj;
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
					GameObject go = new GameObject ();
					go.AddComponent<MeshFilter> ();
					go.AddComponent<MeshRenderer> ();
					go.AddComponent<MeshCollider> ();
					currLine = go.AddComponent<MeshLineRenderer> ();
					go.AddComponent<Interaction> ();
					currLine.setWidth (.1f);
					numClicks = 0;
				} else if (mObj.GetComponent<DrawTool> ().clickMove) {
					currLine.AddPoint (mObj.transform.position);
					float dis = Vector3.Distance (prevPos, mObj.transform.position);
					prevPos = mObj.transform.position;
					currLine.setWidth (.2f - (dis / 10));
					numClicks++;
				}
			}
		}
			
	}
}
