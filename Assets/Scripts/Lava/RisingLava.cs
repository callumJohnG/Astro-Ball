using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{

    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float lavaRiseSpeed;
    private float currentRiseSpeed;
    private float currentGoalSpeed;
    [SerializeField] private float lavaLerpSpeed = 1f;
    private Transform trackTransform;

    public void SetTrackTransform(Transform trackTransform){
        this.trackTransform = trackTransform;
    }

    private void Start() {
        SetStarted();
    }

    public void SetStarted(){
        currentGoalSpeed = lavaRiseSpeed;
        currentRiseSpeed = lavaRiseSpeed;
    }

    public void SetStopped(){
        currentGoalSpeed = 0;
    }

    private void Update() {
        //Lerp the speed to its current goal
        currentRiseSpeed = Mathf.Lerp(currentRiseSpeed, currentGoalSpeed, Time.deltaTime * lavaLerpSpeed);
    }

    public void LavaRise(){
        Vector3 newPosition = transform.position;
        
        //Rise by the rise speed
        newPosition.y += (currentRiseSpeed * Time.deltaTime);

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
