using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    private void Awake(){
        Instance = this;
    }

    public float maxRenderDistance = 300;
}
