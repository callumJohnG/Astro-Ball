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
        menuScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        gameHud.SetActive(false);
    }

    public void StartGame(){
        gameIsActive = true;

        menuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameHud.SetActive(true);

        KillPlayer();
        SpawnCharacter();
        ObjectSpawningManager.Instance.SpawnGrid();
        ObjectSpawningManager.Instance.WipeAllBumpers();
        PowerupManager.Instance.WipeAllPowerups();
        PointsManager.Instance.ResetPoints();
    }


    [SerializeField] private GameObject playerPrefab;
    public PlayerController player {get;private set;}

    [SerializeField] private Transform spawnPos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private GameObject gameHud;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameOverScreen;

    public bool gameIsActive {get; private set;}

    public void GameOver(){
        player.UpdateLaunchCount();

        HighscoreManager.Instance.SetData(PointsManager.Instance.points, PointsManager.Instance.bestCombo);
        PowerupManager.Instance.ClearPowerupText();

        gameIsActive = false;
        //Game over animation here
        gameHud.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void MainMenu(){
        KillPlayer();
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);
        //Spawn a player
        //Spawn the world
    }

    public void Quit(){
        Application.Quit();
    }

    private void SpawnCharacter(){
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation).GetComponent<PlayerController>();
        SmartPlayerCamera.Instance.SetPlayer(player.gameObject);
        PowerupManager.Instance.SetPlayerObject(player.gameObject);
    }

    private void KillPlayer(){
        if(player == null)return;

        Destroy(player.gameObject);
    }
}
