using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrMenu : MonoBehaviour {

    private SteamVR_Controller.Device leftController;


    // Use this for initialization
    void Start () {

        List<Renderer> mRenderers = new List<Renderer>();
        //disable all mesh renderers in children
        foreach (Renderer rend in mRenderers)
        {
            rend.enabled = false;
        }

        int leftDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        leftController = SteamVR_Controller.Input(leftDeviceIndex);

        GameObject leftControlGmOb = GameObject.Find("Controller (left)");
        Debug.Log(leftControlGmOb);

        //if the left controller exists ...
        if (leftController != null) {
            //parent this object to the left controller
            gameObject.transform.parent = leftControlGmOb.transform;
            //enable all mesh renderers
            foreach (Renderer rend in mRenderers){
                rend.enabled = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        //if the controller isn't active - check to see if its become active
        if (leftController == null) {
            //try to get the controller
            int leftDeviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
            leftController = SteamVR_Controller.Input(leftDeviceIndex);
            if (leftController != null)
            {
                List<Renderer> mRenderers = new List<Renderer>();
                //parent this object to the left controller
                transform.parent = GameObject.Find("Controller (left)").transform;
                //enable all mesh renderers
                foreach (Renderer rend in mRenderers)
                {
                    rend.enabled = true;
                }
            }
        }

	}
}
