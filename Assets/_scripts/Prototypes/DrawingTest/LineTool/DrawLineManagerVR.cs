using UnityEngine;
using System.Collections;

public class DrawLineManagerVR : MonoBehaviour {

    public GameObject labTrackedController;
    public GameObject drawTool;

    private GameObject go = null;
    private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (labTrackedController.GetComponent<SteamVR_TrackedController> ().triggerPressed) {
			if (!go) {
				go = new GameObject ();
				go.AddComponent<MeshFilter> ();
				go.AddComponent<MeshRenderer> ();
				go.AddComponent<MeshCollider> ();
				currLine = go.AddComponent<MeshLineRenderer> ();
				go.AddComponent<Interaction> ();
				currLine.setWidth (.1f);
				numClicks = 0;
			} else {
				currLine.AddPoint (drawTool.transform.position);
				float dis = Vector3.Distance (prevPos, drawTool.transform.position);
				prevPos = drawTool.transform.position;
				currLine.setWidth (.2f - (dis / 10));
				numClicks++;
			}
        }
        else
        {
            go = null;
        }
	}
}
