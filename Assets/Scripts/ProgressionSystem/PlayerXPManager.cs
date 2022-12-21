using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXPManager : MonoBehaviour
{

    public static PlayerXPManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
        InitialiseXP();
    }

    [SerializeField] private float pointsPerXP = 150; //The amount of points needed to gain 1 xp
    private const string XP_KEY = "xp";

    private void InitialiseXP(){
        int currentXP = PlayerPrefs.GetInt(XP_KEY, 0);
        PlayerPrefs.SetInt(XP_KEY, currentXP);
    }

    public bool SpendXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        if(currentXP < value) return false;

        currentXP -= value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        return true;
    }

    public void GainXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        currentXP += value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
    }

    public int GetXP(){
        return PlayerPrefs.GetInt(XP_KEY, 0);
    }

    public void CalculateXPFromPoints(int points){
        Debug.Log("Gaining xp from " + points + " points");
        int newXP = Mathf.FloorToInt(points/pointsPerXP);
        Debug.Log("Gained " + newXP + " xp");
        GainXP(newXP);
        Debug.Log("Current XP = " + GetXP());
    }
}
