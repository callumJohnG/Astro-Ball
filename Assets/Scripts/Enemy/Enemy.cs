using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision2D){
        if(collision2D.gameObject.CompareTag("Player")){
            Debug.Log("THE PLAYER HIT ME");
            Die();
        }
    }

    [SerializeField] private GameObject explosionParticles;

    private void Die(){
        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
    }
}
