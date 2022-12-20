using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float boostForce;

    private void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.CompareTag("Player")){
            collider2D.GetComponent<PlayerController>().Boost(boostForce);
            AudioManager.Instance.PlayZoom();
        }
    }
}
