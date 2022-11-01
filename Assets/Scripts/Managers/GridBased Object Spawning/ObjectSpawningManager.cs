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

    public static ObjectSpawningManager Instance {get;private set;}

    private void Awake(){
        Instance = this;
    }

    private void Start(){

    }

    private void Update(){
        CheckGrid();
    }


    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    [SerializeField] private float bounds;
    [SerializeField] private GameObject gridPointPrefab;
    [SerializeField] private Transform gridPointsContainer;
    private List<GridPoint> gridPoints = new List<GridPoint>();

    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private float spawnProbability;

    [SerializeField] private int minimumY = 60;
    [SerializeField] private int maximumY = 1000;
    public void SpawnGrid(){
        gridPoints.Clear();

        //Get the list of grid positions around the player
        List<Vector3> gridPositions = GetGridPositions(GameplayManager.Instance.player.transform.position);

        //Use all of our grid positions to spawn a grid point
        foreach(Vector3 gridPosition in gridPositions){
            GridPoint gridPoint = Instantiate(gridPointPrefab, gridPosition, Quaternion.identity, gridPointsContainer).GetComponent<GridPoint>();
            gridPoint.SetData(obstaclePrefabs, spawnProbability);
            gridPoint.SpawnObstacle();
            gridPoints.Add(gridPoint);
        }
        Debug.Log(gridPoints.Count);
    }

    public void RespawnGrid(){
        foreach(GridPoint gridPoint in gridPoints){
            try{gridPoint.WipeObstacle();}
            catch {}
        }
        gridPoints.Clear();
        SpawnGrid();
    }

    private void CheckGrid(){
        List<Vector3> gridPositions = GetGridPositions(GameplayManager.Instance.player.transform.position);

        //for each of these positions, see if there is a gridpoint there
        foreach(GridPoint gridPoint in gridPoints){
            if(gridPoint == null)continue;
            if(gridPositions.Contains(gridPoint.transform.position)){
                //This grid point is valid
                //If its not active, spawn an obstacle
                if(!gridPoint.active)gridPoint.SpawnObstacle();

                //Remove this gridposition from the list
                gridPositions.Remove(gridPoint.transform.position);
            }
        }

        //All the grid positions left in the list have no grid point there, instantiate a new one????
        int newPointCount = 0;
        foreach(Vector3 gridPosition in gridPositions){
            //Check if the point is too far away
            if(Vector3.Distance(gridPosition, GameplayManager.Instance.player.transform.position) > GameSettingsManager.Instance.maxRenderDistance){
                //TOO FAR AWAY
                continue;
            }

            newPointCount++;
            GridPoint newGridPoint = Instantiate(gridPointPrefab, gridPosition, Quaternion.identity, gridPointsContainer).GetComponent<GridPoint>();
            gridPoints.Add(newGridPoint);
            newGridPoint.SetData(obstaclePrefabs, spawnProbability);
            newGridPoint.SpawnObstacle();
        }
        if(newPointCount > 0)Debug.Log(newPointCount  + " New points added");
    }



    private List<Vector3> GetGridPositions(Vector3 currentPlayerPosition){
        //Round player position to nearest grid point
        //Use that as the center
        //Get all grid points around it
        Vector3 nearestGridPosition = new Vector3(Mathf.Round(currentPlayerPosition.x / bounds) * bounds, (Mathf.Round(currentPlayerPosition.y / bounds) * bounds) + minimumY);

        List<Vector3> newGridPositions = new List<Vector3>();
        //Bottom right posisition
        Vector3 startPoint = new Vector3( nearestGridPosition.x - ((xCount / 2) * bounds), nearestGridPosition.y - ((yCount/2) * bounds));
        Vector3 endPoint = new Vector3(nearestGridPosition.x + ((xCount / 2) * bounds), nearestGridPosition.y + ((yCount/2) * bounds));

        
        for(float x = startPoint.x; x <= endPoint.x; x += bounds){
            for(float y = startPoint.y; y <= endPoint.y; y += bounds){
                if(y < minimumY)continue;
                if(y > maximumY)continue;

                newGridPositions.Add(new Vector3(x, y));
            }
        }

        return newGridPositions;
    }
}
