using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onPlay;
    

    public void Pause(){
        onPause.Invoke();
        GameplayManager.Instance.player.CancelLaunch();
        GameplayManager.Instance.player.isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Play(){
        onPlay.Invoke();
        
        GameplayManager.Instance.player.isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
