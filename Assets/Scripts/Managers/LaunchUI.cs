using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaunchUI : MonoBehaviour
{

    public static LaunchUI Instance {get; private set;}
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TextMeshProUGUI launchCountText;

    public void SetText(string text){
        launchCountText.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
