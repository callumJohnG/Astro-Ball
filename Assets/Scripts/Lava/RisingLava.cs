using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{

    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float lavaRiseSpeed;
    [SerializeField] private Transform menuGoalTransform;
    private Vector3 menuGoalPosition;
    private float currentRiseSpeed;
    private float currentGoalSpeed;
    [SerializeField] private float lavaLerpSpeed = 1f;
    private Transform trackTransform;
    private bool lerpingToMenu;
    private bool rising;

    public void SetTrackTransform(Transform trackTransform){
        this.trackTransform = trackTransform;
    }

    private void Start() {
        SetStarted();
    }

    public void SetStarted(){
        rising = true;
        //menuGoalPosition = Camera.main.ScreenToWorldPoint(menuGoalTransform.position);
        Debug.LogError(menuGoalPosition);
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
        Debug.Log("CurrentPos " + newPosition.y);
        Debug.Log("TargetPos " + menuGoalTransform.position.y);
        newPosition.y = Mathf.Lerp(newPosition.y, menuGoalTransform.position.y, Time.deltaTime * lavaLerpSpeed);
        transform.position = newPosition;
        Debug.Log("NewPos " + transform.position.y);
    }

    public void LavaRise(){
        Debug.Log("Rising lava");
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
