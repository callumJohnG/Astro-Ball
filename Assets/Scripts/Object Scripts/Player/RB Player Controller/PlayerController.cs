using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool DevEnv;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxMovementMomentum;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private Camera mainCam;
    
    [SerializeField] private LayerMask groundLayer;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();

        //ConfigureControlListeners();

        SetTargetTimeScale(1f);

        UpdateLaunchCount();

        GetDifficultySettings();

        mainCam = Camera.main;
    }

    //Get all the player based settings from the settings manager
    private void GetDifficultySettings(){
        rb.gravityScale = GameSettingsManager.Instance.defaultGravity;
        launchRechargeDuration = GameSettingsManager.Instance.launchRechargeTime;
        if(DevEnv){
            rb.gravityScale = 4;
            launchRechargeDuration = 2;   
        }
    }

    private void Update(){
        if(dead){
            return;
        }
        FadeTimeScale();

        CalculateAimVector();

        UpdateAimVisuals();

        CheckLaunchRecharge();

        CheckFastTrailParticles();
    }

    private bool dead = false;
    [SerializeField] private ParticleSystem deathParticles;
    private void Die(){
        dead = true;
        Time.timeScale = 1;
        aimLine.gameObject.SetActive(false);
        mobileAimLine.gameObject.SetActive(false);

        SendImpulse();

        PointsManager.Instance.EndCombo(false);


        AudioManager.Instance.SetMusicState(false);

        //Turn off my collider
        GetComponent<Collider2D>().enabled = false;

        deathParticles.Play();

        //Do death animation here
        AudioManager.Instance.DeactivateWind();
        AudioManager.Instance.PlayDeath();
        GameplayManager.Instance.GameOver();
    }

    #region Player Input Events
    
    private bool queueLaunch;
    
    public void OnLaunch(InputAction.CallbackContext value){
        if(isPaused)return;

        if(value.started){
            //StartLaunch();
            queueLaunch = true;
        } else if(value.canceled){
            PerformLaunch();
        }
    }

    public void OnAim(InputAction.CallbackContext value){
        if(isPaused)return;

        Vector2 aimValue = value.ReadValue<Vector2>();
        currentMousePosition = aimValue;

        if(queueLaunch){
            StartLaunch();
            queueLaunch = false;
        }
    }

    #endregion

    #region Launching

    private Vector3 aimVector = new Vector3();
    private bool launching;
    private Vector2 currentMousePosition;

    [SerializeField] private float slowMoTimeScale;
    [SerializeField] private float slowMoFadeSpeed;
    [SerializeField] private float hitStopPeriod;
    [SerializeField] private float CurrentTimeScale;
    private float targetTimeScale;

    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private LineRenderer mobileAimLine;
    private Vector3 LINEOFFSET = new Vector3(0, 0, 10);
    public float launchForce;
    [SerializeField] private float aimSmoothing = 1;

    [SerializeField] private ParticleSystem launchParticles;
    [SerializeField] private ParticleSystem launchTrail;

    public int launchCount;
    public int maxLaunchCount = 4;


    private void StartLaunch(){
        if(dead)return;

        if(launchCount <= 0){
            AudioManager.Instance.PlayNoLaunch();
            return;
        }

        SetAimAnchor();

        launching = true;
        SetTargetTimeScale(slowMoTimeScale);

        aimLine.gameObject.SetActive(true);
        mobileAimLine.gameObject.SetActive(true);

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, aimVector + transform.position);

        AudioManager.Instance.SetMusicState(true);
    }

    private bool firstLaunch = true;
    private bool secondLaucnh = true;

    private void PerformLaunch(){
        if(dead)return;

        if(!launching)return;

        //Check that we have actually aimed
        if(aimVector == Vector3.zero){
            CancelLaunch();
            return;
        }


        //Check if this is our first launch ever
        if(!DevEnv && firstLaunch){
            FollowPlayer.Instance.StartRisingTracking(transform);
            TutorialBrain.Instance.StopTutorial();
            firstLaunch = false;
        } else if(secondLaucnh){
            TutorialBrain.Instance.StopShootTutorial();
            secondLaucnh = false;
        }
        UpdateLaunchCount(-1);
        rb.velocity = aimVector * launchForce;
        PlayLaunchEffects();

        AudioManager.Instance.SetMusicState(false);

        //Reset the combo audio queue
        PointsManager.Instance.ResetComboPitchCounter();

        
        launching = false;
        SetTargetTimeScale(1f);
        Time.timeScale = targetTimeScale;
        aimLine.gameObject.SetActive(false);
        mobileAimLine.gameObject.SetActive(false);
    }

    public void CancelLaunch(){
        launching = false;
        SetTargetTimeScale(1f);
        aimLine.gameObject.SetActive(false);
        mobileAimLine.gameObject.SetActive(false);
    }

    

    public void UpdateLaunchCount(int count){
        launchCount += count;
        if(launchCount <= 0)launchCount = 0;
        if(launchCount >= maxLaunchCount) launchCount = maxLaunchCount;

        LaunchUI.Instance.SetText(launchCount.ToString());
    }

    public void UpdateLaunchCount(){
        UpdateLaunchCount(99999);
    }

    public void SetTargetTimeScale(float timeScale){
        targetTimeScale = timeScale;
    }

    [SerializeField] private float launchRechargeDuration = 4;
    private float launchRechargeStartTime;
    private bool recharging = false;

    private void CheckLaunchRecharge(){
        if(launchCount >= maxLaunchCount && recharging){
            recharging = false;
            LaunchUI.Instance.StopRecharge();
            return;
        }

        if(launchCount < maxLaunchCount && !recharging){
            recharging = true;
            launchRechargeStartTime = Time.time;
            //Tell the UI about it here
            LaunchUI.Instance.SetRechargeTime(Time.time, launchRechargeDuration + Time.time);

            return;
        }

        if(!recharging)return;

        //We are currently recharging
        if(Time.time > launchRechargeStartTime + launchRechargeDuration){
            //We have recharged a launch
            UpdateLaunchCount(1);
            recharging = false;
            //Tell the UI about it
            LaunchUI.Instance.StopRecharge();

            AudioManager.Instance.PlayRecharge();
        }
    }    
    

    #endregion

    #region Aiming

    private Vector2 aimAnchor;

    private void SetAimAnchor(){
        aimAnchor = currentMousePosition;
    }

    private void CalculateAimVector(){
        if(!launching)return;

        Vector2 destination = mainCam.ScreenToWorldPoint(currentMousePosition);
        Vector2 anchor = mainCam.ScreenToWorldPoint(aimAnchor);
        aimVector = destination - anchor;
        if(GameSettingsManager.Instance.flipAim ^ GameSettingsManager.Instance.inverseAiming){
            aimVector = anchor - destination;
        }
        aimVector = Vector3.Normalize(aimVector) * launchForce;
    }

    private void UpdateAimVisuals(){
        if(!launching)return;
    
        //Get the position on screen that we are aiming at
        Vector3 newAimPosition = aimVector + transform.position;

        //Draw the line visuals to match our current aim
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, Vector3.Lerp(aimLine.GetPosition(1), newAimPosition, Time.deltaTime * aimSmoothing));
    
        mobileAimLine.SetPosition(0, mainCam.ScreenToWorldPoint(aimAnchor) + LINEOFFSET);
        mobileAimLine.SetPosition(1, mainCam.ScreenToWorldPoint(currentMousePosition) + LINEOFFSET);
    }


    
    #endregion

    #region Boosting

    public void Boost(float force){
        //Get the current direction of movement, and set the velocity
        Vector3 newVelocity = Vector3.Normalize(rb.velocity);
        newVelocity *= force;
        //if(newVelocity.magnitude < rb.velocity.magnitude) return;
        //rb.velocity = newVelocity;
        rb.AddForce(newVelocity);
    }

    #endregion

    #region Collision

    private void OnCollisionEnter2D(Collision2D collision2D){
        if(dead)return;
        bool isGroundLayer = (groundLayer == (groundLayer | (1 << collision2D.gameObject.layer)));

        if(isGroundLayer){
            //PointsManager.Instance.EndCombo();
        } else {
            if(collision2D.gameObject.CompareTag("Deadly")){
                Die();
                return;
            }

            StartCoroutine(HitStop());//Pause game for a millisecond or 20
        }  
    }

    private IEnumerator HitStop(){
        //Track current timescale and set it to 0
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isHitStop = true;

        //Pause game
        yield return new WaitForSecondsRealtime(hitStopPeriod);

        //Resume
        Time.timeScale = currentTimeScale;
        isHitStop = false;
    }

    #endregion

    #region Visuals

    [SerializeField] private ParticleSystem fastTrailParticles;
    [SerializeField] private float fastTrailSpeedLimit;
    [SerializeField] private bool checkSpeed;
    private bool overLimit = false;

    private void CheckFastTrailParticles(){
        checkSpeed = rb.velocity.magnitude >= fastTrailSpeedLimit;
        if(checkSpeed && !overLimit){
            overLimit = true;
            ActivateSpeedEffects();
        } else if(!checkSpeed && overLimit){
            overLimit = false;
            DeactivateSpeedEffects();
        }
    }

    private void ActivateSpeedEffects(){
        fastTrailParticles.Play();
        AudioManager.Instance.ActivateWind();
    }

    private void DeactivateSpeedEffects(){
        fastTrailParticles.Stop();
        AudioManager.Instance.DeactivateWind();
    }

    private void PlayLaunchEffects(){
        /*
        Vector3 rotateVector = Vector3.Normalize(new Vector3(aimVector.x, aimVector.y));
        Debug.Log(rotateVector);
        float rotateAngle = Vector3.Angle(rotateVector, Vector3.up);

        //Vector3 targetRotation = new Vector3(0, 0, rotateAngle);
        //Quaternion newRotation = Quaternion.LookRotation(targetRotation);
        //launchParticles.transform.rotation = newRotation;
        Quaternion newAngle = new Quaternion(0, 0, rotateAngle, 0);
        launchParticles.transform.rotation = newAngle;
        Debug.Log("New rotation : " + launchParticles.transform.rotation);
        */
        //NONE OF THIS WORKED LOL

        launchParticles.Play();
        launchTrail.Play();
        AudioManager.Instance.PlayLaunch();
    }

    #endregion

    #region CameraShake

    [SerializeField] private CinemachineImpulseSource impulseSource;
    private void SendImpulse(){
        impulseSource.GenerateImpulse();
    }

    #endregion



    [HideInInspector] public bool isPaused = false;
    private bool isHitStop;

    private void FadeTimeScale(){
        if(dead){
            return;
        }

        if(isPaused || isHitStop)return;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * slowMoFadeSpeed);
        CurrentTimeScale = Time.timeScale;
    }
}