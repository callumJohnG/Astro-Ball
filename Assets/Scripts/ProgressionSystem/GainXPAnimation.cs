using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainXPAnimation : MonoBehaviour
{

    [SerializeField] private float maxAnimTime;
    [SerializeField] private float maxTimePerCoin;
    [SerializeField] private float startWaitTime;
    private float timePerCoin;

    [SerializeField] private TextMeshProUGUI gainedXPText;
    [SerializeField] private TextMeshProUGUI totalXPText;
    private int gainedXP;
    private int totalXP;

    public void SetUpValues(int gainedXP, int totalXP){
        this.gainedXP = gainedXP;
        this.totalXP = totalXP;

        gainedXPText.text = "+" + gainedXP.ToString();
        totalXPText.text = totalXP.ToString();

        //Calculate the time between coin tics
        float totalTime = gainedXP * maxTimePerCoin;
        if(totalTime <= maxAnimTime){
            timePerCoin = maxTimePerCoin;
        } else {
            timePerCoin = maxAnimTime / gainedXP;
        }
    }    

    public void PlayXPAnimation(){
        StartCoroutine(XPAnimation());
    }

    public void StopXPAnimation(){
        StopCoroutine(XPAnimation());
    }

    private IEnumerator XPAnimation(){
        yield return new WaitForSeconds(startWaitTime);

        for(int i = 1; i <= gainedXP; i++){
            gainedXPText.text = (gainedXP - i).ToString() + "+";
            totalXPText.text = (totalXP + i).ToString();
            //Play some soundeffect here
            AudioManager.Instance.PlayCoin();

            yield return new WaitForSeconds(timePerCoin);
        }
    }
}
