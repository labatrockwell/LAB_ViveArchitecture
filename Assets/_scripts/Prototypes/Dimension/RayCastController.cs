using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour {

    //Ray mRay; 
    private bool dimCreated = false;
    private GameObject mDim = null;
    private LineRenderer mLineRend = null;
    	
    // Use this for initialization
	void Start () {
        //mRay = new Ray(transform.position, transform.forward);
        Debug.Log(Input.GetJoystickNames() );
        foreach (string name in Input.GetJoystickNames()) {
            Debug.Log(name);
        }
        
        //preview the rayCast
        mLineRend = gameObject.AddComponent<LineRenderer>();
        mLineRend.startWidth = 0.05f;
        mLineRend.endWidth = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        Ray mRay = new Ray(transform.position, transform.forward);
        RaycastHit mHit;

        mLineRend.SetPosition(0, transform.position);

        //THE STEAM VR WAY
        if (Physics.Raycast(mRay, out mHit))
        {
            //Debug.Log("Raycast Hit: " + mHit.transform.gameObject.name);
            mLineRend.SetPosition(1, mHit.point);
            int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

            SteamVR_Controller.Device mController = SteamVR_Controller.Input(deviceIndex);

            if (dimCreated) {
                mDim.GetComponent<Dimension>().dimPtB.transform.position = mHit.point;

                if (deviceIndex != -1) {

                    if ( mController.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
                        Vector2 touchPad = mController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                        //add or subrtact extenstions based on touchpad y
                        Debug.Log("TouchPad Y: " + touchPad.y);
                        mDim.GetComponent<Dimension>().offset = touchPad.y;
                    }

                    //if (mController.GetPressDown(SteamVR_Controller.ButtonMask.)) {

                    //}

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
                    }

                }
            }

        }
        else {
            Vector3 endPt = transform.forward * Camera.main.farClipPlane;
            mLineRend.SetPosition(1, endPt);
        }                   

        Debug.DrawRay(transform.position, transform.forward, Color.red);

       
        //LineRenderer mLR = gameObject.AddComponent<LineRenderer>();
        //mLR.positionCount = 2;
        //mLR.SetPosition(0, transform.position);

        //RaycastHit hit;
        //Physics.Raycast(transform.position, transform.forward, hit);            

	}
}
