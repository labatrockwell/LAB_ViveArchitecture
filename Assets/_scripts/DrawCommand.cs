using UnityEngine;
using System.Collections;

public class DrawCommand : Command {

    public GameObject controller;
    public GameObject drawTool;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    private GameObject go = null;
    private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;

    //public bool commandActive;
    public bool interrupted;
    private float prevTouchpad;
    private float initialTouch;
    private float extensionFactor;
    private float prevExtensionFactor;
    private Vector3 originalPosition;
    private Vector3 originalWorldPosition;

    // Use this for initialization
    void Awake () {
        trackedObject = controller.GetComponent<SteamVR_TrackedObject>();
        commandActive = false;
        extensionFactor = 0.0f;
        originalPosition = drawTool.transform.localPosition;
        interrupted = false;
	}

	// Update is called once per frame
	void Update () {

        if (commandActive) {
            if (!paused) {
                //Debug.Log("DrawCommandActive");
                Vector3 extension;
                if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                {

                    Debug.Log("Trigger pressed");
                    if (!go)
                    {
                        //Debug.Log("Creating go");
                        go = new GameObject();
                        go.AddComponent<MeshFilter>();
                        go.AddComponent<MeshRenderer>();
                        go.AddComponent<MeshCollider>();
                        currLine = go.AddComponent<MeshLineRenderer>();
                        go.AddComponent<Interaction>();
                        float width = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
                        //Debug.Log("Width X: " + width);
                        width = width.Remap(0.15f, 1.0f, 0.001f, 0.025f);
                        currLine.setWidth(width);
                        numClicks = 0;
                    }
                    else
                    {
                        //Debug.Log("Editing go");
                        Debug.Log(drawTool.transform.position);

                        

                        //record the value of the first touch
                        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                            Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                            initialTouch = touchPad.y;
                        }

                        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                        {
                            Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                            //find the difference between the current touch position and the original touch position
                            extensionFactor = (touchPad.y-initialTouch)/10;
                            //extensionFactor = (touchPad.y - prevTouchpad)/5;
                            extension = drawTool.transform.position + (drawTool.transform.forward * extensionFactor);
                            drawTool.transform.position = extension;
                        }

                        currLine.AddPoint(drawTool.transform.position);

                        float dis = Vector3.Distance(prevPos, drawTool.transform.position);
                        prevPos = drawTool.transform.position * extensionFactor;                        

                        //pressure of the trigger pull
                        float width = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
                        //Debug.Log("Width X: " + width);
                        width = width.Remap(0.6f, 1.0f, 0.001f, 0.1f);
                        currLine.setWidth(width / 2);                    
                        numClicks++;

                    }

                }
                else
                {
                    if (go)
                    {
                        go = null;
                        extensionFactor = 0.0f;
                        drawTool.transform.localPosition = originalPosition;
                    }
                }
            }
        }
    }
}
