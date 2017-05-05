using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

// Based on Type-Safe Event System by Will Miller http://www.willrmiller.com/?p=87

// Attach this script to an empty GameObject - name it something like EventManager

//=========== EVENTS - Move this to a separate script ===============
// These are GameEvent classes or Types of events that will be stored in a dictionary with associated delegate functions
// Delegates are essentially references to a method somewhere else

//The empty GameEvent class - this will be extended by other the classes that follow
public class GameEvent {}

//Some examples: 
public class CommandStartEvent : GameEvent
{
	public string command{ get; private set;}
	public CommandStartEvent(string _command){
		this.command = _command;
	}
}

public class CommandEndEvent : GameEvent
{
    public string command { get; private set; }
    public CommandEndEvent(string _command)
    {
        this.command = _command;
    }
}

public class CommandInterruptEvent : GameEvent
{
    public CommandInterruptEvent()
    {
        //this.command = _command;
    }
}

public class CommandResumeEvent : GameEvent
{
    public CommandResumeEvent()
    {
        //this.command = _command;
    }
}

//===================================================================


public class EventManagerTypeSafe : MonoBehaviour 
{
	// declare the Event Manager
	private static EventManagerTypeSafe eventManager;

	//The dictionary that holds all of our events
	private Dictionary<System.Type, System.Delegate> eventDictionary = new Dictionary<System.Type, System.Delegate>();

	//declare the delgate with a specific template type and an event argument called 'e'
	public delegate void EventDelegate<T>(T e) where T : GameEvent;

	//constructor
	public static EventManagerTypeSafe instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType (typeof (EventManagerTypeSafe)) as EventManagerTypeSafe;
				if (!eventManager)
				{
					Debug.LogError ("There needs to be one active EventManager script on a GameObject in your scene.");
				}
				else
				{
					eventManager.Init (); 
				}
			}
			return eventManager;
		}
	}

	public void Init()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<System.Type, System.Delegate>();
		}
	}


	public void AddListener<T> (EventDelegate<T> _delegate) where T : GameEvent
	{
		if (eventDictionary.ContainsKey (typeof(T))) {
			System.Delegate tempDel = eventDictionary [typeof(T)];
			eventDictionary [typeof(T)] = System.Delegate.Combine (tempDel, _delegate);		
		} else {
			eventDictionary [typeof(T)] = _delegate;
		}
	}		

	public void RemoveListener<T> (EventDelegate<T> _delegate) where T : GameEvent
	{
		if (eventDictionary.ContainsKey(typeof(T)))
		{
			var currentDel = System.Delegate.Remove(eventDictionary[typeof(T)], _delegate);
			if (currentDel == null)
			{
				eventDictionary.Remove(typeof(T));
			}
			else
			{
				eventDictionary[typeof(T)] = currentDel;
			}
		}
	}

	public void TriggerEvent(GameEvent e)
	{
		if (e == null)
		{
			Debug.Log("Invalid event argument: " + e.GetType().ToString());
			return;
		}

		if (eventDictionary.ContainsKey(e.GetType()))
		{
			eventDictionary[e.GetType()].DynamicInvoke(e);
		}		
		
	}
}