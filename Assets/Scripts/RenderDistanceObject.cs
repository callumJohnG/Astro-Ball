using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDistanceObject : MonoBehaviour
{

    [SerializeField] private float maxRenderDistance = 10f;
    private Transform player;

    // Update is called once per frame
    void Update()
    {
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
        if(Vector3.Distance(transform.position, player.position) > maxRenderDistance){
            Destroy(gameObject);
        }
    }
}
