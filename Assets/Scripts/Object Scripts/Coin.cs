using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int rewardedPoints = 1;

    private void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.CompareTag("Player")){
            //We've been collected
            Collect();
        }
    }

    private void Collect(){
        PointsManager.Instance.GainPoints(rewardedPoints);
    }
}
