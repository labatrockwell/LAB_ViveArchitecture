using UnityEngine;
using System.Collections;

public class DrawLineManagerVR : MonoBehaviour {

    public GameObject controller;
    public GameObject drawTool;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    private GameObject go = null;
    private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;

	// Use this for initialization
	void Awake () {
        trackedObject = controller.GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {

        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Trigger pressed");
            if (!go)
            {
                Debug.Log("Creating go");
                go = new GameObject();
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.AddComponent<MeshCollider>();
                currLine = go.AddComponent<MeshLineRenderer>();
                go.AddComponent<Interaction>();
                currLine.setWidth(.01f);
                numClicks = 0;
            }
            else
            {
                Debug.Log("Editing go");
                Debug.Log(drawTool.transform.position);
                currLine.AddPoint(drawTool.transform.position);
                float dis = Vector3.Distance(prevPos, drawTool.transform.position);
                prevPos = drawTool.transform.position;
                currLine.setWidth(.2f - (dis / 10));
                numClicks++;
            }
        }
        else
        {
            go = null;
        }
    }
}
