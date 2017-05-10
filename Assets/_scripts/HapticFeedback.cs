using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticFeedback : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

    // Use this for initialization
    void Start () {
        trackedObject = gameObject.GetComponent<SteamVR_TrackedObject>();
    }
	

    void OnCollisionEnter(Collision col) {
        Debug.LogWarning("Hit Something!");
        SteamVR_Controller.Input((int)device.index).TriggerHapticPulse(500);
    }
}
