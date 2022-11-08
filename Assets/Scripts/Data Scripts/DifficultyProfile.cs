using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DifficultyProfile", menuName = "ScriptableObjects/DifficultyProfile", order = 1)]
public class DifficultyProfile : ScriptableObject
{
    public int profileID;
    public float bumperSpacing;
    public float powerupDuration;
    
    public float launchRechargeTime;
    public float defaultGravity;
    public float platformProbability;
    public float pointsBaseMultiplier;
    public float powerupProbability;
    public int coinReward;
    public bool collectPointsOnDeath;

    public List<float> bumperProbabilities;


}
