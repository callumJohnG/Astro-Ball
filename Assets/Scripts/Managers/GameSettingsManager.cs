using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    private void Awake(){
        Instance = this;
    }

    [HideInInspector] public float maxRenderDistance = 300;
    [HideInInspector] public bool inverseAiming;

    #region Difficulty Variables
    [HideInInspector] public float bumperSpacing = 20;
    [HideInInspector] public float powerupDuration = 10;
    [HideInInspector] public float launchRechargeTime;
    [HideInInspector] public float defaultGravity;
    [HideInInspector] public float platformProbability;
    [HideInInspector] public float powerupProbability;
    [HideInInspector] public float pointsMultiplier = 1;
    [HideInInspector] public int coinReward;
    [HideInInspector] public List<float> bumperProbabilities;
    [HideInInspector] public int maxGameHeight = 1000;
    [HideInInspector] public int minGameHeight = 60;
    [HideInInspector] public float lavaSpeed;
    [HideInInspector] public float lavaDistance;

    #endregion

    [SerializeField] private DifficultyProfile normalDifficulty;
    [SerializeField] private DifficultyProfile challengeDifficulty;
    [SerializeField] private RisingLava risingLava;
    

    private void Start(){
        SetDifficulty(0);
        pointsMultiplier = 1;
    }


    public void SetDifficulty(int difficultyInt){
        switch (difficultyInt){
            case 0 : SetDifficulty(normalDifficulty);break;
            case 1 : SetDifficulty(challengeDifficulty);break;
        }
    }

    public void SetDifficulty(DifficultyProfile difficultyProfile){
        PlayerPrefs.SetInt("Difficulty", difficultyProfile.profileID);
        this.bumperSpacing = difficultyProfile.bumperSpacing;
        this.powerupDuration = difficultyProfile.powerupDuration;
        this.launchRechargeTime = difficultyProfile.launchRechargeTime;
        this.defaultGravity = difficultyProfile.defaultGravity;
        this.platformProbability = difficultyProfile.platformProbability;
        this.bumperProbabilities = difficultyProfile.bumperProbabilities;
        this.powerupProbability = difficultyProfile.powerupProbability;
        this.coinReward = difficultyProfile.coinReward;
        this.lavaSpeed = difficultyProfile.lavaRiseSpeed;
        this.lavaDistance = difficultyProfile.lavaDistance;

        ConfirmAllDifficultyVariables();
    }

    private void ConfirmAllDifficultyVariables(){
        ObjectSpawningManager.Instance.GetDifficultySettings();
        risingLava.SetSpeed(lavaSpeed, lavaDistance);
    }

}
