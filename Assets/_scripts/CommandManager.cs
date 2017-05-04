using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour {

    private string activeCommand;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        EventManagerTypeSafe.instance.AddListener<CommandEvent>(OnCommandStart);
    }

    void OnDisable()
    {
        EventManagerTypeSafe.instance.RemoveListener<CommandEvent>(OnCommandStart);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCommandStart(CommandEvent _e) {
        //kill the current command

        //parse the command
        string command = _e.command;
        Debug.Log("The Command was: " + command);

        if (command == "dimensionStart") {
            GetComponent<DimensionCommand>().commandActive = true;
        }

        if (command == "drawStart") {
            GetComponent<DrawCommand>().commandActive = true;
        }
    }
}
