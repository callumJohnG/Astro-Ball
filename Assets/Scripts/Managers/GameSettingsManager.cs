using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    private void Awake(){
        Instance = this;

        GetInverseAimSetting();
    }

    [HideInInspector] public float maxRenderDistance = 300;
    [HideInInspector] public bool inverseAiming;
    [HideInInspector] public bool flipAim;
    [SerializeField] private bool isDevEnv;

    #region Difficulty Variables
    [HideInInspector] public float bumperSpacing = 20;
    [HideInInspector] public float powerupDuration = 10;
    [HideInInspector] public float launchRechargeTime;
    [HideInInspector] public float defaultGravity = 4;
    [HideInInspector] public float platformProbability;
    [HideInInspector] public float powerupProbability;
    [HideInInspector] public float pointsMultiplier = 1;
    [HideInInspector] public int coinReward;
    [HideInInspector] public List<float> bumperProbabilities;
    [HideInInspector] public int maxGameHeight = 1000;
    [HideInInspector] public int minGameHeight = 60;
    [HideInInspector] public float lavaStartSpeed;
    [HideInInspector] public float lavaMaxSpeed;
    [HideInInspector] public float lavaAcceleration;    
    [HideInInspector] public float lavaDistance;

    #endregion


    [HideInInspector] public bool canPostScore;
    private const string CANPOSTSCORE_KEY = "CanPostScore";
    public void SetCanPostScore(bool canPostScore){
        this.canPostScore = canPostScore;
        if(canPostScore) PlayerPrefs.SetInt(CANPOSTSCORE_KEY, 1);
        else PlayerPrefs.SetInt(CANPOSTSCORE_KEY, 0);

    }

    [SerializeField] private DifficultyProfile normalDifficulty;
    [SerializeField] private DifficultyProfile challengeDifficulty;
    [SerializeField] private RisingLava risingLava;
    

    private void Start(){
        SetDifficulty(0);
        pointsMultiplier = 1;
        SetCanPostScore(PlayerPrefs.GetInt(CANPOSTSCORE_KEY, 1) == 1);
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
        this.lavaStartSpeed = difficultyProfile.lavaStartSpeed;
        this.lavaMaxSpeed = difficultyProfile.lavaMaxSpeed;
        this.lavaAcceleration = difficultyProfile.lavaAcceleration;
        this.lavaDistance = difficultyProfile.lavaDistance;

        ConfirmAllDifficultyVariables();
    }

    private void ConfirmAllDifficultyVariables(){
        if(isDevEnv) return;

        ObjectSpawningManager.Instance.GetDifficultySettings();
        risingLava.SetSpeed(lavaStartSpeed, lavaMaxSpeed, lavaAcceleration, lavaDistance);
    }

    private const string INVERSE_AIM_KEY = "InverseAiming";

    private void GetInverseAimSetting(){
        inverseAiming = PlayerPrefs.GetInt(INVERSE_AIM_KEY, 0) == 1;
    }

    public void SetInverseAim(bool inverseAiming){
        this.inverseAiming = inverseAiming;
        PlayerPrefs.SetInt(INVERSE_AIM_KEY, inverseAiming?1:0);
    }

}
