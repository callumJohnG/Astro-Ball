using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public void SetVariables(){
        SetDataToggle();
    }


    [Header("Music")]
    [SerializeField] private float maxMusicVolume;
    [SerializeField] private float minMusicVolume;

    public void SetMusicVolume(float value){
        float increment = (Mathf.Abs(maxMusicVolume - minMusicVolume)) / 25;

        float newValue = minMusicVolume + (value * increment);

        if(value == 0) newValue = -80;
        
        audioMixer.SetFloat("MusicVolume", newValue);
    }

    [Header("SFX")]
    [SerializeField] private float maxSFXVolume;
    [SerializeField] private float minSFXVolume;

    public void SetSFXVolume(float value){
        float increment = (Mathf.Abs(maxSFXVolume - minSFXVolume)) / 20;

        float newValue = minSFXVolume + (value * increment);

        if(value == 0) newValue = -80;
        
        audioMixer.SetFloat("SFXVolume", newValue);
    }

    [Header("Pixilation")]
    [SerializeField] private float maxPixilation;
    [SerializeField] private float minPixilation;

    public void SetPixelation(float value){
        float increment = (Mathf.Abs(maxPixilation - minPixilation)) / 25;

        float newValue = minPixilation + (value * increment);

        shaderMaterialManager.SetPixelation(newValue);
    }

    [Header("Legal")]
    [SerializeField] private Toggle dataToggle;
    private const string CANPOSTSCORE_KEY = "CanPostScore";
    public void SetDataToggle(){
        dataToggle.isOn = GameSettingsManager.Instance.canPostScore;
    }
    
    [SerializeField] private string privacyPolicyURL = "https://sites.google.com/view/astroball-privacy-policy/home";
    public void OpenPrivacyPolicy(){
        Application.OpenURL(privacyPolicyURL);
    }

    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private ShaderMaterialManager shaderMaterialManager;




    public void RESETDATA(){
        PlayerPrefs.DeleteAll();
    }

}
