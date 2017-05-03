using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionCommand : MonoBehaviour {

    SteamVR_Controller.Device mController;
    private bool dimCreated;
    private GameObject mDim;
    public bool commandActive;

	// Use this for initialization
	void Start () {

        commandActive = false;
        dimCreated = false;
        mDim = null;
	}
	
	// Update is called once per frame
	void Update () {

        //if (commandActive) {

            Ray mRay = new Ray(transform.position, transform.forward);
            RaycastHit mHit;

            //TODO add layermask to ignore VR interface elements
            if (Physics.Raycast(mRay, out mHit))
            {
                int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
                mController = SteamVR_Controller.Input(deviceIndex);

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
        //}
    }
}
