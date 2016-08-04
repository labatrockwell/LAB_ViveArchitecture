using UnityEngine;
using System.Collections;
using System;

public class VRInteractiveItem : MonoBehaviour {

	public event Action OnEnter;
	public event Action OnOver;             // Called when the gaze moves over this object
	public event Action OnOut;              // Called when the gaze leaves this object
	public event Action OnTrigger;           // Called when click input is detected whilst the gaze is over this object.

	protected bool m_IsOver;
	protected bool m_IsActive = false;

	public bool IsOver{
		get { return m_IsOver; }
	}

	public bool IsActive{
		get { return m_IsActive; }
	}

	public void Enter(){
		m_IsOver = true;
		if (OnEnter != null){
			OnEnter();
		}
	}

	public void Over()
	{
		m_IsOver = true;

		if (OnOver != null)
			OnOver();
	}
		
	public void Out()
	{
		m_IsOver = false;

		if (OnOut != null)
			OnOut();
	}


	public void Trigger()
	{
		m_IsActive = !m_IsActive;

		if (OnTrigger != null)
			OnTrigger();
	}
}
