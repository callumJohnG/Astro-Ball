using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onPlay;
    [SerializeField] private string pauseSnapshotName = "snapshot:/pause";
    FMOD.Studio.EventInstance pauseSnapshot;
    
    private void Start(){
        pauseSnapshot = FMODUnity.RuntimeManager.CreateInstance(pauseSnapshotName);
    }

    public void Pause(){
        onPause.Invoke();
        GameplayManager.Instance.player.CancelLaunch();
        GameplayManager.Instance.player.isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;

        pauseSnapshot.start();
    }

    public void Play(){
        onPlay.Invoke();
        
        GameplayManager.Instance.player.isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;

        pauseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.SetMusicState(false);
    }
}
