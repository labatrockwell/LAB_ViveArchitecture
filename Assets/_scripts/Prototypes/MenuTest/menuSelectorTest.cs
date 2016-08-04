using UnityEngine;
using System.Collections;

public class menuSelectorTest : MonoBehaviour {

	//events
	// onEnter
	// onExit
	// onTrigger

	private bool debugRay;
	private LineRenderer m_RayCastLine;
	private GameObject m_CurrentObject;
	private GameObject m_LastObject;

	// Use this for initialization
	void Start () {
		debugRay = true;

		m_RayCastLine = gameObject.AddComponent<LineRenderer> ();
		m_RayCastLine.SetVertexCount (2);
		m_RayCastLine.SetPosition (0, Camera.main.transform.position - new Vector3(0f,1f,0f) );
		m_RayCastLine.SetPosition (1, Vector3.zero);
		m_RayCastLine.SetWidth (0.005f, 0.005f);
	}


	void Update () {

		Ray m_Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit m_Hit;

		if (Physics.Raycast (m_Ray, out m_Hit)) {		
			m_CurrentObject = m_Hit.transform.gameObject;

			if (m_CurrentObject.GetComponent<VRInteractiveItem> () != null) {
				//if the selection has changed
				if (m_CurrentObject != m_LastObject) { 
					
					// the first loop, m_LastObject will be null
					if (m_LastObject != null) {
						if (m_LastObject.GetComponent<VRInteractiveItem> () != null) {
							Debug.Log ("Exited: " + m_LastObject.name);
							m_LastObject.GetComponent<VRInteractiveItem> ().Out ();		
						}
					}

					Debug.Log ("Entered: " + m_CurrentObject.name);
					m_CurrentObject.GetComponent<VRInteractiveItem> ().Enter ();				
				} else {
					//we're over an object
					if (Input.GetMouseButtonDown(0) ){
						m_CurrentObject.GetComponent<VRInteractiveItem> ().Trigger ();
					}
				}	
			
			}


			m_RayCastLine.SetPosition (1, m_Hit.point);
			m_LastObject = m_CurrentObject;

		} else {
			m_RayCastLine.SetPosition (1, Camera.main.transform.position - new Vector3 (0f, 1f, 0f));
		}	
	}
}
