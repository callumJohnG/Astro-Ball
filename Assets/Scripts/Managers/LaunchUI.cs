using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LaunchUI : MonoBehaviour
{

    public static LaunchUI Instance {get; private set;}
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRechargeSlider();
    }

    [SerializeField] private TextMeshProUGUI launchCountText;

    public void SetText(string text){
        launchCountText.text = text;
    }


    [SerializeField] private Slider timerSlider;
    private float startTime;
    private float endTime;
    private bool timerActive = false;
    public void SetRechargeTime(float startTime, float endTime){
        timerActive = true;
        this.startTime = startTime;
        this.endTime = endTime;
        timerSlider.minValue = startTime;
        timerSlider.maxValue = endTime;
    }

    private void UpdateRechargeSlider(){
        if(!timerActive)return;

        timerSlider.value = Time.time;
    }

    public void StopRecharge(){
        timerActive = false;
        timerSlider.minValue = 0;
        timerSlider.maxValue = 1;
        timerSlider.value = 1;
    }
}
