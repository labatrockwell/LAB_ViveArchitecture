using UnityEngine;
using System.Collections;

// Based on Type-Safe Event System by Will Miller http://www.willrmiller.com/?p=87

// Attach this script to a GameObject that will respond to GameEvents
// The class methods are project specific, but this will outline how to use a delegate funciton with parameters

// The events can be called from another object by calling:
// EventManagerTypeSafe.instance.TriggerEvent(new ReticleEndEvent());

public class EventRegister : MonoBehaviour {
	
	void Start(){

	}

	void OnEnable()
	{
		EventManagerTypeSafe.instance.AddListener<ChangeOfTimeEvent> (OnChangeTime);
	}

	void OnDisable()
	{
		//When the object is disabled, tell the Event Manager to remove it to the Event Dictionary
		EventManagerTypeSafe.instance.RemoveListener<ChangeOfTimeEvent> (OnChangeTime);
	}

	void OnChangeTime(ChangeOfTimeEvent _e){
		print("Change of time event!");
		//Do stuff to the audio emitter
	}

}