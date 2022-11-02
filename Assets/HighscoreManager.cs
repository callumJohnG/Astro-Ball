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

    public void SetData(int points, int combo){
        string pointsString = points + " Points\nBest Combo : " + combo;
        pointsText.text = pointsString;
    }
}
