using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{

    public static PointsManager Instance {get;private set;}

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
    }


    private void Start(){
        UpdatePointsUI();
    }

    //I would like to make a combo system where you gain multipliers for many points gained in short succession
    private int points;
    [SerializeField] private TextMeshProUGUI pointsText;

    public void GainPoints(int quantity){
        points += quantity;
        UpdatePointsUI();
    }

    private void UpdatePointsUI(){
        pointsText.text = points.ToString();
    }

}
