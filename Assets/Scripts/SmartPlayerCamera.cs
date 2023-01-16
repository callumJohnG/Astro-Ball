using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SmartPlayerCamera : MonoBehaviour
{

    public static SmartPlayerCamera Instance {get; private set;}

    private void Awake(){
        Instance = this;
        playerCam = GetComponent<CinemachineVirtualCamera>();
        playerCam.m_Lens.OrthographicSize = minZoom;
    }

    [SerializeField] private bool DevEnv;

    private CinemachineVirtualCamera playerCam;
    [SerializeField] private Rigidbody2D player;


    // Update is called once per frame
    void Update()
    {  
        if(!DevEnv && !GameplayManager.Instance.gameIsActive){
            playerCam.Follow = null;
            CinemachineBasicMultiChannelPerlin perlin = playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0;
            return;
        }

        CalculateCameraZoom();
        CalculateCameraShake();
    }

    [SerializeField] private float zoomGradient = 0.1f;
    [SerializeField] private float minZoom = 10;
    [SerializeField] private float maxZoom = 50;

    private void CalculateCameraZoom(){

        float playerVelocity = player.velocity.magnitude;

        //x = 0, y = 10
        //y = 50 is the max
        //increment in steps of 0.25 per X
        //y = 0.25X + 10
        float targetOrtho = (playerVelocity * zoomGradient) + minZoom;
        if(targetOrtho > maxZoom)targetOrtho = maxZoom;

        playerCam.m_Lens.OrthographicSize = Mathf.Lerp(playerCam.m_Lens.OrthographicSize, targetOrtho, Time.deltaTime);
    }

    [SerializeField] private float shakeGradient = 0.1f;
    [SerializeField] private float minShake = 0;
    [SerializeField] private float maxShake = 5;

    private void CalculateCameraShake(){
        //Based on the player speed, the camera will shake
        float speed = player.velocity.magnitude;
        float targetShake = (speed * shakeGradient) + minShake;
        if(targetShake > maxShake) targetShake = maxShake;
        
        CinemachineBasicMultiChannelPerlin perlin = playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = targetShake;

    }

    public void SetPlayer(GameObject playerObject){
        player = playerObject.GetComponent<Rigidbody2D>();
        playerCam.Follow = playerObject.transform;
    }
}
