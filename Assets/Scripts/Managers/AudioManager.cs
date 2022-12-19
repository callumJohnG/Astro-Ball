using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
        audioSource = GetComponent<AudioSource>();

        DeactivateWind();
    }

    void Update(){
        CheckSong();
        FadeWind();
    }

    #region SFX

    #region Special

    [SerializeField] private AudioClip bumper;
    [SerializeField] private AudioSource bumperSource;
    [SerializeField] private AudioSource musicSource;

    public void PlayBumper(float pitch){
        bumperSource.pitch = pitch;
        bumperSource.PlayOneShot(bumper);
    }





    private float timeOfLastZoom;
    [SerializeField] private float zoomTimeWindow = 0.25f;
    [SerializeField] private float zoomPitchIncrease = 0.2f;
    [SerializeField] private AudioClip zoom;
    [SerializeField] private AudioSource zoomSource;
    public void PlayZoom(){
        if(Time.time <= timeOfLastZoom + zoomTimeWindow){
            zoomSource.pitch = zoomSource.pitch + zoomPitchIncrease;
        } else {
            zoomSource.pitch = 1;
        }
        timeOfLastZoom = Time.time;
        zoomSource.PlayOneShot(zoom);
    }

    #endregion

    #region General

    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip launch;
    [SerializeField] private AudioClip noLaunch;
    [SerializeField] private AudioClip recharge;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip powerDown;
    [SerializeField] private AudioClip comboOver;
    [SerializeField] private AudioClip hitWall;
    [SerializeField] private List<AudioClip> buttonClicks;

    public void PlayCoin(){
        PlaySoundEffect(coin);
    }

    public void PlayDeath(){
        PlaySoundEffect(death);
    }

    public void PlayLaunch(){
        PlaySoundEffect(launch);
    }

    public void PlayNoLaunch(){
        PlaySoundEffect(noLaunch);
    }

    public void PlayRecharge(){
        PlaySoundEffect(recharge);
    }

    public void PlayPowerUp(){
        PlaySoundEffect(powerUp);
    }

    public void PlayPowerDown(){
        PlaySoundEffect(powerDown);
    }
    
    public void PlayComboOver(){
        PlaySoundEffect(comboOver);
    }

    public void PlayHitWall(){
        PlaySoundEffect(hitWall);
    }

    public void PlayButtonClick(){
        PlaySoundEffect(buttonClicks[Random.Range(0, buttonClicks.Count)]);
    }

    private void PlaySoundEffect(AudioClip audioClip){
        audioSource.PlayOneShot(audioClip);
    }

    #endregion

    #region Wind

    private float windTargetVolume;
    [SerializeField] private float windFadeSpeed = 10;
    [SerializeField] private float windTopVolume = 0;
    [SerializeField] private float windBottomVolume = -80;

    public void ActivateWind(){
        windTargetVolume = windTopVolume;
    }

    public void DeactivateWind(){
        windTargetVolume = windBottomVolume;
    }

    private void FadeWind(){
        mainMixer.GetFloat("WindVolume", out float currentVolumne);
        float newVolume = Mathf.Lerp(currentVolumne, windTargetVolume, windFadeSpeed);
        mainMixer.SetFloat("WindVolume", newVolume);
    }

    #endregion

    #endregion

    #region Music

    [SerializeField] private List<AudioClip> musicList;
    private int songIndex = 0;

    private void CheckSong(){
        if(!musicSource.isPlaying){
            PlayNextSong();
        }
    }

    public void PlayNextSong(){
        musicSource.clip = musicList[songIndex];
        musicSource.Play();
        songIndex++;
        if(songIndex >= musicList.Count)songIndex = 0;
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
