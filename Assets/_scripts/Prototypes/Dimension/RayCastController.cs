using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour {

    //Ray mRay; 
    private bool dimCreated = false;
    private GameObject mDim = null;
    private LineRenderer mLineRend = null;
    public bool dimsActive;
    private GameObject m_CurrentObject;
    private GameObject m_LastObject;

    public Ray mRay;
    public RaycastHit mHit;
    public bool mRayCastHit;

    // Use this for initialization
    void Start () {
        //mRay = new Ray(transform.position, transform.forward);
        //Debug.Log(Input.GetJoystickNames() );
        //foreach (string name in Input.GetJoystickNames()) {
        //    Debug.Log(name);
        //}
        dimsActive = false;        
        //preview the rayCast
        mLineRend = gameObject.AddComponent<LineRenderer>();
        mLineRend.startWidth = 0.025f;
        mLineRend.endWidth = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        mRay = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(mRay, out mHit))
        {
            mRayCastHit = true;
            mLineRend.SetPosition(0, transform.position);
            //Debug.Log("Raycast Hit: " + mHit.transform.gameObject.name);
            mLineRend.SetPosition(1, mHit.point);
            int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            SteamVR_Controller.Device mController = SteamVR_Controller.Input(deviceIndex);
            m_CurrentObject = mHit.transform.gameObject;

            //Interact with Menu Items
            if (m_CurrentObject.GetComponent<VRInteractiveItem>() != null)
            {
                //if the selection has changed
                if (m_CurrentObject != m_LastObject)
                {

                    // the first loop, m_LastObject will be null
                    if (m_LastObject != null)
                    {
                        if (m_LastObject.GetComponent<VRInteractiveItem>() != null)
                        {
                            //Debug.Log("Exited: " + m_LastObject.name);
                            m_LastObject.GetComponent<VRInteractiveItem>().Out();
                        }
                    }

                    //Debug.Log("Entered: " + m_CurrentObject.name);
                    m_CurrentObject.GetComponent<VRInteractiveItem>().Enter();
                }
                else
                {
                    //we're over an object
                    if (deviceIndex != -1 && mController.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        m_CurrentObject.GetComponent<VRInteractiveItem>().Trigger();
                    }
                }

                m_LastObject = m_CurrentObject;
            }

            if (dimsActive) {
                if (dimCreated) {
                    mDim.GetComponent<Dimension>().dimPtB.transform.position = mHit.point;
                    if (deviceIndex != -1) {
                        if ( mController.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
                            Vector2 touchPad = mController.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                            //add or subrtact extenstions based on touchpad y
                            //Debug.Log("TouchPad Y: " + touchPad.y);
                            mDim.GetComponent<Dimension>().offset = touchPad.y;
                        }
                    }
                }

                if (deviceIndex != -1 && mController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    //Debug.Log("Right Trigger Pulled!");
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
        }
        else {
            Vector3 endPt = transform.forward * Camera.main.farClipPlane;
            mLineRend.SetPosition(1, endPt);
            mRayCastHit = false;
        }                          

	}
}
