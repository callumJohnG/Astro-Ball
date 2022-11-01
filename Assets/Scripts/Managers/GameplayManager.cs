using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance {get; private set;}

    private void Awake(){
        Instance = this;
    }

    private void Start(){
        Restart();
    }


    [SerializeField] private GameObject playerPrefab;
    private PlayerController player;

    [SerializeField] private Transform spawnPos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    public void GameOver(){
        Debug.Log("Player has lost, kill them");
        Destroy(player.gameObject);
        Restart();
    }

    public void Restart(){
        //Spawn a player
        //Spawn the world
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation).GetComponent<PlayerController>();
        SmartPlayerCamera.Instance.SetPlayer(player.gameObject);
    }
}
