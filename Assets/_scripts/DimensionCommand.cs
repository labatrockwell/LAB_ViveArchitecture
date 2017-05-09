using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionCommand : Command {

    private bool dimCreated;
    private GameObject mDim;
    public GameObject rightController;
    private LineRenderer dimLineRenderer;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device mController { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

	// Use this for initialization
	void Start () {
        commandActive = false;
        dimCreated = false;
        mDim = null;

        trackedObject = rightController.GetComponent<SteamVR_TrackedObject>();

        dimLineRenderer = gameObject.AddComponent<LineRenderer>();
        dimLineRenderer.startWidth= 0.025f;
        dimLineRenderer.endWidth = 0.025f;
        Material lineMat = new Material(Shader.Find("Unlit/Color"));
        lineMat.color = Color.cyan;

        dimLineRenderer.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (!trackedObject) {
            trackedObject = rightController.GetComponent<SteamVR_TrackedObject>();
        }

        if (commandActive && !paused)
        {
            //Debug.Log("Dimension Command is active! From DimensionCommand.cs");
            //the ray from the right controller
            Ray mRay = rightController.GetComponent<RayCastController>().mRay;
            RaycastHit mHit = rightController.GetComponent<RayCastController>().mHit;
            bool raycastHit = rightController.GetComponent<RayCastController>().mRayCastHit;

            //int deviceIndex = SteamVR_Controller.GetDeviceIndex()
            //int deviceIndex = SteamVR_Controller.GetDeviceIndex(mController);
            //mController = SteamVR_Controller.Input(deviceIndex);

            dimLineRenderer.enabled = true;
            dimLineRenderer.SetPosition(0, rightController.transform.position);

            //TODO add layermask to ignore VR interface elements
            if (raycastHit)
            {
                //Debug.Log("We got a hit! From DimensionCommand.cs");

                dimLineRenderer.SetPosition(1, mHit.point);

                if (dimCreated)
                {
                    mDim.GetComponent<Dimension>().dimPtB.transform.position = mHit.point;
                    //if (deviceIndex != -1)
                    //{
                        if (mController.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                        {
                            Vector2 touchPad = mController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                            //add or subrtact extenstions based on touchpad y
                            Debug.Log("TouchPad Y: " + touchPad.y);
                            mDim.GetComponent<Dimension>().offset = touchPad.y;
                        }
                    //}
                }
            }
            else {
                Vector3 endPt = rightController.transform.forward * Camera.main.farClipPlane;
                dimLineRenderer.SetPosition(1, endPt);
            }

            //if (deviceIndex != -1 && mController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            if( mController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) )
            {
                Debug.Log("Right Trigger Pulled!");
                if (mHit.collider != null)
                {
                    if (!dimCreated)
                    {
                        //create new dimension pt A
                        dimCreated = true;
                        mDim = GameObject.Instantiate(Resources.Load("_prefabs/Dimension") as GameObject);
                        mDim.GetComponent<Dimension>().dimPtA.transform.position = mHit.point;
                    }
                    else
                    {
                        mDim.GetComponent<Dimension>().dimPtB.transform.position = mHit.point;
                        dimCreated = false;
                        //commandActive = false;
                    }
                }
            }
        }
        else {
            dimLineRenderer.enabled = false;
        }
    }
}
