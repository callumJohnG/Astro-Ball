using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBrain : MonoBehaviour
{

    public static TutorialBrain Instance {get; private set;}

    [SerializeField] private Animator tutorialAnimator;
    [SerializeField] private float waitTimeMax = 2f;
    [SerializeField] private float waitTimeBetweenLoops = 1.5f;
    private float currentWaitTime;
    private bool playing = false;
    private const string TUTORIAL_STATE = "Tutorial";

    private void Awake(){
        Instance = this;
    }


    private void Update(){
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
    }

    public void CheckAnimation(){
        if(!IsAnimatorPlaying()){
            Debug.Log("PLAYING NEXT ANIM LOOP");
            tutorialAnimator.CrossFade(TUTORIAL_STATE, 0, 0);
            currentWaitTime = waitTimeBetweenLoops;
        }
    }

    public void StopTutorial(){
        Debug.Log("STOPPING ANIM");
        playing = false;
    }

    private bool IsAnimatorPlaying(){
        return tutorialAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1 || tutorialAnimator.IsInTransition(0);
    }
}
