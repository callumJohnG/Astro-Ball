using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private CharacterController2D controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        ProcessInput();
    }


    private bool leftPressed;
    [SerializeField] private KeyCode leftKey = KeyCode.W;
    
    private bool rightPressed;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    
    private bool jumpPressed;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private float horizontalMovement;

    private void GatherInput(){
        horizontalMovement = Input.GetAxis("Horizontal");

        leftPressed = Input.GetKey(leftKey);
        rightPressed = Input.GetKey(rightKey);
        jumpPressed = Input.GetKeyDown(jumpKey);
    }

    private void ProcessInput(){
        controller.Move(horizontalMovement, false, jumpPressed);
    }
}
