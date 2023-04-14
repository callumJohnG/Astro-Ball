using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerVisual : MonoBehaviour
{
    
    [SerializeField] private Image[] timerImages;


    private void Update(){
        float comboPercentage = PointsManager.Instance.GetComboTime();

        foreach(Image image in timerImages){
            image.fillAmount = comboPercentage;
        }
    }

}
