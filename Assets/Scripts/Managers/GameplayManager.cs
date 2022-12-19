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
        MainMenu();
    }

    public void StartGame(){
        PrepareGame();
        PointsManager.Instance.ResetPoints();
    }

    public void ContinueGame(){
        PrepareGame();
    }

    private void PrepareGame(){
        gameIsActive = true;

        menuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        continueScreen.SetActive(false);
        gameHud.SetActive(true);

        FollowPlayer.Instance.Reset();
        KillPlayer();
        SpawnCharacter();
        ObjectSpawningManager.Instance.SpawnGrid();
        ObjectSpawningManager.Instance.WipeAllBumpers();
        PowerupManager.Instance.WipeAllPowerups();
    }


    [SerializeField] private GameObject playerPrefab;
    public PlayerController player {get;private set;}

    [SerializeField] private Transform spawnPos;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private GameObject gameHud;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject continueScreen;
    [SerializeField] private GameObject gameOverScreen;

    public bool gameIsActive {get; private set;}

    [SerializeField] private HighscoreManager highscoreManager;
    public void GameOver(){
        player.UpdateLaunchCount();

        highscoreManager.SetData(PointsManager.Instance.points, PointsManager.Instance.bestCombo);
        PowerupManager.Instance.ClearPowerupText();
        FollowPlayer.Instance.PlayerDied();

        gameIsActive = false;
        //Game over animation here
        gameHud.SetActive(false);
        continueScreen.SetActive(true);
        Time.timeScale = 1;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine(){
        yield return Leaderboard.Instance.SubmitScoreRoutine(PointsManager.Instance.points);
        Leaderboard.Instance.SetCurrentLeaderboard();
    }

    public void MainMenu(){
        KillPlayer();
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);
        continueScreen.SetActive(false);
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
        try{
            HeightUIManager.Instance.SetPlayer(player.transform);
        }catch{}
    }

    private void KillPlayer(){
        if(player == null)return;

        Destroy(player.gameObject);
    }
}
