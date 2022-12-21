using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bumper : MonoBehaviour
{

    private void Awake(){
        myColliders = GetComponentsInChildren<Collider2D>();

        CheckOverlapping();
    }

    private Collider2D[] myColliders;

    private void CheckOverlapping(){
        //Disable my collider so it doesnt interfere
        SetColliders(false);

        //Check if we collide with anything in the scene (aside from other bumpers)
        Collider2D overlappingCollider = Physics2D.OverlapCircle(transform.position, GameSettingsManager.Instance.bumperSpacing);
        if(overlappingCollider != null){
            Destroy(gameObject);
        }

        //Reenable my collider
        SetColliders(true);
    }

    private void SetColliders(bool enabled){
        foreach(Collider2D collider2D in myColliders){
            collider2D.enabled = enabled;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D){
        if(isDeadlyBumper)return;
        
        if(collision2D.gameObject.CompareTag("Player")){
            //Give the player an extra launch
            collision2D.gameObject.GetComponent<PlayerController>().UpdateLaunchCount(1);

            /*if(myPowerup != PowerupType.None){
                PowerupManager.Instance.AddPowerup(myPowerup);
            }*/
            if(isPowerup){
                PowerupManager.Instance.AddPowerup();
            }

            Die();
        }
    }

    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private int rewardedPoints;
    [SerializeField] private bool isDeadlyBumper;
    [SerializeField] private bool isPowerup;
    [SerializeField] GameObject pointCanvasPrefab;

    private void Die(){
        if(!isDeadlyBumper){
            PointsManager.Instance.GainComboPoints(rewardedPoints);
            Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
            
            GameObject myCanvas = Instantiate(pointCanvasPrefab, transform.position, Quaternion.identity);
            pointsText = myCanvas.GetComponentInChildren<TextMeshProUGUI>();
            pointsAnimtor = myCanvas.GetComponentInChildren<Animator>();

            pointsText.text = "+" + Mathf.FloorToInt(rewardedPoints * GameSettingsManager.Instance.pointsMultiplier);
            
            pointsAnimtor.SetBool("Fade", true);

            Destroy(myCanvas, 5f);
        }
        
        Destroy(gameObject);
    }

    private Animator pointsAnimtor;
    private TextMeshProUGUI pointsText;
}
