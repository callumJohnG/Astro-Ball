using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreManager : MonoBehaviour
{

    public static HighscoreManager Instance;

    private void Start(){
        Instance = this;
    }

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Transform highscoreArea;
    [SerializeField] private TextMeshProUGUI highscoreText;

    public void SetData(int points, int combo){
        float highscore = GetHighscore();
        if(points > highscore){
            SetHighscore(points);
            highscore = points;
        }
        highscoreText.text = "Highscore : " + highscore.ToString();

        string pointsString = points + " Points\nBest Combo : " + combo;
        pointsText.text = pointsString;
    }

    private int GetHighscore(){
        switch (PlayerPrefs.GetInt("Difficulty", 0)){
            case 0 : return PlayerPrefs.GetInt("HighscoreNormal", 0);
            case 1 : return PlayerPrefs.GetInt("HighscoreHard", 0);
            case 2 : return PlayerPrefs.GetInt("HighscoreInsane", 0);
        }
        return 0;
    }

    private void SetHighscore(int points){
        switch (PlayerPrefs.GetInt("Difficulty", 0)){
            case 0 : PlayerPrefs.SetInt("HighscoreNormal", points); break;
            case 1 : PlayerPrefs.SetInt("HighscoreHard", points); break;
            case 2 : PlayerPrefs.SetInt("HighscoreInsane", points); break;
        }
    }
}
