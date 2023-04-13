using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{

    [SerializeField] private Image powerupImage;
    [SerializeField] private Slider timerSlider;

    private float startTime;
    private float endTime;

    public void SetData(Sprite sprite, float startTime, float endTime){
        powerupImage.sprite = sprite;

        SetTime(startTime, endTime);
    }

    public void SetTime(float startTime, float endTime){
        this.startTime = startTime;
        this.endTime = endTime;

        timerSlider.minValue = startTime;
        timerSlider.maxValue = endTime;
        timerSlider.value = endTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliderTime();
        //SetPosition();
    }

    private void UpdateSliderTime(){

        timerSlider.value = endTime + startTime - Time.time ;
    }

    private void SetPosition(){
        Vector3 newPos = transform.position;
        newPos.z = 0;
        transform.position = newPos;
    }

    public void KillMe(){
        Destroy(gameObject);
    }
}
