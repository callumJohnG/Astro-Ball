using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxMovementMomentum;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private Camera mainCam;
    
    [SerializeField] private LayerMask groundLayer;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();

        //ConfigureControlListeners();

        targetTimeScale = 1;

        UpdateLaunchCount();

        GetDifficultySettings();

        mainCam = Camera.main;
    }

    //Get all the player based settings from the settings manager
    private void GetDifficultySettings(){
        rb.gravityScale = GameSettingsManager.Instance.defaultGravity;
        launchRechargeDuration = GameSettingsManager.Instance.launchRechargeTime;
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
        aimLine.gameObject.SetActive(false);
        mobileAimLine.gameObject.SetActive(false);

        PointsManager.Instance.EndCombo();

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
        targetTimeScale = slowMoTimeScale;

        aimLine.gameObject.SetActive(true);
        mobileAimLine.gameObject.SetActive(true);

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, aimVector + transform.position);
    }

    private bool firstLaunch = true;

    private void PerformLaunch(){
        if(dead)return;

        if(!launching)return;

        //Check that we have actually aimed
        if(aimVector == Vector3.zero){
            CancelLaunch();
            return;
        }


        //Check if this is our first launch ever
        if(firstLaunch){
            FollowPlayer.Instance.StartRisingTracking(transform);
            firstLaunch = false;
        }
        UpdateLaunchCount(-1);
        rb.velocity = aimVector * launchForce;
        PlayLaunchEffects();

        
        launching = false;
        setAnchor = false;
        targetTimeScale = 1;
        Time.timeScale = targetTimeScale;
        aimLine.gameObject.SetActive(false);
        mobileAimLine.gameObject.SetActive(false);
    }

    public void CancelLaunch(){
        launching = false;
        setAnchor = false;
        targetTimeScale = 1;
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

    private bool setAnchor = false;
    private Vector2 aimAnchor;

    private void SetAimAnchor(){
        aimAnchor = currentMousePosition;
        setAnchor = true;
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
        bool isGroundLayer = (groundLayer == (groundLayer | (1 << collision2D.gameObject.layer)));

        if(collision2D.gameObject.CompareTag("Deadly")){
            Die();
        } else if(isGroundLayer){
            PointsManager.Instance.EndCombo();
        }
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
            fastTrailParticles.Play();
            AudioManager.Instance.ActivateWind();
        } else if(!checkSpeed && overLimit){
            overLimit = false;
            fastTrailParticles.Stop();
            AudioManager.Instance.DeactivateWind();
        }
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

    [HideInInspector] public bool isPaused = false;

    private void FadeTimeScale(){
        if(isPaused)return;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * slowMoFadeSpeed);
        CurrentTimeScale = Time.timeScale;
    }
}