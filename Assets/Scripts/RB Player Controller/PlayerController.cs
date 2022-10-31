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
        FadeTimeScale();

        CalculateAim();
    }

    #region Player Controls
    
    private void ConfigureControlListeners(){
        //Adding new listeners to the movement action
        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;

        //Adding listeners for the jump action
        controls.Player.Jump.started += _ => Jump();
        controls.Player.Jump.started += _ => StartLaunch();
        controls.Player.Jump.canceled += _ => PerformLaunch();

        //Adding listeners for aim variables
        controls.Player.HorizontalAim.performed += ctx => horizontalAimVector = ctx.ReadValue<float>();
        controls.Player.HorizontalAim.canceled += _ => horizontalAimVector = 0;
        controls.Player.VerticalAim.performed += ctx => verticalAimVector = ctx.ReadValue<float>();
        controls.Player.VerticalAim.canceled += _ => verticalAimVector = 0;
    }

    #endregion

    #region Horizontal Movement

    private void CalculateHorizontalVeclocity(){
        //rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        //rb.AddForce(new Vector2(movement * movementSpeed, 0));
        
        
        float newVelocity = (movement * movementSpeed) + rb.velocity.x;
        if(newVelocity > maxMovementMomentum) newVelocity = maxMovementMomentum;
        if(newVelocity < -maxMovementMomentum) newVelocity = -maxMovementMomentum;
        rb.velocity = new Vector2(newVelocity, rb.velocity.y);
        

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
    [SerializeField] private Vector3 previousFrameAimVector = new Vector3();

    private bool launching;

    [SerializeField] private float slowMoTimeScale;
    [SerializeField] private float slowMoFadeSpeed;
    [SerializeField] private float CurrentTimeScale;
    private float targetTimeScale;

    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float launchForce;
    [SerializeField] private float aimSmoothing = 1;


    private void StartLaunch(){
        launching = true;
        targetTimeScale = slowMoTimeScale;

        aimLine.gameObject.SetActive(true);

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, aimVector + transform.position);

        previousFrameAimVector = Vector3.zero;
    }

    private void CalculateAim(){
        if(!launching)return;

        aimVector = new Vector3(horizontalAimVector, verticalAimVector) * launchForce;
        if(aimVector == Vector3.zero){
            //Use mouse
            aimVector = GetMouseAim() * launchForce;
        }

        Vector3 newAimPosition = aimVector + transform.position;


        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, Vector3.Lerp(aimLine.GetPosition(1), newAimPosition, Time.deltaTime * aimSmoothing));
    }

    //Returns the normalised vector that the mouse is pointing in in reference to the player position
    private Vector3 GetMouseAim(){
        //Get the mouse position on screen
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition -= transform.position;
        Debug.Log(mousePosition + ".1");
        mousePosition =  Vector3.Normalize(mousePosition);
        Debug.Log(mousePosition + ".2");
        return mousePosition;
    }

    private void PerformLaunch(){
        launching = false;
        targetTimeScale = 1;
        Time.timeScale = targetTimeScale;

        aimLine.gameObject.SetActive(false);

        rb.velocity = aimVector * launchForce;

        previousFrameAimVector = Vector3.zero;
    }

    private void FadeTimeScale(){
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * slowMoFadeSpeed);
        CurrentTimeScale = Time.timeScale;
    }

    #endregion

    #endregion

}
