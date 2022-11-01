using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawningManager : MonoBehaviour
{

    //This class contains the logic pertaining to spawning obstacles and bumpers in a grid based fashion around the world
    //HOW IT WORKS
    //..
    //..
    //There are grid points around the world


    //RIGHT NOW - set boundary of spawn spots
    //using this set boundary, spawn a random obstacle at each one
    private void Start(){
        SpawnGrid();
    }


    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    [SerializeField] private float bounds;
    [SerializeField] private GameObject gridPointPrefab;
    [SerializeField] private Transform gridPointsContainer;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GridPoint> gridPoints = new List<GridPoint>();

    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private float spawnProbability;
    private void SpawnGrid(){
        //Get the first point (bottom left)

        int startX = 0;
        int startY = 40;


        Vector3 initialPoint = new Vector3(startX - ((xCount/2) * bounds), startY);
        Vector3 endPoint = new Vector3(startX + ((xCount/2) * bounds), startY + (yCount * bounds));

        for(float x = initialPoint.x; x < endPoint.y; x += bounds){
            for(float y = initialPoint.y; y < endPoint.y; y += bounds){
                //THIS IS A GRID POINT
                gridPositions.Add(new Vector3(x, y));
            }
        }


        //Use all of our grid positions to spawn a grid point
        foreach(Vector3 gridPosition in gridPositions){
            GridPoint gridPoint = Instantiate(gridPointPrefab, gridPosition, Quaternion.identity, gridPointsContainer).GetComponent<GridPoint>();
            gridPoint.SetData(obstaclePrefabs, spawnProbability);
            gridPoint.SpawnObstacle();
            gridPoints.Add(gridPoint);
        }

    }
}
