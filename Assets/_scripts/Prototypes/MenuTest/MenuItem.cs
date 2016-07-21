using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	private VRInteractiveItem m_InteractiveItem;

	void OnEnable(){

		m_InteractiveItem = gameObject.GetComponent<VRInteractiveItem> ();

		m_InteractiveItem.OnEnter += HandleEnter;
		m_InteractiveItem.OnTrigger += HandleTrigger;
		m_InteractiveItem.OnOut += HandleOut;
	}

	void OnDisable(){
		m_InteractiveItem.OnEnter -= HandleEnter;
		m_InteractiveItem.OnOut -= HandleOut;
	}

	void HandleEnter(){
		revealSubmenus ();
	}

	void HandleTrigger(){
//		Debug.Log ("Triggered delegate Trigger function for: " + gameObject.name);
		revealSubmenus ();
	}

	void HandleOut(){
//		Debug.Log ("Triggered delegate Trigger function for: " + gameObject.name);
		hideSubmenus ();
	}

	void revealSubmenus(){
		foreach (Renderer subMenu in gameObject.GetComponentsInChildren<Renderer>() ) {
			//exclude the parent
			if (subMenu.gameObject != gameObject) {
				subMenu.enabled = true;			
			}
		}
	}

	void hideSubmenus(){
		foreach (Renderer subMenu in gameObject.GetComponentsInChildren<Renderer>() ) {
			//exclude the parent
			if (subMenu.gameObject != gameObject) {
				subMenu.enabled = false;			
			}

		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
