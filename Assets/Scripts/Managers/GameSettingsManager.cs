using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlScheme{
    Keyboard,
    Mouse,
    GamePad
}

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    private void Awake(){
        Instance = this;
    }

    public float maxRenderDistance = 300;
    public float bumperSpacing = 20;
    public bool inverseAiming;
    public ControlScheme controlScheme;
    public float powerupDuration = 10;
}
