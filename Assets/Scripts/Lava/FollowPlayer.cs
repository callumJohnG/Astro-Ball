using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public static FollowPlayer Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    
    [SerializeField] private RisingLava risingLava;

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
            risingLava.LavaRise();
        }
    }

    #region Interface Methods

    public void StartRisingTracking(Transform trackTransform){
        isRising = true;
        isTracking = true;
        this.trackTransform = trackTransform;
        risingLava.SetTrackTransform(trackTransform);
    }

    public void PlayerDied(){
        isTracking = false;
        risingLava.SetStopped();
        trackTransform = null;
        risingLava.SetTrackTransform(null);
    }

    public void Reset(){
        isTracking = false;
        isRising = false;
        risingLava.transform.position = Vector3.zero;
        transform.position = Vector3.zero;

        risingLava.SetStarted();
        
        trackTransform = null;
    }

    #endregion

    private void TrackPlayer(){
        Vector3 newPosition = transform.position;
        
        newPosition.x = trackTransform.position.x;

        transform.position = newPosition;
    }

}
