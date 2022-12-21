using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerXPManager : MonoBehaviour
{

    public static PlayerXPManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
        InitialiseXP();
    }

    [SerializeField] private float pointsPerXP = 150; //The amount of points needed to gain 1 xp
    private const string XP_KEY = "xp";

    //Stores all the text components in the game that display the player's currency
    [SerializeField] private List<TextMeshProUGUI> playerXPTexts;
    [SerializeField] private TextMeshProUGUI gainedXPText;

    private void InitialiseXP(){
        int currentXP = PlayerPrefs.GetInt(XP_KEY, 0);
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        bool cheat = true;
        if(cheat){
            PlayerPrefs.SetInt(XP_KEY, 2000);
        }
        UpdateTexts();
    }

    public bool SpendXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        if(currentXP < value) return false;

        currentXP -= value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        UpdateTexts();
        Debug.Log("Spent " + value + " coins - new value:" + GetXP());
        return true;
    }

    public void GainXP(int value){
        int currentXP = PlayerPrefs.GetInt(XP_KEY);
        currentXP += value;
        PlayerPrefs.SetInt(XP_KEY, currentXP);
        UpdateTexts();
    }

    public int GetXP(){
        return PlayerPrefs.GetInt(XP_KEY, 0);
    }

    public void CalculateXPGain(int points){
        Debug.Log("Gaining xp from " + points + " points");
        int newXP = Mathf.FloorToInt(points/pointsPerXP);
        GainXP(newXP);

        gainedXPText.text = "Gained +" + newXP + " coins";
    }

    private void UpdateTexts(){
        foreach(TextMeshProUGUI display in playerXPTexts){
            display.text = GetXP().ToString();
        }
    }
}
