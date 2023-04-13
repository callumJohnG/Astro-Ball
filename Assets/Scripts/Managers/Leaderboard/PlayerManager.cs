using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField playerNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    public void SetPlayerName(){
        StartCoroutine(SetPlayerNameRoutine());
    }

    private IEnumerator SetPlayerNameRoutine(){
        bool done = false;
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) => {
            if(response.success){
                PlayerPrefs.SetString("Name", playerNameInputField.text);
                Debug.Log("Changed name successfully");
                done = true;
            } else {
                Debug.Log("Failed to change name :" + response.Error);
                done = true;
            }
        });
        
        yield return new WaitWhile(() => done == false);

        Leaderboard.Instance.SetCurrentLeaderboard();
    }

    IEnumerator LoginRoutine(){
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>{
            if(response.success){
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            } else {
                Debug.Log("Could not start session : " + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
        done = false;

        LootLockerSDKManager.GetPlayerName((response) => {
            if(response.success){
                PlayerPrefs.SetString("Name", response.name);
                playerNameInputField.text = response.name;
                done = true;
            } else {
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);

        Leaderboard.Instance.SetCurrentLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
