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
        GetPixelation();
        GetMusic();
        GetSFX();
    }

    private void Update(){
        
    }


    [Header("Music")]
    [SerializeField] private float maxMusicVolume;
    [SerializeField] private float minMusicVolume;
    [SerializeField] private Slider musicSlider;
    private const string MUSIC_KEY = "Music_Key";

    public void SetMusicVolume(float value){
        //float increment = (Mathf.Abs(maxMusicVolume - minMusicVolume)) / 25;
        //float newValue = minMusicVolume + (value * increment);
        //if(value == 0) newValue = -80;

        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        
        //audioMixer.SetFloat("MusicVolume", newValue);
    }
    private void GetMusic(){
        float value = PlayerPrefs.GetFloat(MUSIC_KEY, maxMusicVolume);

        musicSlider.minValue = minMusicVolume;
        musicSlider.maxValue = maxMusicVolume;

        musicSlider.value = value;
        //SetMusicVolume(value);
    }

    [Header("SFX")]
    [SerializeField] private float maxSFXVolume;
    [SerializeField] private float minSFXVolume;
    [SerializeField] private Slider SFXSlider;
    private const string SFX_KEY = "SFX_Key";

    public void SetSFXVolume(float value){
        //float increment = (Mathf.Abs(maxSFXVolume - minSFXVolume)) / 20;
        //float newValue = minSFXVolume + (value * increment);
        //if(value == 0) newValue = -80;
        
        PlayerPrefs.SetFloat(SFX_KEY, value);
        
        //audioMixer.SetFloat("SFXVolume", newValue);
    }
    private void GetSFX(){
        float value = PlayerPrefs.GetFloat(SFX_KEY, maxSFXVolume);

        SFXSlider.minValue = minSFXVolume;
        SFXSlider.maxValue = maxSFXVolume;

        SFXSlider.value = value;
        //SetSFXVolume(value);
    }

    [Header("Pixelation")]
    [SerializeField] private float smallPixelSize;
    [SerializeField] private float midPixelSize;
    [SerializeField] private float bigPixelSize;
    [SerializeField] private Slider pixelSlider;
    private const string PIXELATION_KEY = "Pixelation_Key";
    [SerializeField] private int defaultPixelValue = 1;

    public void SetPixelation(float valuef){
        int value = Mathf.RoundToInt(valuef);
        switch (value){
            case 0:
                //Turn off pixelation
                shaderMaterialManager.SetPixelation(0f);
                break;
            case 1:
                //Turn off pixelation
                shaderMaterialManager.SetPixelation(smallPixelSize);
                break;
            case 2:
                //Turn off pixelation
                shaderMaterialManager.SetPixelation(midPixelSize);
                break;
            case 3:
                //Turn off pixelation
                shaderMaterialManager.SetPixelation(bigPixelSize);
                break;
        }

        PlayerPrefs.SetInt(PIXELATION_KEY, value);

    }

    private void GetPixelation(){
        int value = PlayerPrefs.GetInt(PIXELATION_KEY, defaultPixelValue);
        pixelSlider.value = value;
        SetPixelation(value);
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
