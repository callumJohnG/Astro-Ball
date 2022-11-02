using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType{
    None,
    Bouncy,
}

public class Powerup
{
    private float endTime;
    public PowerupType myType {get; private set;}

    public Powerup(PowerupType type, float endTime){
        this.endTime = endTime;
        myType = type;
    }

    public bool IsPowerupDurationComplete(){
        return(Time.time >= endTime);
    }

    public void SetEndTime(float endTime){
        this.endTime = endTime;
    }
}
