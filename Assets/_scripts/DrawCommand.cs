using UnityEngine;
using System.Collections;

public class DrawCommand : MonoBehaviour {

    public GameObject controller;
    public GameObject drawTool;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    private GameObject go = null;
    private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;

    public bool commandActive;

    private float prevTouchpad;
    private float extensionFactor;
    private float prevExtensionFactor;
    private Vector3 originalPosition;


	// Use this for initialization
	void Awake () {
        trackedObject = controller.GetComponent<SteamVR_TrackedObject>();
        commandActive = false;
        extensionFactor = 0.0f;
        originalPosition = drawTool.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {

        if (commandActive) {
            //Debug.Log("DrawCommandActive");

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
                    float width = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
                    //Debug.Log("Width X: " + width);
                    width = width.Remap(0.15f, 1.0f, 0.001f, 0.05f);
                    currLine.setWidth(width);
                    numClicks = 0;
                }
                else
                {
                    Debug.Log("Editing go");
                    //Debug.Log(drawTool.transform.position);                    

                    if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        //Debug.Log("XXXXXXXXXXXXXXXXXXXX TouchPad Happening");
                        Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                        //get the difference between the current position and the previous position
                        extensionFactor = touchPad.y - prevTouchpad;
                        prevTouchpad = touchPad.y;
                        //Debug.Log("XXXXXXXXXXXXXXXXXXXX Extension Factor: " + extensionFactor);
                    }
                    else {
                        extensionFactor = prevExtensionFactor;
                    }

                    Vector3 extension = drawTool.transform.forward * extensionFactor;
                    currLine.AddPoint(drawTool.transform.position + extension);
                    float dis = Vector3.Distance(prevPos, drawTool.transform.position);
                    prevPos = drawTool.transform.position * extensionFactor;

                    drawTool.transform.position += extension;

                    float width = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
                    //Debug.Log("Width X: " + width);
                    width = width.Remap(0.15f, 1.0f, 0.001f, 0.05f);
                    currLine.setWidth(width / 2);                    
                    numClicks++;

                    prevExtensionFactor = extensionFactor;
                }
            }
            else
            {
                if (go) {
                    go = null;
                    commandActive = false;
                    extensionFactor = 0.0f;
                    drawTool.transform.localPosition = originalPosition;
                }
            }

        }
        

    }
}
