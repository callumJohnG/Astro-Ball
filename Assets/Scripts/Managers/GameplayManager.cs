using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance {get; private set;}

    private void Awake(){
        Instance = this;
        SetUp();
    }

    public event EventHandler OnGameEnded;
    public event EventHandler OnGameStarted;

    private void SetUp(){
        //PlayerPrefs.DeleteAll();

        if(PlayerPrefs.GetInt(AGREEMENT_KEY, 0) == 0){
            Agreement();
        } else {
            MainMenu();
        }
    }

    public void StartGame(){
        PrepareGame();
        FollowPlayer.Instance.Reset(false);
        PointsManager.Instance.ResetPoints();
        continueButtonsManager.ResetButtons();

        OnGameStarted.Invoke(this, EventArgs.Empty);
    }

    public void ContinueGame(){
        PrepareGame();
        FollowPlayer.Instance.Reset(true);

        OnGameStarted.Invoke(this, EventArgs.Empty);
    }

    private void PrepareGame(){
        gameIsActive = true;

        menuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        shopScreen.SetActive(false);
        gameHud.SetActive(true);

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
    [SerializeField] private GameObject leaderboardScreen;
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject agreementScreen;
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private ContinueButtonsManager continueButtonsManager;

    public bool gameIsActive {get; private set;}

    [SerializeField] private HighscoreManager highscoreManager;
    public void GameOver(){
        player.UpdateLaunchCount();

        highscoreManager.SetData(PointsManager.Instance.points, PointsManager.Instance.bestCombo);
        PowerupManager.Instance.ClearPowerupText();
        PowerupManager.Instance.WipeAllPowerups();
        FollowPlayer.Instance.PlayerDied();

        gameIsActive = false;
        //Game over animation here
        gameHud.SetActive(false);
        gameOverAnimator.CrossFade("GameOver", 0, 0, 0);
        gameOverScreen.SetActive(true);
        Time.timeScale = 1;

        //Gain XP
        PointsManager.Instance.SubmitPointsToXP();
        
        //Set the continue buttons
        continueButtonsManager.LoadButtons();

        OnGameEnded.Invoke(this, EventArgs.Empty);

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine(){
        if(GameSettingsManager.Instance.canPostScore){
            yield return Leaderboard.Instance.SubmitScoreRoutine(PointsManager.Instance.points);
        }
        Leaderboard.Instance.SetCurrentLeaderboard();
    }

    private const string AGREEMENT_KEY = "CompletedAgreement";

    private void Agreement(){
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        shopScreen.SetActive(false);
        agreementScreen.SetActive(true);
    }

    public void CompletedAgreement(){
        PlayerPrefs.SetInt(AGREEMENT_KEY, 1);
    }

    public void MainMenu(){
        KillPlayer();
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);
        leaderboardScreen.SetActive(false);
        shopScreen.SetActive(false);
        agreementScreen.SetActive(false);
        //Spawn a player
        //Spawn the world
    }

    public void ShowLeaderboard(){
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
        leaderboardScreen.SetActive(true);
        shopScreen.SetActive(false);
    }

    public void ShowShop(){
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        shopScreen.SetActive(true);
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
