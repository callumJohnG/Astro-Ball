using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightUIManager : MonoBehaviour
{
    public static HeightUIManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }


    [SerializeField] private Slider slider;
    [SerializeField] private float sliderMin;
    [SerializeField] private float sliderMax;
    private Transform trackingTransform;

    public void SetPlayer(Transform playerTransform){
        Debug.Log("Player Transform Set");
        this.trackingTransform = playerTransform;
    }

    public void SetMax(float max){
        this.sliderMax = max;

        slider.maxValue = sliderMax;
    }

    public void SetMin(float min){
        this.sliderMin = min;

        slider.minValue = sliderMin;
    }

    private void Update() {
        UpdateSlider();
    }

    private void UpdateSlider(){
        if(trackingTransform == null) return;

        float currentHeight = trackingTransform.position.y;
        currentHeight = Mathf.Clamp(currentHeight, sliderMin, sliderMax);
        slider.value = currentHeight;
    }

}
