using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{

    private float maxDistanceFromPlayer;
    [SerializeField] private float currentSpeed;
    private float startSpeed;
    private float maxSpeed;
    private float acceleration;
    [SerializeField] private float lavaMenuSpeed = 15;
    [SerializeField] private Transform menuGoalTransform;
    private Vector3 menuGoalPosition;
    [SerializeField] private float lavaLerpSpeed = 1f;
    private Transform trackTransform;
    private bool lerpingToMenu;
    private bool rising;
    private float cacheSpeed;

    public void SetSpeed(float startSpeed, float maxSpeed, float acceleration, float lavaDistance){
        this.startSpeed = startSpeed;
        this.maxSpeed = maxSpeed;
        this.acceleration = acceleration;
        this.maxDistanceFromPlayer = lavaDistance;
        this.currentSpeed = startSpeed;
    }

    public void SetTrackTransform(Transform trackTransform){
        this.trackTransform = trackTransform;
    }

    public void SetStarted(bool useCache = false){
        Debug.Log("LAVA IS NOW RISING!");
        if(useCache){
            currentSpeed = cacheSpeed;
        } else {
            currentSpeed = startSpeed;
        }
        rising = true;
    }

    public void SetStopped(){
        rising = false;
        cacheSpeed = currentSpeed;
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
        CalculateSpeed();
        SetLavaPosition();
    }

    private void CalculateSpeed(){
        //Inc speed by our acceleration
        currentSpeed += (acceleration * Time.deltaTime);

        //Check if our speed has hit the limit
        if(currentSpeed > maxSpeed){
            currentSpeed = maxSpeed;
        }
    }

    private void SetLavaPosition(){
        Vector3 newPosition = transform.position;
        
        //Rise by the current speed
        newPosition.y += (currentSpeed * Time.deltaTime);

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
