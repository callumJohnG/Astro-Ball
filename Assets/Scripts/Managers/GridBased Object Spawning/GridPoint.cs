using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private List<GameObject> obstaclePrefabs;
    private float spawnProbability;
    private GameObject storedObstacle;
    public bool active {get; private set;} = false;

    public void SetData(List<GameObject> obstaclePrefabs, float spawnProbability){
        this.obstaclePrefabs = obstaclePrefabs;
        this.spawnProbability = spawnProbability;
    }

    private void Update(){
        if(!GameplayManager.Instance.gameIsActive) return;

        CheckRangeToPlayer();
    }

    private void CheckRangeToPlayer(){
        if(Vector3.Distance(GameplayManager.Instance.player.transform.position, transform.position) > GameSettingsManager.Instance.maxRenderDistance){
            //We are far away from the player
            //Destory our object
            WipeObstacle();
        }
    }

    public void Move(){

    }

    public void SpawnObstacle(){
        active = true;
        
        if(Random.Range(0, 100) > spawnProbability)return;

        if(obstaclePrefabs.Count <= 0)return; // THIS IS BECAUSE OF A BUG, THE FIRST GRID POINT TO SPAWN SOMETHING HAS A LIST OF 0 ELEMENTS IDK WHY

        storedObstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], transform.position, transform.rotation, transform);

        //Random rotation
        Vector3 rotation = new Vector3(0, 0, 0);
        if(Random.Range(0,100) > 50){
            rotation.y = 180;
        }
        storedObstacle.transform.Rotate(rotation);
    }

    public void WipeObstacle(){
        active = false;
        Destroy(storedObstacle);
        Destroy(gameObject);
    }
}
