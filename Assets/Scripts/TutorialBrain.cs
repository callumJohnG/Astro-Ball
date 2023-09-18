using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBrain : MonoBehaviour
{

    public static TutorialBrain Instance {get; private set;}

    [SerializeField] private Animator tutorialAnimator;
    [SerializeField] private float waitTimeMax = 2f;
    [SerializeField] private float waitTimeBetweenLoops = 1.5f;
    private float currentWaitTime;
    private bool playing = false;
    private const string TUTORIAL_STATE = "Tutorial";
    [SerializeField] private Color onColour;
    [SerializeField] private Color offColour;
    [SerializeField] private List<Image> childImages;
    [SerializeField] private GameObject shootUpText;
    [SerializeField] private GameObject shootUpText2;
    [SerializeField] private GameObject arrows;
    [SerializeField] private float shootTutorialWaitTime;
    private float shootTutorialCurrentTime;
    private bool waitingForShootTutorial;

    private void Awake(){
        Instance = this;
    }


    private void Update(){
        PlayTutorialAnim();

        if(!waitingForShootTutorial)return;

        shootTutorialCurrentTime += Time.deltaTime;
        if(shootTutorialCurrentTime >= shootTutorialWaitTime) {
            waitingForShootTutorial = false;
            StartShootTutorial();
        }
    }

    private void PlayTutorialAnim(){
        if(!playing) return;

        currentWaitTime -= Time.deltaTime;

        if(currentWaitTime <= 0){
            CheckAnimation();
        }
    }

    public void StartTutorial(bool inverse){
        Debug.Log("STARTING TUTORIAL COUINTER");
        //Start the timer
        playing = true;
        currentWaitTime = waitTimeMax;

        Debug.Log(inverse + " Inverse aim");

        if(inverse){
            //Flip upside down
            tutorialAnimator.transform.eulerAngles = new Vector3(0, 0, 180f);
            //transform.Rotate(new Vector3(0, 0, 180f));
        } else {
            tutorialAnimator.transform.eulerAngles = Vector3.zero;
        }
        
        

        if(PlayerPrefs.GetInt(FIRST_EVER_PLAY_TUTORIAL, 0) != 0) return;
        shootUpText.SetActive(true);
        arrows.SetActive(true);
    }

    public void CheckAnimation(){
        if(!IsAnimatorPlaying()){
            Debug.Log("PLAYING NEXT ANIM LOOP");        
            tutorialAnimator.CrossFade(TUTORIAL_STATE, 0, 0);
            currentWaitTime = waitTimeBetweenLoops;
            SetChildColour(onColour);
        }
    }

    public void StopTutorial(){
        Debug.Log("STOPPING ANIM");
        playing = false;
        SetChildColour(offColour);

        shootUpText.SetActive(false);
        arrows.SetActive(false);
        waitingForShootTutorial = true;
    }

    private bool IsAnimatorPlaying(){
        return tutorialAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1 || tutorialAnimator.IsInTransition(0);
    }

    private void SetChildColour(Color colour){
        foreach(Image image in childImages){
            image.color = colour;
        }
    }

    private void StartShootTutorial(){
        if(PlayerPrefs.GetInt(FIRST_EVER_PLAY_TUTORIAL, 0) != 0) return;
        //It's their first ever play, so we should do this thing :)

        GameplayManager.Instance.player.SetTargetTimeScale(0f);
        //continue to play shoot animation
        shootUpText2.SetActive(true);
        arrows.SetActive(true);
    }
    private const string FIRST_EVER_PLAY_TUTORIAL = "FirstEverPlayTutorial";
    public void StopShootTutorial(){
        shootUpText2.SetActive(false);
        arrows.SetActive(false);
        PlayerPrefs.SetInt(FIRST_EVER_PLAY_TUTORIAL, 1);
    }
}
