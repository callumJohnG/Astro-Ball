using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType{
    Bouncy,
    Inverted,
    Moon,
    Weak,
    DoublePoints
}

public class Powerup
{
    private float endTime;
    public PowerupType myType {get; private set;}
    public PowerupUI powerupUI {get; private set;}

    public Powerup(PowerupType type, float endTime, PowerupUI powerupUI){
        this.endTime = endTime;
        myType = type;
        this.powerupUI = powerupUI;
    }

    public bool IsPowerupDurationComplete(){
        return(Time.time >= endTime);
    }

    public void SetEndTime(float endTime){
        this.endTime = endTime;
    }
}
