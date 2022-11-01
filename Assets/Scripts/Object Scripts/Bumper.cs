using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    private void Awake(){
        myCollider = GetComponent<Collider2D>();

        CheckOverlapping();
    }

    private Collider2D myCollider;

    private void CheckOverlapping(){
        //Disable my collider so it doesnt interfere
        myCollider.enabled = false;

        //Check if we collide with anything in the scene (aside from other bumpers)
        Collider2D overlappingCollider = Physics2D.OverlapCircle(transform.position, GameSettingsManager.Instance.bumperSpacing);
        if(overlappingCollider != null){
            Debug.Log("I overlap");
            Debug.Log(overlappingCollider);
            Destroy(gameObject);
        }

        //Reenable my collider
        myCollider.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision2D){
        if(collision2D.gameObject.CompareTag("Player")){
            //Give the player an extra launch
            collision2D.gameObject.GetComponent<PlayerController>().UpdateLaunchCount(1);
            Die();
        }
    }

    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private int rewardedPoints;
    [SerializeField] private LayerMask ignoreLayer;

    private void Die(){
        PointsManager.Instance.GainPoints(rewardedPoints);

        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
    }
}
