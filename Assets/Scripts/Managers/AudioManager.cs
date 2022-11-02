using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    [SerializeField] private AudioClip bumper;
    [SerializeField] private AudioSource bumperSource;

    public void PlayBumper(float pitch){
        bumperSource.pitch = pitch;
        bumperSource.PlayOneShot(bumper);
    }

    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip launch;
    [SerializeField] private AudioClip noLaunch;
    [SerializeField] private AudioClip recharge;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip powerDown;

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


    private void PlaySoundEffect(AudioClip audioClip){
        audioSource.PlayOneShot(audioClip);
    }
}
