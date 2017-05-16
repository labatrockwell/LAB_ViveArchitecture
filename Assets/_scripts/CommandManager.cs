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
	}

    void OnCommandStart(CommandStartEvent _e) {
        //kill the current command
        if (activeCommand) {
            activeCommand.commandActive = false;
        }

        //parse the command
        string command = _e.command;
        Debug.Log("The Command was: " + command);

        if (command == "dimensionStart") {
            isCommandActive = true;
            activeCommand = GetComponent<DimensionCommand>();
        }

        if (command == "drawStart") {
            isCommandActive = true;
            activeCommand = GetComponent<DrawCommand>();
        }

        if (command == "teleportStart") {
            isCommandActive = true;
            activeCommand = GetComponent<TeleportCommand>();         
        }

        activeCommand.commandActive = true;
        activeCommand.paused = false;
        PauseInactiveCommands();
        DisableInactiveCommands();

    }

    void OnCommandInterrupt(CommandInterruptEvent _e) {
        //current command
        foreach(Command command in mCommands)
        {
            //pause the active command
            if (command.commandActive) {
                command.Pause();
            }
        }        
    }

    void OnCommandResume(CommandResumeEvent _e)
    {
        foreach (Command command in mCommands) {
            if (command.commandActive) {
                command.Unpause();
            }
        }
    }

    void OnCommandEnd(CommandEndEvent _e) {
        isCommandActive = false;
    }

    void PauseInactiveCommands() {

        foreach (Command com in mCommands) {
            if (!com.Equals(activeCommand) ) {
                com.paused = true;
            }
        }
    }

    void DisableInactiveCommands() {
        foreach (Command com in mCommands)
        {
            if (!com.Equals(activeCommand))
            {
                com.commandActive = false;
            }
        }
    }

}
