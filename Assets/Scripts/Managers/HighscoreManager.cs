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
        float highscore = PlayerPrefs.GetInt("Highscore", 0);
        if(points > highscore){
            PlayerPrefs.SetInt("Highscore", points);
        }
        highscoreText.text = "Highscore:\n" + PlayerPrefs.GetInt("Highscore").ToString();

        string pointsString = points + " Points\nBest Combo : " + combo;
        pointsText.text = pointsString;
    }
}
