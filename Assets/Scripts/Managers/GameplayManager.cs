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

    private const string FIRST_EVER_PLAY = "FirstEverPlay";

    public void StartGame(){
        //Check if the player has ever clicked this button before!
        if(PlayerPrefs.GetInt(FIRST_EVER_PLAY, 0) == 0){
            //Player has never played before
            //Show Glossary Screen
            OpenGlossary(ScreenType.Play);
            PlayerPrefs.SetInt(FIRST_EVER_PLAY, 1);
            return;
        }

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

        CloseAllScreens();
        gameHud.SetActive(true);

        KillPlayer();
        SpawnCharacter();
        ObjectSpawningManager.Instance.SpawnGrid();
        ObjectSpawningManager.Instance.WipeAllBumpers();
        PowerupManager.Instance.WipeAllPowerups();
        TutorialBrain.Instance.StartTutorial(GameSettingsManager.Instance.inverseAiming);
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
    [SerializeField] private GameObject glossaryScreen;
    private ScreenType nextScreenAfterGlossary;
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
        CloseAllScreens();
        agreementScreen.SetActive(true);
    }

    public void CompletedAgreement(){
        PlayerPrefs.SetInt(AGREEMENT_KEY, 1);
    }

    public void MainMenu(){
        KillPlayer();
        CloseAllScreens();
        menuScreen.SetActive(true);
        //Spawn a player
        //Spawn the world
    }

    public void ShowLeaderboard(){
        CloseAllScreens();
        leaderboardScreen.SetActive(true);
    }

    public void ShowShop(){
        CloseAllScreens();
        shopScreen.SetActive(true);
    }

    public void OpenGlossary(){
        OpenGlossary(ScreenType.MainMenu);
    }

    public void OpenGlossary(ScreenType nextScreen){
        CloseAllScreens();
        glossaryScreen.SetActive(true);
        nextScreenAfterGlossary = nextScreen;
    }

    public void CloseGlossary(){
        if(nextScreenAfterGlossary == ScreenType.Null){
            Debug.LogError("Next screen is Null");
            return;
        }

        CloseAllScreens();
        switch(nextScreenAfterGlossary){
            case ScreenType.MainMenu:
                menuScreen.SetActive(true);
                break;
            case ScreenType.Play:
                StartGame();
                break;
        }

        nextScreenAfterGlossary = ScreenType.Null;
    }

    private void CloseAllScreens(){
        gameHud.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
        leaderboardScreen.SetActive(false);
        shopScreen.SetActive(false);
        agreementScreen.SetActive(false);
        glossaryScreen.SetActive(false);
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

public enum ScreenType{
    Null,
    MainMenu,
    Play
}