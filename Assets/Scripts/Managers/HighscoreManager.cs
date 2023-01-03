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
    [SerializeField] private TextMeshProUGUI bestComboText;
    [SerializeField] private Transform highscoreArea;
    [SerializeField] private TextMeshProUGUI highscoreText;

    public void SetData(int points, int combo){
        float highscore = GetHighscore();
        if(points > highscore){
            SetHighscore(points);
            highscore = points;
        }
        highscoreText.text = "Highscore : " + highscore.ToString();

        pointsText.text = "Points : " + points;
        bestComboText.text = "Best Combo : " + combo;
    }

    private int GetHighscore(){
        switch (PlayerPrefs.GetInt("Difficulty", 0)){
            case 0 : return PlayerPrefs.GetInt("HighscoreNormal", 0);
            case 1 : return PlayerPrefs.GetInt("HighscoreChallenge", 0);
        }
        return 0;
    }

    public void SetHighscore(int points){
        switch (PlayerPrefs.GetInt("Difficulty", 0)){
            case 0 : PlayerPrefs.SetInt("HighscoreNormal", points); break;
            case 1 : PlayerPrefs.SetInt("HighscoreChallenge", points); break;
        }
    }
}
