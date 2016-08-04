﻿using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	private VRInteractiveItem m_InteractiveItem;
	private bool m_EnterAnimationRunning;
	private bool m_SubMenuActive;

	private GameObject[] m_SubMenus;
	private Vector3[] m_SubMenuPositions;
	private Vector3 m_MenuOrigin;
	private bool m_TriggerAnimationPlaying;

	void OnEnable(){
		m_InteractiveItem = gameObject.GetComponent<VRInteractiveItem> ();
		m_EnterAnimationRunning = false;
		m_SubMenuActive = false;

		m_InteractiveItem.OnEnter += HandleEnter;
		m_InteractiveItem.OnTrigger += HandleTrigger;
		m_InteractiveItem.OnOut += HandleOut;

		m_SubMenuPositions = retrieveSubmenuPositions ();
		m_MenuOrigin = gameObject.transform.position;
		m_SubMenus = retrieveSubmenuObjects ();

		m_TriggerAnimationPlaying = false;


		for (int i = 0; i < m_SubMenus.Length; i++) {
			StartCoroutine (TriggerAnimation (m_SubMenus [i], m_SubMenuPositions [i], m_MenuOrigin, new Vector3(.5f,.5f,.5f), new Vector3(0f,0f,0f), m_InteractiveItem.IsActive));
		}


	}

	void OnDisable(){
		m_InteractiveItem.OnEnter -= HandleEnter;
		m_InteractiveItem.OnTrigger -= HandleTrigger;
		m_InteractiveItem.OnOut -= HandleOut;
	}

	void HandleEnter(){
		if (!m_SubMenuActive) {
			if (!m_EnterAnimationRunning) {
				m_EnterAnimationRunning = true;
				StartCoroutine(EnterAnimation(gameObject.transform.localScale.x));
			}		
		}
	}

	void HandleTrigger(){
//		Debug.Log ("Triggered delegate Trigger function for: " + gameObject.name);
		for (int i = 0; i < m_SubMenus.Length; i++) {			
			if (m_InteractiveItem.IsActive == true) {
				StartCoroutine (TriggerAnimation (m_SubMenus [i], m_MenuOrigin, m_SubMenuPositions [i], new Vector3(0f,0f,0f), new Vector3(.5f,.5f,.5f), m_InteractiveItem.IsActive));
			} else {
				StartCoroutine (TriggerAnimation (m_SubMenus [i], m_SubMenuPositions [i], m_MenuOrigin, new Vector3(.5f,.5f,.5f), new Vector3(0f,0f,0f), m_InteractiveItem.IsActive));			
			}
		}
	}

	void HandleOut(){
//		Debug.Log ("Triggered delegate Trigger function for: " + gameObject.name);
//		hideSubmenus ();
	}

	void revealSubmenus(){
		m_SubMenuActive = true;
		for (int i = 0; i < m_SubMenus.Length; i++) {
			m_SubMenus[i].GetComponent<Renderer>().enabled = true;
			m_SubMenus[i].gameObject.GetComponent<Collider> ().enabled = true;		
		}
	}

	void hideSubmenus(){
		m_SubMenuActive = false;
		for (int i = 0; i < m_SubMenus.Length; i++) {
			m_SubMenus[i].GetComponent<Renderer>().enabled = false;
			m_SubMenus[i].gameObject.GetComponent<Collider> ().enabled = false;		
		}
	}

		
	IEnumerator EnterAnimation(float _originalScale){
		float timeLimit = 0.125f;
		for (float f = 0f; f < timeLimit; f += Time.deltaTime) {
			float scale = Mathf.Sin (f * Mathf.PI)*.125f + _originalScale;
			gameObject.transform.localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
		gameObject.transform.localScale = new Vector3 (_originalScale, _originalScale, _originalScale);
		m_EnterAnimationRunning = false;
	}

	IEnumerator TriggerAnimation(GameObject _subMenu, Vector3 _startPos, Vector3 _endPos, Vector3 _startScale, Vector3 _endScale, bool _submenuVisible){

		if (_submenuVisible) {
			_subMenu.GetComponent<Renderer> ().enabled = true;
			_subMenu.gameObject.GetComponent<Collider> ().enabled = true;			
		}

		float timeLimit = .125f;
		for (float f = 0f; f < 3.0; f += Time.deltaTime/timeLimit) {
			_subMenu.transform.position = Vector3.Lerp (_startPos, _endPos, f);
			_subMenu.transform.localScale = Vector3.Lerp (_startScale, _endScale, f);
			yield return null;
		}

		if(!_submenuVisible) {
			_subMenu.GetComponent<Renderer> ().enabled = false;
			_subMenu.gameObject.GetComponent<Collider> ().enabled = false;		
		}

		m_TriggerAnimationPlaying = false;
	}

	private Vector3[] retrieveSubmenuPositions(){
		Vector3[] positions = new Vector3[gameObject.GetComponentsInChildren<Renderer>().Length-1];
		int iterator = 0;
		foreach (Renderer subMenu in gameObject.GetComponentsInChildren<Renderer>() ) {
			//exclude the parent
			if (subMenu.gameObject.tag == "Submenu") {
				positions[iterator] = subMenu.gameObject.transform.position;
				iterator++;
			}
		}
		return positions;		
	}
	
	private GameObject[] retrieveSubmenuObjects(){
		GameObject[] gameObjects = new GameObject[gameObject.GetComponentsInChildren<Renderer>().Length-1];
		int iterator = 0;
		foreach (Renderer subMenu in gameObject.GetComponentsInChildren<Renderer>() ) {
			//exclude the parent
			if (subMenu.gameObject.tag == "Submenu") {
				gameObjects[iterator] = subMenu.gameObject;
				iterator++;
			}
		}
		return gameObjects;		
	}

}
