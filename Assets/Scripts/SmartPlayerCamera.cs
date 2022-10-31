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

    private CinemachineVirtualCamera playerCam;
    [SerializeField] private Rigidbody2D player;


    // Update is called once per frame
    void Update()
    {
        CalculateCameraZoom();
    }

    [SerializeField] private float gradient = 0.1f;
    [SerializeField] private float minZoom = 10;
    [SerializeField] private float maxZoom = 50;

    private void CalculateCameraZoom(){

        float playerVelocity = Mathf.Abs(player.velocity.x) + Mathf.Abs(player.velocity.y);

        //x = 0, y = 10
        //y = 50 is the max
        //increment in steps of 0.25 per X
        //y = 0.25X + 10
        float targetOrtho = (playerVelocity * gradient) + minZoom;
        if(targetOrtho > maxZoom)targetOrtho = maxZoom;

        playerCam.m_Lens.OrthographicSize = Mathf.Lerp(playerCam.m_Lens.OrthographicSize, targetOrtho, Time.deltaTime);
    }

    public void SetPlayer(GameObject playerObject){
        player = playerObject.GetComponent<Rigidbody2D>();
        playerCam.Follow = playerObject.transform;
    }
}
