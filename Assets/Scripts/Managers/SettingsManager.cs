using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    [Header("Music")]
    [SerializeField] private float maxMusicVolume;
    [SerializeField] private float minMusicVolume;

    public void SetMusicVolume(float value){
        float increment = (Mathf.Abs(maxMusicVolume - minMusicVolume)) / 10;

        float newValue = minMusicVolume + (value * increment);

        if(value == 0) newValue = -80;
        
        audioMixer.SetFloat("MusicVolume", newValue);
    }

    [Header("SFX")]
    [SerializeField] private float maxSFXVolume;
    [SerializeField] private float minSFXVolume;

    public void SetSFXVolume(float value){
        float increment = (Mathf.Abs(maxSFXVolume - minSFXVolume)) / 10;

        float newValue = minSFXVolume + (value * increment);

        if(value == 0) newValue = -80;
        
        audioMixer.SetFloat("SFXVolume", newValue);
    }

    [Header("Pixilation")]
    [SerializeField] private float maxPixilation;
    [SerializeField] private float minPixilation;

    public void SetPixelation(float value){
        float increment = (Mathf.Abs(maxPixilation - minPixilation)) / 10;

        float newValue = minPixilation + (value * increment);

        shaderMaterialManager.SetPixelation(newValue);
    }


    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private ShaderMaterialManager shaderMaterialManager;
}
