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

        storedObstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], transform.position, transform.rotation, transform);
    }

    public void WipeObstacle(){
        active = false;
        Destroy(storedObstacle);
        Destroy(gameObject);
    }
}
