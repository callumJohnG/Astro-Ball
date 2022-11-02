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
        SpawnBumper();
    }

    #region Obstacle Spawning

    [Header("Obstacles")]
    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    [SerializeField] private float bounds;
    [SerializeField] private GameObject gridPointPrefab;
    [SerializeField] private Transform gridPointsContainer;
    private List<GridPoint> gridPoints = new List<GridPoint>();

    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private List<GameObject> debugPrefab;
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
            if(debugPrefab != null) gridPoint.SetData(debugPrefab, spawnProbability);
            else gridPoint.SetData(obstaclePrefabs, spawnProbability);
            gridPoint.SpawnObstacle();
            gridPoints.Add(gridPoint);
        }
    }

    public void RespawnGrid(){
        foreach(GridPoint gridPoint in gridPoints){
            if(gridPoint != null){
                gridPoint.WipeObstacle();
            }
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

    #endregion

    #region Bumper Spawning

    [SerializeField] private List<GameObject> bumperPrefabs;
    [SerializeField] private List<float> bumperProbabilities;
    [SerializeField] private List<GameObject> powerupPrefabs;
    [SerializeField] private float powerupProbability = 1;
    [SerializeField] private float minimumBumperY;
    [SerializeField] private float bumperRingMin;
    [SerializeField] private float bumperRingMax;
    [SerializeField] private Transform bumpersContainer;
    private List<GameObject> bumpers = new List<GameObject>();

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(GameplayManager.Instance.player.transform.position, bumperRingMax);
        Gizmos.DrawWireSphere(GameplayManager.Instance.player.transform.position, bumperRingMin);
    }

    private void SpawnBumper(){
        //Get random point
        Vector3 randomPoint = GetRandomPointInCircle(GameplayManager.Instance.player.transform.position, bumperRingMin, bumperRingMax);
        
        //Try to spawn a bumper there
        GameObject bumper = Instantiate(GetRandomBumper(), randomPoint, Quaternion.identity, bumpersContainer);
        if(bumper != null){
            bumpers.Add(bumper);
        }
    }

    private GameObject GetRandomBumper(){
        //Return a powerup
        if(Random.Range(0,100) < powerupProbability){
            return(powerupPrefabs[Random.Range(0, powerupPrefabs.Count)]);
        }

        int randomNum = Random.Range(0,100);
        float checkVal = 0;
        for(int i = 0; i < bumperPrefabs.Count; i ++){
            checkVal += bumperProbabilities[i];
            if(randomNum <= checkVal){
                Debug.Log("F");
                return bumperPrefabs[i];
            }
        }
        Debug.Log("Default");
        return bumperPrefabs[0];
    }

    public void WipeAllBumpers(){
        foreach(GameObject bumper in bumpers){
            if(bumper == null)continue;

            Destroy(bumper);
        }
        bumpers.Clear();
    }

    private Vector3 GetRandomPointInCircle(Vector3 center, float minDistance, float maxDistance){
        //Pick random direction
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomDirection = Vector3.Normalize(randomDirection);

        //Get random magnitude
        float randomMagnitude = Random.Range(minDistance, maxDistance);

        //Multiply the direction to get random point in the range
        randomDirection *= randomMagnitude;

        //Get that as a vector from the center
        randomDirection += center;

        return randomDirection;
    }

    #endregion
}
