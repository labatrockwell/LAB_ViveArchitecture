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
    private List<Vector3> mPolyLinePts;

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
        mPolyLinePts = new List<Vector3>();
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

    public override void StartCommand() {
        commandActive = true;
        drawTool.SetActive(true);
    }

    public override void PauseCommand()
    {
        paused = true;
        drawTool.SetActive(false);
    }

    public override void ResumeCommand()
    {
        paused = false;
        drawTool.SetActive(true);
    }

    public override void EndCommand()
    {
        //eliminate last position in the linerender
        mLineRend.positionCount = mPolyLinePts.Count;

        commandActive = false;
        drawTool.SetActive(false);
        drawTool.transform.localPosition = originalPosition;
        mGo = null;
        mLineRend = null;
        extensionFactor = 0.0f;
        mPolyLinePts.Clear();
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
                        mPolyLinePts.Add(drawTool.transform.position);
                        //mLineRend.SetPosition(0, drawTool.transform.position);
                        pointCounter++;
                    }
                    else
                    {
                        mPolyLinePts.Add(drawTool.transform.position);
                        //mLineRend.SetPosition(pointCounter, drawTool.transform.position);
                        pointCounter++;          
                    }
                }

                Vector3[] points = new Vector3[mPolyLinePts.Count+1];
                Debug.Log("Size: " + points.Length);

                //copy the list points into the array
                for (int i = 0; i < points.Length - 1; i++)
                {
                    points[i] = mPolyLinePts[i];
                }

                points[points.Length - 1] = drawTool.transform.position;
                mLineRend.positionCount = points.Length;
                mLineRend.SetPositions(points);
            }
        }
    }
}
