using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxMovementMomentum;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private PlayerControls controls;

    private float movement;

    private bool grounded;
    [SerializeField] private LayerMask groundLayer;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();

        //Creating new player controlls
        controls = new PlayerControls();

        ConfigureControlListeners();

        targetTimeScale = 1;

        UpdateLaunchCount();

        GetDifficultySettings();
    }

    //Get all the player based settings from the settings manager
    private void GetDifficultySettings(){
        rb.gravityScale = GameSettingsManager.Instance.defaultGravity;
        launchRechargeDuration = GameSettingsManager.Instance.launchRechargeTime;
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateHorizontalVeclocity();

        grounded = IsGrounded();
    }

    private void Update(){
        if(dead){
            return;
        }
        FadeTimeScale();

        CalculateAim();

        CheckLaunchRecharge();

        CheckFastTrailParticles();
    }

    private bool dead = false;
    [SerializeField] private ParticleSystem deathParticles;
    private void Die(){
        dead = true;
        aimLine.gameObject.SetActive(false);

        if(GameSettingsManager.Instance.collectPointsOnDeath){
            PointsManager.Instance.EndCombo();
        }



        //Turn off my collider
        GetComponent<Collider2D>().enabled = false;

        deathParticles.Play();

        //Do death animation here
        AudioManager.Instance.PlayDeath();
        GameplayManager.Instance.GameOver();
    }

    #region Player Controls
    
    private void ConfigureControlListeners(){
        //Adding new listeners to the movement action
        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;

        //Adding listeners for the jump action
        //controls.Player.Jump.started += _ => Jump();
        controls.Player.Jump.started += _ => StartLaunch();
        controls.Player.Jump.canceled += _ => PerformLaunch();

        //Adding listeners for aim variables
        controls.Player.HorizontalAimDigital.performed += ctx => horizontalAimVector = ctx.ReadValue<float>();
        controls.Player.HorizontalAimDigital.canceled += _ => horizontalAimVector = 0;
        controls.Player.VerticalAimDigital.performed += ctx => verticalAimVector = ctx.ReadValue<float>();
        controls.Player.VerticalAimDigital.canceled += _ => verticalAimVector = 0;

        controls.Player.HorizontalAimAnalog.performed += ctx => horizontalAimVector = ctx.ReadValue<float>();
        controls.Player.HorizontalAimAnalog.canceled += ctx => horizontalAimVector = 0;
        controls.Player.VerticalAimAnalog.performed += ctx => verticalAimVector = ctx.ReadValue<float>();
        controls.Player.VerticalAimAnalog.canceled += ctx => verticalAimVector = 0;
    }

    #endregion

    #region Horizontal Movement



    [SerializeField] private float currentVelocity;

    private void CalculateHorizontalVeclocity(){
        //rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        //rb.AddForce(new Vector2(movement * movementSpeed, 0));
        if(GameSettingsManager.Instance.controlScheme == ControlScheme.Mouse){
            movement = GetMouseMovement();
        }
        
        float newVelocity = (movement * movementSpeed) + rb.velocity.x;
        if(newVelocity > maxMovementMomentum) newVelocity = maxMovementMomentum;
        if(newVelocity < -maxMovementMomentum) newVelocity = -maxMovementMomentum;
        rb.velocity = new Vector2(newVelocity, rb.velocity.y);
        
        currentVelocity = rb.velocity.magnitude;
    }

    #endregion

    #region Jumping and Launching

    #region Jumping

    private void Jump(){
        if(grounded){
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }

    [SerializeField] private Transform groundCheck;

    private bool IsGrounded(){
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        return collider != null;
    }

    #endregion

    #region Launching

    private float horizontalAimVector;
    private float verticalAimVector;
    [SerializeField] private Vector3 aimVector = new Vector3();

    private bool launching;

    [SerializeField] private float slowMoTimeScale;
    [SerializeField] private float slowMoFadeSpeed;
    [SerializeField] private float CurrentTimeScale;
    private float targetTimeScale;

    [SerializeField] private LineRenderer aimLine;
    public float launchForce;
    [SerializeField] private float aimSmoothing = 1;

    [SerializeField] private ParticleSystem launchParticles;
    [SerializeField] private ParticleSystem launchTrail;

    public int launchCount;
    public int maxLaucnhCount = 4;


    private void StartLaunch(){
        if(dead)return;

        if(launchCount <= 0){
            AudioManager.Instance.PlayNoLaunch();
            return;
        }

        launching = true;
        targetTimeScale = slowMoTimeScale;

        aimLine.gameObject.SetActive(true);

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, aimVector + transform.position);
    }

    private void CalculateAim(){
        if(!launching)return;

        Vector3 rawAimVector = new Vector3(horizontalAimVector, verticalAimVector);

        if(GameSettingsManager.Instance.controlScheme == ControlScheme.Mouse){
            //Use mouse
            rawAimVector = GetMouseAim();
        }

        if(rawAimVector == Vector3.zero){
            //Use momentum
            rawAimVector = rb.velocity;
        }

        if(GameSettingsManager.Instance.inverseAiming){
            rawAimVector *= -1;
        }

        aimVector = Vector3.Normalize(rawAimVector) * launchForce;

        Vector3 newAimPosition = aimVector + transform.position;


        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, Vector3.Lerp(aimLine.GetPosition(1), newAimPosition, Time.deltaTime * aimSmoothing));
    }

    //Returns the normalised vector that the mouse is pointing in in reference to the player position
    private Vector3 GetMouseAim(){
        //Get the mouse position on screen
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition -= transform.position;
        mousePosition =  Vector3.Normalize(mousePosition);
        return mousePosition;
    }

    private float mouseMoveRange = 50;

    private float GetMouseMovement(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float posVal = Mathf.InverseLerp(transform.position.x, transform.position.x + mouseMoveRange, mousePosition.x);
        float negVal = Mathf.InverseLerp(transform.position.x, transform.position.x - mouseMoveRange, mousePosition.x);
        if(posVal == 0){
            switch(GameSettingsManager.Instance.inverseAiming){
                case true : return negVal;
                case false : return -negVal;
            }
        } else {
            switch(GameSettingsManager.Instance.inverseAiming){
                case true : return -posVal;
                case false : return posVal;
            }
        }
    }

    private void PerformLaunch(){
        if(dead)return;

        if(!launching)return;

        UpdateLaunchCount(-1);
        launching = false;
        targetTimeScale = 1;
        Time.timeScale = targetTimeScale;

        aimLine.gameObject.SetActive(false);

        rb.velocity = aimVector * launchForce;

        PlayLaunchEffects();
    }

    private void FadeTimeScale(){
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * slowMoFadeSpeed);
        CurrentTimeScale = Time.timeScale;
    }

    public void UpdateLaunchCount(int count){
        launchCount += count;
        if(launchCount <= 0)launchCount = 0;
        if(launchCount >= maxLaucnhCount) launchCount = maxLaucnhCount;

        LaunchUI.Instance.SetText(launchCount.ToString());
    }

    public void UpdateLaunchCount(){
        UpdateLaunchCount(99999);
    }

    [SerializeField] private float launchRechargeDuration = 4;
    private float launchRechargeStartTime;
    private bool recharging = false;

    private void CheckLaunchRecharge(){
        if(launchCount >= maxLaucnhCount && recharging){
            recharging = false;
            LaunchUI.Instance.StopRecharge();
            return;
        }

        if(launchCount < maxLaucnhCount && !recharging){
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
    [SerializeField] private bool overSpeedLimit;
    private void CheckFastTrailParticles(){
        overSpeedLimit = rb.velocity.magnitude >= fastTrailSpeedLimit;
        if(overSpeedLimit){
            fastTrailParticles.Play();
        } else{
            fastTrailParticles.Stop();
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

}