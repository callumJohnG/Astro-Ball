using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public static FollowPlayer Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float lavaRiseSpeed;
    
    private bool isRising = false;
    private bool isTracking = false;
    private float defaultY;
    private Transform trackTransform = null;

    private void Start() {
        defaultY = transform.position.y;
    }

    void Update()
    {
        if(isTracking){
            TrackPlayer();
        }

        if(isRising){
            LavaRise();
        }
    }

    #region Interface Methods

    public void StartRisingTracking(Transform trackTransform){
        isRising = true;
        isTracking = true;
        this.trackTransform = trackTransform;
    }

    public void PlayerDied(){
        isTracking = false;
    }

    public void Reset(){
        isTracking = false;
        isRising = false;
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.x);
        trackTransform = null;
    }

    #endregion

    #region Tracking / Rising Lava

    private void TrackPlayer(){
        Vector3 newPosition = transform.position;
        
        newPosition.x = trackTransform.position.x;

        transform.position = newPosition;
    }


    private void LavaRise(){
        Vector3 newPosition = transform.position;
        
        //Rise by the rise speed
        newPosition.y += (lavaRiseSpeed * Time.deltaTime);

        //Check max distance
        if(trackTransform != null){
            float playerY = trackTransform.position.y;
            if((- playerY + transform.position.y) > maxDistanceFromPlayer){
                newPosition.y = playerY - maxDistanceFromPlayer;
            }
        }

        transform.position = newPosition;
    }

    #endregion
}
