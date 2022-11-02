using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDistanceObject : MonoBehaviour
{

    private Transform player;

    // Update is called once per frame
    void Update()
    {
        if(!GameplayManager.Instance.gameIsActive)return;

        CheckDistance();
    }

    private void SeekPlayer(){
        player = GameObject.FindWithTag("Player").transform;
    }

    private void CheckDistance(){
        if(player == null){
            SeekPlayer();
            return;
        }
        if(Vector3.Distance(transform.position, player.position) > GameSettingsManager.Instance.maxRenderDistance){
            Destroy(gameObject);
        }
    }
}
