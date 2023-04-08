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

        GameplayManager.Instance.OnGameStarted += GameplayManager_OnGameStarted;
        GameplayManager.Instance.OnGameEnded += GameplayManager_OnGameEnded;

        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    void Update(){
        //CheckSong();
        //FadeWind();

        FadeMusic();
    }

    #region SFX

    #region Event Emitters

    //private FMOD.Studio.EventInstance bumperInstance;
    public EventReference bumperEvent;

    [SerializeField] private StudioEventEmitter bumperEmitter;

    public void PlayBumper(float pitch){
        //bumperInstance = FMODUnity.RuntimeManager.CreateInstance(bumperEvent);
        RuntimeManager.PlayOneShot(bumperEvent, "bumper-combo", pitch, transform.position);
        //bumperEmitter.SetParameter("bumper-combo", pitch);
        //bumperEmitter.Play();
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

    [SerializeField] private StudioEventEmitter spendCoinsEmitter;
    public void PlaySpendCoins(){
        spendCoinsEmitter.Play();
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
    private FMOD.Studio.EventInstance musicInstance;
    public EventReference musicEvent;

    [SerializeField] private float musicStateValue = 0f;
    [SerializeField] private float menuStateValue = 0f;
    [SerializeField] private float musicStateGoal = 0f;
    [SerializeField] private float menuStateGoal = 0f;

    private const float MUSIC_ON = 1f;
    private const float MUSIC_OFF = 0f;

    private const string MUSIC_STATE = "music-state";
    private const string MENU_STATE = "menu-state";


    [SerializeField] private float musicFadeSpeed = 1f;
    [SerializeField] private float menuFadeSpeed = 5f;

    private void FadeMusic(){
        float currentMusicValue = Mathf.Lerp(musicStateValue, musicStateGoal, Time.deltaTime * musicFadeSpeed);
        musicStateValue = currentMusicValue;
        musicInstance.setParameterByName(MUSIC_STATE, musicStateValue);

        float currentMenuValue = Mathf.Lerp(menuStateValue, menuStateGoal, Time.deltaTime * menuFadeSpeed);
        menuStateValue = currentMenuValue;
        musicInstance.setParameterByName(MENU_STATE, menuStateValue);
    }

    private void GameplayManager_OnGameStarted(object sender, System.EventArgs e){
        menuStateGoal = MUSIC_ON;
    }

    private void GameplayManager_OnGameEnded(object sender, System.EventArgs e){
        menuStateGoal = MUSIC_OFF;
    }

    public void SetMusicState(bool playerIsHolding){
        Debug.Log("CHANGING MUSIC STATE TO " + playerIsHolding);
        //If the player is holding down, state is 1, else 0
        if(playerIsHolding){
            musicStateGoal = MUSIC_ON;
        } else {
            musicStateGoal = MUSIC_OFF;
        }
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
