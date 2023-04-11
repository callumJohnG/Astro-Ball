using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalInfiniteGroup : MonoBehaviour
{

    private float startPosition;
    [SerializeField] private GameObject trackingCamera;
    [SerializeField] private float parallaxStrength;


    private List<GameObject> groupObjects;
    [SerializeField] private Transform groupParent;
    [SerializeField] private float spacingDistance;
    private int objectCount;
    

    void Start(){
        GetGroupObjects();
        startPosition = transform.position.x;

        SetPositions();
    }

    private void Update() {
        SetParallaxPositions();
    }

    private void GetGroupObjects(){
        groupObjects = new List<GameObject>();
        foreach(Transform child in groupParent){
            groupObjects.Add(child.gameObject);
        }

        objectCount = groupObjects.Count;
    }

    private void SetPositions(){
        List<float> xPositions = CalculatePositions(spacingDistance, objectCount);


        for(int i = 0; i < objectCount; i++){
            GameObject nextObject = groupObjects[i];
            float nextXPos = xPositions[i];

            Vector3 newPosition = new Vector3(nextXPos, 0, 0);
            nextObject.transform.localPosition = newPosition;
        }
    }

    private List<float> CalculatePositions(float spacingDistance, float objectCount){
        float totalDistance = spacingDistance * (objectCount - 1);
        float firstPosition = (totalDistance / 2) * (-1);

        //We have the first position, and the spacing
        //We can calculate the position of each object in the list

        List<float> result = new List<float>();
        for(int i = 0; i < objectCount; i++){
            float nextPoint = firstPosition + (spacingDistance * i);
            result.Add(nextPoint);
        }

        return result;
    }

    private void SetParallaxPositions(){
        float temp = (trackingCamera.transform.position.x * (1 - parallaxStrength));
        float distance = (trackingCamera.transform.position.x * parallaxStrength);

        transform.position = new Vector3(startPosition * distance, transform.position.y, transform.position.z);

        if(temp > startPosition + spacingDistance){
            startPosition += spacingDistance;
        } else if (temp < startPosition - spacingDistance){
            startPosition -= spacingDistance;
        }
    }

}
