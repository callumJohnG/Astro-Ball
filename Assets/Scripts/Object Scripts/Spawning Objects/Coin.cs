using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.CompareTag("Player")){
            //We've been collected
            Collect();
        }
    }

    private void Collect(){
        AudioManager.Instance.PlayCoin();
        PointsManager.Instance.GainPoints(GameSettingsManager.Instance.coinReward);
        Destroy(gameObject);
    }
}
