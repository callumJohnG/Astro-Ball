using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start(){
        LoadGame();
    }

    //private void SetColourPalette(){}

    private void LoadGame(){
        Debug.Log("Loading Game");
        SceneManager.LoadScene(1);
        
    }
}
