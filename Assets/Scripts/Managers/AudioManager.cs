using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FMODUnity;


public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    private void Awake(){
        Instance = this;
    }

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        DeactivateWind();
    }

    void Update(){
        //CheckSong();
        //FadeWind();
    }

    #region SFX

    #region Event Emitters

    [SerializeField] private StudioEventEmitter bumperEmitter;

    public void PlayBumper(float pitch){
        bumperEmitter.Play();
    }





    private float timeOfLastZoom;
    [SerializeField] private float zoomTimeWindow = 0.25f;
    [SerializeField] private float zoomPitchIncrease = 0.2f;
    [SerializeField] private StudioEventEmitter zoomEmitter;
    public void PlayZoom(){
        /*if(Time.time <= timeOfLastZoom + zoomTimeWindow){
            zoomSource.pitch = zoomSource.pitch + zoomPitchIncrease;
        } else {
            zoomSource.pitch = 1;
        }
        timeOfLastZoom = Time.time;*/
        //zoomSource.PlayOneShot(zoom);
        zoomEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter coinEmitter;

    public void PlayCoin(){
        //PlaySoundEffect(coin);
        coinEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter deathEmitter;
    public void PlayDeath(){
        //PlaySoundEffect(death);
        deathEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter launchEmitter;

    public void PlayLaunch(){
        //PlaySoundEffect(launch);
        launchEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter noLaunchEmitter;
    public void PlayNoLaunch(){
        //PlaySoundEffect(noLaunch);
        noLaunchEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter rechargeEmitter;
    public void PlayRecharge(){
        //PlaySoundEffect(recharge);
        rechargeEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter powerUpEmitter;
    public void PlayPowerUp(){
        //PlaySoundEffect(powerUp);
        powerUpEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter powerDownEmitter;
    public void PlayPowerDown(){
        //PlaySoundEffect(powerDown);
        powerDownEmitter.Play();
    }
    
    [SerializeField] private StudioEventEmitter comboOverEmitter;
    public void PlayComboOver(){
        //PlaySoundEffect(comboOver);
        comboOverEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter buttonClickEmitter;
    public void PlayButtonClick(){
        //PlaySoundEffect(buttonClicks[Random.Range(0, buttonClicks.Count)]);
        buttonClickEmitter.Play();
    }

    [SerializeField] private StudioEventEmitter spendCoins;
    public void PlaySpendCoins(){
        spendCoins.Play();
    }

    private void PlaySoundEffect(AudioClip audioClip){
        //audioSource.PlayOneShot(audioClip);
    }

    #endregion

    #region Wind

    private float windTargetVolume;
    [SerializeField] private float windFadeSpeed = 10;
    [SerializeField] private float windTopVolume = 0;
    [SerializeField] private float windBottomVolume = -80;
    [SerializeField] private StudioEventEmitter windEmitter;

    public void ActivateWind(){
        //windTargetVolume = windTopVolume;
        windEmitter.Play();
    }

    public void DeactivateWind(){
        //windTargetVolume = windBottomVolume;
        windEmitter.Stop();
    }

    /*private void FadeWind(){
        mainMixer.GetFloat("WindVolume", out float currentVolumne);
        float newVolume = Mathf.Lerp(currentVolumne, windTargetVolume, windFadeSpeed);
        mainMixer.SetFloat("WindVolume", newVolume);
    }*/

    #endregion

    #endregion

    #region Music
    /*
    //[SerializeField] private List<AudioClip> musicList;
    //private int songIndex = 0;

    private void CheckSong(){
        //if(!musicSource.isPlaying){
        //    PlayNextSong();
        //}
    }

    public void PlayNextSong(){
        //musicSource.clip = musicList[songIndex];
        //musicSource.Play();
        songIndex++;
        if(songIndex >= musicList.Count)songIndex = 0;
    }*/

    public void SetMainMenuMusic(){

    }

    public void SetPlayMusic(){

    }

    #endregion

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private float sfxDefaultValue;
    public void MuteSFX(bool isMuted){
        float newVolume = sfxDefaultValue;
        if(isMuted){
            newVolume = -100f;
        }

        mainMixer.SetFloat("SFXVolume", newVolume);
    }
    [SerializeField] private float musicDefaultValue;
    public void MuteMusic(bool isMuted){
        float newVolume = musicDefaultValue;
        if(isMuted){
            newVolume = -100f;
        }

        mainMixer.SetFloat("MusicVolume", newVolume);
    }
}
