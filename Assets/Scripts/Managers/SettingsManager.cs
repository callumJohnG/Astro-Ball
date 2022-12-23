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
        
        audioMixer.SetFloat("MusicVolume", newValue);
    }

    [Header("SFX")]
    [SerializeField] private float maxSFXVolume;
    [SerializeField] private float minSFXVolume;

    public void SetSFXVolume(float value){
        float increment = (Mathf.Abs(maxSFXVolume - minSFXVolume)) / 10;

        float newValue = minSFXVolume + (value * increment);
        
        audioMixer.SetFloat("SFXVolume", newValue);
    }

    [Header("Pixilation")]
    [SerializeField] private float maxPixilation;
    [SerializeField] private float minPixilation;

    public void SetPixilation(float value){

    }


    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private ShaderMaterialManager shaderMaterialManager;
}
