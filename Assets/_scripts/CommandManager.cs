using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour {

    public bool isCommandActive { get; private set; }
    public GameObject rightController;
    LineRenderer controllerRayCast = null;
    private Command[] mCommands;
    public Command activeCommand = null; //start with no active command    
    
	// Use this for initialization
	void Start () {
        isCommandActive = false;
        mCommands = GetComponentsInChildren<Command>();
    }

    void OnEnable()
    {
        EventManagerTypeSafe.instance.AddListener<CommandStartEvent>(OnCommandStart);
        EventManagerTypeSafe.instance.AddListener<CommandEndEvent>(OnCommandEnd);
        EventManagerTypeSafe.instance.AddListener<CommandInterruptEvent>(OnCommandInterrupt);
        EventManagerTypeSafe.instance.AddListener<CommandResumeEvent>(OnCommandResume);
    }

    void OnDisable()
    {
        EventManagerTypeSafe.instance.RemoveListener<CommandStartEvent>(OnCommandStart);
        EventManagerTypeSafe.instance.RemoveListener<CommandEndEvent>(OnCommandEnd);
        EventManagerTypeSafe.instance.RemoveListener<CommandInterruptEvent>(OnCommandInterrupt);
        EventManagerTypeSafe.instance.RemoveListener<CommandResumeEvent>(OnCommandResume);
    }
	
	// Update is called once per frame
	void Update () {

        //if (GetComponent<DrawCommand>().commandActive || GetComponent<TeleportCommand>().commandActive)
        //{
        //    //turn off the lineRenderer
        //    controllerRayCast = rightController.GetComponent<LineRenderer>();
        //    controllerRayCast.enabled = false;
        //}
        //else {
        //    controllerRayCast = rightController.GetComponent<LineRenderer>();
        //    controllerRayCast.enabled = true;
        //}
        		
	}

    void OnCommandStart(CommandStartEvent _e) {
        //kill the current command

        //parse the command
        string command = _e.command;
        Debug.Log("The Command was: " + command);

        if (command == "dimensionStart") {
            isCommandActive = true;
            activeCommand = GetComponent<DimensionCommand>();

            GetComponent<DimensionCommand>().commandActive = true;
            GetComponent<DrawCommand>().commandActive = false;
            GetComponent<TeleportCommand>().commandActive = false;
            //disable all other commands
        }

        if (command == "drawStart") {
            isCommandActive = true;
            activeCommand = GetComponent<DrawCommand>();

            GetComponent<DimensionCommand>().commandActive = false;
            GetComponent<DrawCommand>().commandActive = true;
            GetComponent<TeleportCommand>().commandActive = false;
            //disable all other commands
        }

        if (command == "teleportStart") {
            isCommandActive = true;
            activeCommand = GetComponent<TeleportCommand>();

            GetComponent<DimensionCommand>().commandActive = false;
            GetComponent<DrawCommand>().commandActive = false;
            GetComponent<TeleportCommand>().commandActive = true;
            //disable all other commands           
        }
    }

    void OnCommandInterrupt(CommandInterruptEvent _e) {
        //current command
        foreach(Command command in mCommands)
        {
            //pause the active command
            if (command.commandActive) {
                command.paused = true;
            }
        }        
    }

    void OnCommandResume(CommandResumeEvent _e)
    {
        foreach (Command command in mCommands) { 
            if (command.paused)
            {
                command.paused = false;
            }
        }
    }

    void OnCommandEnd(CommandEndEvent _e) {
        isCommandActive = false;
    }

}
