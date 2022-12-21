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

    #endregion

    [SerializeField] private DifficultyProfile normalDifficulty;
    

    private void Start(){
        //SetDifficulty(0);
        GetDifficulty();
        pointsMultiplier = 1;
    }

    private void GetDifficulty(){
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        SetDifficulty(difficulty);
    }

    public void SetDifficulty(int difficultyInt){
        switch (difficultyInt){
            case 0 : SetDifficulty(normalDifficulty);break;
        }
    }

    public void SetDifficulty(DifficultyProfile difficultyProfile){
        this.bumperSpacing = difficultyProfile.bumperSpacing;
        this.powerupDuration = difficultyProfile.powerupDuration;
        this.launchRechargeTime = difficultyProfile.launchRechargeTime;
        this.defaultGravity = difficultyProfile.defaultGravity;
        this.platformProbability = difficultyProfile.platformProbability;
        this.bumperProbabilities = difficultyProfile.bumperProbabilities;
        this.powerupProbability = difficultyProfile.powerupProbability;
        this.coinReward = difficultyProfile.coinReward;

        ConfirmAllDifficultyVariables();
    }

    private void ConfirmAllDifficultyVariables(){
        ObjectSpawningManager.Instance.GetDifficultySettings();
    }

}
