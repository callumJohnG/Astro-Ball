using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private bool followX;
    [SerializeField] private bool followY;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(GameplayManager.Instance.player == null || !GameplayManager.Instance.gameIsActive){
            return;
        }

        Vector3 newPosition = transform.position;
        
        if(followX){
            newPosition.x = GameplayManager.Instance.player.transform.position.x;
        }

        if(followY){
            newPosition.y = GameplayManager.Instance.player.transform.position.y;
        }
        transform.position = newPosition;
    }
}
