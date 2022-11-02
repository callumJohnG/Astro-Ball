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
        SpawnCharacter();
        ObjectSpawningManager.Instance.SpawnGrid();
    }


    [SerializeField] private GameObject playerPrefab;
    public PlayerController player {get;private set;}

    [SerializeField] private Transform spawnPos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    public void GameOver(){
        Destroy(player.gameObject);
        Restart();
    }

    public void Restart(){
        //Spawn a player
        //Spawn the world
        SpawnCharacter();
        ObjectSpawningManager.Instance.RespawnGrid();
        ObjectSpawningManager.Instance.WipeAllBumpers();
    }

    private void SpawnCharacter(){
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation).GetComponent<PlayerController>();
        SmartPlayerCamera.Instance.SetPlayer(player.gameObject);
        PowerupManager.Instance.SetPlayerObject(player.gameObject);
    }
}
