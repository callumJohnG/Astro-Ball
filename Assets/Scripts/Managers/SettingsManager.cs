using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingsManager : MonoBehaviour
{

    public void SetVariables(){
        SetDataToggle();
    }

    private void Start() {
        SetUpAimSetting();
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

    [Header("Controls")]
    [SerializeField] private bool inverseAiming = false;
    [SerializeField] private Toggle inverseAimToggle;
    private void SetUpAimSetting(){
        inverseAiming = GameSettingsManager.Instance.inverseAiming;
        inverseAimToggle.isOn = inverseAiming;
    }

    public void SetInverseAim(bool inverseAiming){
        this.inverseAiming = inverseAiming;
        GameSettingsManager.Instance.SetInverseAim(inverseAiming);
    }

    [Header("Legal")]
    [SerializeField] private Toggle dataToggle;
    private const string CANPOSTSCORE_KEY = "CanPostScore";
    public void SetDataToggle(){
        dataToggle.isOn = GameSettingsManager.Instance.canPostScore;
    }
    
    [SerializeField] private string privacyPolicyURL = "https://sites.google.com/view/astroball-privacy-policy/home";
    [SerializeField] private string termsOfServiceURL = "https://sites.google.com/view/astroballtermsofservice/home";
    [SerializeField] private string endUserAgreementURL = "https://sites.google.com/view/astroball-end-user-license-agr/home";
    public void OpenPrivacyPolicy(){
        Application.OpenURL(privacyPolicyURL);
    }

    public void OpenTermsOfService(){
        Application.OpenURL(termsOfServiceURL);
    }

    public void OpenEULA(){
        Application.OpenURL(endUserAgreementURL);
    }

    [Header("Credits")]
    [SerializeField] private string callumItchLink = "https://callumg.itch.io/";
    public void OpenCallumLink(){
        Application.OpenURL(callumItchLink);
    }

    [SerializeField] private string brunoInstagramLink = "https://www.instagram.com/brunex.audio/";
    public void OpenBrunoLink(){
        Application.OpenURL(brunoInstagramLink);
    }


    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private ShaderMaterialManager shaderMaterialManager;



    [Header("DevEnv")]
    [SerializeField] private string developerSceneName;
    public void OpenDeveloperEnv(){
        SceneManager.LoadScene(developerSceneName);
    }


    public void RESETDATA(){
        PlayerPrefs.DeleteAll();
    }

}
