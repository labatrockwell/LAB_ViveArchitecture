using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour {

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

    // Use this for initialization
    void Awake () {
        dimsActive = false;        
        mLineRend = gameObject.AddComponent<LineRenderer>(); //preview the rayCast
        mLineRend.startWidth = 0.025f;
        mLineRend.endWidth = 0.0f;
        commandManager = cmdMgr.GetComponent<CommandManager>();
    }
	
	// Update is called once per frame
	void Update () {
        mRay = new Ray(transform.position, transform.forward);
        mLineRend.SetPosition(0, transform.position);

        if (Physics.Raycast(mRay, out mHit))
        {
            mRayCastHit = true;
            mLineRend.SetPosition(1, mHit.point);
            int deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            SteamVR_Controller.Device mController = SteamVR_Controller.Input(deviceIndex);
            m_CurrentObject = mHit.transform.gameObject;

            //if a command isn't active, show the line renderer
            //TODO: create an event listener for commandActive from the CommandManager
            if (commandManager.isCommandActive)
            {
                mLineRend.enabled = false;
            }
            else
            {
                mLineRend.enabled = true;
            }

            //Interact with Menu Items
            if (m_CurrentObject.GetComponent<VRInteractiveItem>() != null)
            {
                //show the line renderer if we're interacting with a menu item
                mLineRend.enabled = true;                

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
                    
                    EventManagerTypeSafe.instance.TriggerEvent(new CommandInterruptEvent());
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
        }
        else {
            Vector3 endPt = transform.forward * Camera.main.farClipPlane;
            mLineRend.SetPosition(1, endPt);
            mRayCastHit = false;
        }                          

	}
}
