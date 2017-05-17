using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolylineCommand : Command {

    public GameObject controller;
    public GameObject drawTool;
    public bool interrupted;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    private GameObject mGo;
    private LineRenderer mLineRend;
    public Material mLineMaterial;

    private Vector3 prevPos = new Vector3(0, 0, 0);
    private int numClicks = 0;
    private float prevTouchpad;
    private float initialTouch;
    private float extensionFactor;
    private float prevExtensionFactor;
    private Vector3 originalPosition;
    private Vector3 originalWorldPosition;

    // Use this for initialization
    void Awake()
    {
        trackedObject = controller.GetComponent<SteamVR_TrackedObject>();
        commandActive = false;
        extensionFactor = 0.0f;
        originalPosition = drawTool.transform.localPosition;
        interrupted = false;
    }

    // Use this for initialization
    void Start () {
        mGo = null;
	}

    public new void StartCommand() {
        commandActive = true;
        drawTool.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (commandActive)
        {
            if (!paused)
            {
                //show the draw tool cursor
                Vector3 extension;
                int pointCounter = 0;

                // CONTROL EXTENSION OF THE DRAW TOOL
                //record the value of the first touch
                if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                    initialTouch = touchPad.y;
                }

                if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    Vector2 touchPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                    if (Mathf.Abs(touchPad.y - prevTouchpad) > .01f)
                    {
                        //find the difference between the current touch position and the original touch position
                        extensionFactor = (touchPad.y - initialTouch) / 10;
                        //extensionFactor = (touchPad.y - prevTouchpad)/5;
                        extension = drawTool.transform.position + (drawTool.transform.forward * extensionFactor);
                        drawTool.transform.position = extension;
                        prevTouchpad = touchPad.y;
                    }
                }

                // X 1. when the trigger is first pulled, create a new gameObject 
                // 2. parent that gameObject to a parent gmOb 
                // 3. assign material, width to a lineRenderer of the gmObj
                // 4. set the first point of the line renderer
                // 4. each subsequent press adds a new point to the lineRenderer
                // 5. if the command is inactive, set gmOb to null

                if (pointCounter > 0)
                {
                    mLineRend.SetPosition(pointCounter+1, drawTool.transform.position);
                }

                // TRIGGER INTERACTION
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    if (!mGo)
                    {
                        //Debug.Log("Creating go");
                        mGo = new GameObject();
                        mLineRend = mGo.AddComponent<LineRenderer>();
                        mLineRend.material = mLineMaterial;
                        mLineRend.startWidth = 0.01f;
                        mLineRend.endWidth = 0.01f;
                        //go.AddComponent<Interaction>();
                        mLineRend.SetPosition(0, drawTool.transform.position);
                        pointCounter++;
                    }
                    else
                    {
                        
                        mLineRend.SetPosition(pointCounter, drawTool.transform.position);
                        pointCounter++;                       

                    }
                }
            }
        }
        //else
        //{
        //    //
        //}
    }

    public new void EndCommand() {
        commandActive = false;
        drawTool.SetActive(false);
        if (mGo)
        {
            mGo = null;
            extensionFactor = 0.0f;
            //drawTool.transform.localPosition = originalPosition;
        }
    }


}
