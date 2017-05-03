using UnityEngine;
using System.Collections;

public class SubMenuItem : MonoBehaviour {

	private VRInteractiveItem m_InteractiveItem;
	private bool m_EnterAnimationRunning;
	private GameObject m_ParentMenu;

    public string command;

	void OnEnable(){

		m_InteractiveItem = gameObject.GetComponent<VRInteractiveItem> ();
		m_ParentMenu = gameObject.transform.parent.gameObject;
		m_EnterAnimationRunning = false;

		m_InteractiveItem.OnEnter += HandleEnter;
		m_InteractiveItem.OnTrigger += HandleTrigger;
		m_InteractiveItem.OnOut += HandleOut;
	}

	void OnDisable(){
		m_InteractiveItem.OnEnter -= HandleEnter;
		m_InteractiveItem.OnTrigger -= HandleTrigger;
		m_InteractiveItem.OnOut -= HandleOut;
	}

	void HandleEnter(){
//		Debug.Log ("Triggered delegate Enter function for: " + gameObject.name);
//		Debug.Log ("This object is a child of : " + m_ParentMenu);
		if (!m_EnterAnimationRunning) {
			m_EnterAnimationRunning = true;
			StartCoroutine(EnterAnimation(gameObject.transform.localScale.x));
		}
	}

	void HandleTrigger(){
        //		Debug.Log ("Triggered delegate Trigger function for: " + gameObject.name);
        //		Debug.Log ("This object is a child of : " + m_ParentMenu);
        //CALL SOME FUNCTION - eventManager, execute command with ID or string Name
        EventManagerTypeSafe.instance.TriggerEvent(new CommandEvent(command));        
		//HIDE ALL MENUS
		m_ParentMenu.GetComponent<VRInteractiveItem>().Trigger();
	}

	void HandleOut(){
//		Debug.Log ("Triggered delegate Exit function for: " + gameObject.name);
//		Debug.Log ("This object is a child of : " + m_ParentMenu);
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
}
