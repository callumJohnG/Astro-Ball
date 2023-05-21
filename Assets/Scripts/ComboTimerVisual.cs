using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerVisual : MonoBehaviour
{
    
    [SerializeField] private Image[] timerImages;
    private bool visible = false;

    private void Update(){
        if(!visible){
            return;
        }

        float comboPercentage = PointsManager.Instance.GetComboTime();

        foreach(Image image in timerImages){
            image.fillAmount = comboPercentage;
        }
    }

    public void Show(){
        visible = true;
    }

    public void Hide(){
        visible = false;
        foreach(Image image in timerImages){
            image.fillAmount = 0f;
        }
    }

}
