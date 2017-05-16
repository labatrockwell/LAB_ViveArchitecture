using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour {

    public bool commandActive;
    public string commandName;
    public bool paused = false;

    public void Pause() {
        this.paused = true;
    }

    public void Unpause() {
        this.paused = false;
    }

}
