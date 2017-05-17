using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour {

    public bool commandActive;
    public string commandName;
    public bool paused = false;

    public virtual void Pause() {
        this.paused = true;
    }

    public virtual void Unpause() {
        this.paused = false;
    }

    public virtual void StartCommand() {
        Debug.Log("Base Class");
        this.commandActive = true;
    }

    public virtual void PauseCommand() {
        this.paused = true;
    }

    public virtual void ResumeCommand() {
        this.paused = false;
    }

    public virtual void EndCommand() {
        this.commandActive = false;
    }

}
