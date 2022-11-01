using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision2D){
        if(collision2D.gameObject.CompareTag("Player")){
            //Give the player an extra launch
            collision2D.gameObject.GetComponent<PlayerController>().UpdateLaunchCount(1);
            Die();
        }
    }

    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private int rewardedPoints;

    private void Die(){
        PointsManager.Instance.GainPoints(rewardedPoints);

        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
    }
}
