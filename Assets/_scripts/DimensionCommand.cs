﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionCommand : MonoBehaviour {

    SteamVR_Controller.Device mController;
    private bool dimCreated;
    private GameObject mDim;
    public bool commandActive;
    public GameObject rightController;

	// Use this for initialization
	void Start () {

        commandActive = false;
        dimCreated = false;
        mDim = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (commandActive)
        {
            //Debug.Log("Dimension Command is active! From DimensionCommand.cs");
            //the ray from the right controller
            Ray mRay = rightController.GetComponent<RayCastController>().mRay;
            RaycastHit mHit = rightController.GetComponent<RayCastController>().mHit;
            bool raycastHit = rightController.GetComponent<RayCastController>().mRayCastHit;

            int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            mController = SteamVR_Controller.Input(deviceIndex);

            //TODO add layermask to ignore VR interface elements
            if (raycastHit)
            {
                Debug.Log("We got a hit! From DimensionCommand.cs");

                if (dimCreated)
                {
                    mDim.GetComponent<Dimension>().dimPtB.transform.position = mHit.point;
                    if (deviceIndex != -1)
                    {
                        if (mController.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                        {
                            Vector2 touchPad = mController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                            //add or subrtact extenstions based on touchpad y
                            Debug.Log("TouchPad Y: " + touchPad.y);
                            mDim.GetComponent<Dimension>().offset = touchPad.y;
                        }
                    }
                }
            }

            if (deviceIndex != -1 && mController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
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
                        commandActive = false;
                    }
                }
            }
        }
    }
}
