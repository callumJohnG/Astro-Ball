using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreObject : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI rankText;

    [SerializeField] private Color meColour;

    public void SetText(string rank, string name, string score, bool isMe){
        rankText.text = rank + ".";
        nameText.text = name;
        scoreText.text = score;
        if(isMe){
            nameText.color = meColour;
            scoreText.color = meColour;
            rankText.color = meColour;
        }
    }
}
