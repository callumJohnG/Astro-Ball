using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;

    [SerializeField] int normalLeaderboardID = 8694;
    [SerializeField] int hardLeaderboardID = 8695;
    [SerializeField] int insaneLeaderboardID = 8696;

    [SerializeField] private Transform scoresContainer;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private GameObject loadingVisuals;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Update(){
        CheckLoadingVisuals();
    }

    private void CheckLoadingVisuals(){
        if(scoresContainer.childCount == 0){
            loadingVisuals.SetActive(true);
        } else {
            loadingVisuals.SetActive(false);
        }
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload){
        int leaderboardID = GetLeaderboardID(PlayerPrefs.GetInt("Difficulty", 0));

        bool done = false;
        string PlayerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(PlayerID, scoreToUpload, leaderboardID, (response) => {
            if(response.success){
                Debug.Log("Score Submitted Successfully");
                done = true;
            } else {
                Debug.Log("Failed to submit score :" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public int GetLeaderboardID(int difficulty){
        switch (difficulty){
            case 0 : return normalLeaderboardID;
            case 1 : return hardLeaderboardID;
            case 2 : return insaneLeaderboardID;
        }
        return 0;
    }

    private bool fetching = false;

    public IEnumerator FetchTopHighscoresRoutine(int leaderboardID){

        //Ensures that only one copy of this function can run at a time (was causing multiple copies of scores to be posted to leaderboard)
        if(fetching){
            yield break;
        }

        fetching = true;
        
        bool done = false;

        WipeScores(out done);

        yield return new WaitWhile(() => done == false);

        currentLeaderboardID = leaderboardID;

        //int leaderboardID = GetLeaderboardID(PlayerPrefs.GetInt("Difficulty", 0));
        done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 2000, 0, (response) => {
            if(response.success){

                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++){
                    string playerName = "";
                    if(members[i].player.name != ""){
                        playerName += members[i].player.name;
                    } else {
                        playerName += "Player " + members[i].player.id;
                    }
                    string playerScore = members[i].score.ToString();

                    bool isMe = ComparePlayers(members[i], leaderboardID);

                    SpawnScore(playerName, playerScore, isMe, members[i].rank.ToString());

                }
                done = true;

            } else {
                Debug.Log("Failed to get scores :" + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);

        fetching = false;
    }

    private bool ComparePlayers(LootLockerLeaderboardMember member, int leaderboardID){
        
        if(member.player.name != PlayerPrefs.GetString("Name")) return false;

        int highscore = GetPlayerHighScore(leaderboardID);
        Debug.Log("Highscore : " +highscore);
        Debug.Log("Member score : " + member.score);
        if(member.score != highscore) return false;
        Debug.Log("ITS ME");
        return true;
    }

    private int GetPlayerHighScore(int leaderboardID){
        if(leaderboardID == normalLeaderboardID){
            Debug.Log("Normal");
            return PlayerPrefs.GetInt("HighscoreNormal", 0);
        } else if (leaderboardID == hardLeaderboardID){
            Debug.Log("Hard");
            return PlayerPrefs.GetInt("HighscoreHard", 0);
        } else {
            Debug.Log("Insane");
            return PlayerPrefs.GetInt("HighscoreInsane", 0);
        }
        
    }

    private List<GameObject> spawnedScores = new List<GameObject>();

    private void SpawnScore(string name, string score, bool isMe, string rank){
        GameObject spawnedScore = Instantiate(scorePrefab, transform.position, transform.rotation, scoresContainer);
        spawnedScores.Add(spawnedScore);
        spawnedScore.GetComponent<PlayerScoreObject>().SetText(rank, name, score, isMe);
    }

    private void WipeScores(out bool done){
        foreach(GameObject score in spawnedScores){
            Destroy(score);
        }
        foreach(Transform score in scoresContainer){
            if(score == null)continue;
            Destroy(score.gameObject);
        }
        spawnedScores.Clear();
        done = true;
    }

    private int currentLeaderboardID;

    public void SetCurrentLeaderboard(){
        switch (PlayerPrefs.GetInt("Difficulty", 0)){
            case 0 : SetNormalLeaderboard();break;
            case 1 : SetHardLeaderboard();break;
            case 2 : SetInsaneLeaderboard();break;
        }
    }

    public void NextLeaderboard(){
        if(currentLeaderboardID == normalLeaderboardID){
            SetHardLeaderboard();
        } else if (currentLeaderboardID == hardLeaderboardID){
            SetInsaneLeaderboard();
        } else if(currentLeaderboardID == insaneLeaderboardID){
            SetNormalLeaderboard();
        }
    }
    public void PreviousLeaderboard(){
        if(currentLeaderboardID == normalLeaderboardID){
            SetInsaneLeaderboard();
        } else if (currentLeaderboardID == hardLeaderboardID){
            SetNormalLeaderboard();
        } else if(currentLeaderboardID == insaneLeaderboardID){
            SetHardLeaderboard();
        }
    }

    [SerializeField] private TextMeshProUGUI leaderboardTitle;
    
    private void SetNormalLeaderboard(){
        leaderboardTitle.text = "Highscores : Normal";
        StartCoroutine(FetchTopHighscoresRoutine(normalLeaderboardID));
    }

    private void SetHardLeaderboard(){
        leaderboardTitle.text = "Highscores : Hard";
        StartCoroutine(FetchTopHighscoresRoutine(hardLeaderboardID));
    }

    private void SetInsaneLeaderboard(){
        leaderboardTitle.text = "Highscores : Insane";
        StartCoroutine(FetchTopHighscoresRoutine(insaneLeaderboardID));
    }
}
