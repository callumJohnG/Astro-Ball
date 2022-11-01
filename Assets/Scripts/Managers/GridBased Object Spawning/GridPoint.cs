using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private List<GameObject> obstaclePrefabs;
    private float spawnProbability;
    private GameObject storedObstacle;

    public void SetData(List<GameObject> obstaclePrefabs, float spawnProbability){
        this.obstaclePrefabs = obstaclePrefabs;
        this.spawnProbability = spawnProbability;
    }

    public void Move(){

    }

    public void SpawnObstacle(){
        if(Random.Range(0, 100) > spawnProbability)return;

        storedObstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], transform.position, transform.rotation, transform);
    }

    public void WipeObstacle(){
        Destroy(storedObstacle);
        storedObstacle = null;
    }
}
