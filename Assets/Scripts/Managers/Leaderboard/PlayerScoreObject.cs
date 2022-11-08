using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreObject : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Color meColour;

    public void SetText(string name, string score, bool isMe){
        nameText.text = name;
        scoreText.text = score;
        if(isMe){
            nameText.color = meColour;
            scoreText.color = meColour;
        }
    }
}
