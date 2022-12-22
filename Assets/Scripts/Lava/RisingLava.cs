using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{

    private float maxDistanceFromPlayer;
    private float lavaRiseSpeed;
    [SerializeField] private float lavaMenuSpeed = 15;
    [SerializeField] private Transform menuGoalTransform;
    private Vector3 menuGoalPosition;
    private float currentRiseSpeed;
    private float currentGoalSpeed;
    [SerializeField] private float lavaLerpSpeed = 1f;
    private Transform trackTransform;
    private bool lerpingToMenu;
    private bool rising;

    public void SetSpeed(float lavaSpeed, float lavaDistance){
        this.lavaRiseSpeed = lavaSpeed;
        this.maxDistanceFromPlayer = lavaDistance;
    }

    public void SetTrackTransform(Transform trackTransform){
        this.trackTransform = trackTransform;
    }

    private void Start() {
        SetStarted();
    }

    public void SetStarted(){
        rising = true;
    }

    public void SetStopped(){
        rising = false;
    }

    private void Update() {
        if(!rising) {
            LerpToMenu();
        }
    }

    private void LerpToMenu(){
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Lerp(newPosition.y, menuGoalTransform.position.y, Time.deltaTime * lavaMenuSpeed);
        transform.position = newPosition;
    }

    public void LavaRise(){
        Vector3 newPosition = transform.position;
        
        //Rise by the rise speed
        newPosition.y += (lavaRiseSpeed * Time.deltaTime);

        //Check max distance
        if(trackTransform != null){
            float playerY = trackTransform.position.y;
            if((playerY - transform.position.y) > maxDistanceFromPlayer){
                newPosition.y = playerY - maxDistanceFromPlayer;
            }
        }

        transform.position = newPosition;
    }

    public void Reset(){
        transform.position = Vector3.zero;
    }

    private void OnDrawGizmos() {
        //Vertical tracking line
        Gizmos.DrawRay(transform.position, Vector3.up * maxDistanceFromPlayer);
    }
}
