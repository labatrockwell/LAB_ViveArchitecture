﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    public GameObject cmdMgr;
    private CommandManager commandManager;
    public bool dimsActive;
    public Ray mRay;
    public RaycastHit mHit;
    public bool mRayCastHit;
    private bool dimCreated = false;
    private GameObject mDim = null;
    private LineRenderer mLineRend = null;
    private GameObject m_CurrentObject;
    private GameObject m_LastObject;
    private int deviceIndex;
    private int frameCounter;
    private bool interactingWithMenu;

    // Use this for initialization
    void Awake () {
        interactingWithMenu = false;
        frameCounter = 0;
        dimsActive = false;        
        mLineRend = gameObject.AddComponent<LineRenderer>(); //preview the rayCast
        mLineRend.startWidth = 0.025f;
        mLineRend.endWidth = 0.0f;
        commandManager = cmdMgr.GetComponent<CommandManager>();
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Start() {
        //deviceIndex = SteamVR_Controller.GetDeviceIndex(device);
    }
	
	// Update is called once per frame
	void Update () {
        mRay = new Ray(transform.position, transform.forward);
        mLineRend.SetPosition(0, transform.position);

        //if a command isn't active, show the line renderer
        if (commandManager.isCommandActive)
        {
            mLineRend.enabled = false;
        }
        else
        {
            mLineRend.enabled = true;
        }

        if (Physics.Raycast(mRay, out mHit))
        {
            mRayCastHit = true;
            mLineRend.SetPosition(1, mHit.point);
            
            SteamVR_Controller.Device mController = SteamVR_Controller.Input(deviceIndex);
            m_CurrentObject = mHit.transform.gameObject;

            //Interact with Menu Items
            if (m_CurrentObject.GetComponent<VRInteractiveItem>() != null)
            {
                frameCounter = 0;
                interactingWithMenu = true;
                //show the line renderer if we're interacting with a menu item
                mLineRend.enabled = true;

                //interrupt the current function
                EventManagerTypeSafe.instance.TriggerEvent(new CommandInterruptEvent());

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
                            EventManagerTypeSafe.instance.TriggerEvent(new CommandResumeEvent());
                        }
                    }
                    //Debug.Log("Entered: " + m_CurrentObject.name);
                    m_CurrentObject.GetComponent<VRInteractiveItem>().Enter();
                    
                }
                else
                {         
                    //we're over an object
                    if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        m_CurrentObject.GetComponent<VRInteractiveItem>().Trigger();
                    }
                }

                m_LastObject = m_CurrentObject;
            }
            else {
                //this only needs to be called on the first frame that nothing is being touched
                if (interactingWithMenu ) {
                    frameCounter++;
                    interactingWithMenu = false;
                    Debug.Log("Not interacting with a menu item");
                    //call this only once we leave a menu item
                    EventManagerTypeSafe.instance.TriggerEvent( new CommandResumeEvent() );
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
