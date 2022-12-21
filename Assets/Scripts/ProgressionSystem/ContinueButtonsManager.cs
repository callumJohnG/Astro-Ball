using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinueButtonsManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueCostText;
    [SerializeField] private Button advertButton;
    [SerializeField] private RewardedAdsButton rewardedAdsButton;
    [SerializeField] private int baseContinueCost;
    [SerializeField] private int continueCostIncrement;
    private int currentCostMultiplier = 0;
    private int currentCost;
    private bool advertAvailable = true;

    public void LoadButtons(){
        //Paying Button
        currentCost = baseContinueCost + (continueCostIncrement * currentCostMultiplier);
        continueCostText.text = (currentCost + " coins");

        int playerXP = PlayerXPManager.Instance.GetXP();
        continueButton.interactable = currentCost <= playerXP;

    }

    public void ResetButtons(){
        currentCostMultiplier = 0;
        advertAvailable = true;
    }

    public void PurchaseContinue(){
        if(!PlayerXPManager.Instance.SpendXP(currentCost))return;
        currentCostMultiplier ++;

        //We spent the points
        ContinueGame();
    }

    public void ClickOnAdvert(){
        if(!advertAvailable)return;
        
        rewardedAdsButton.ShowAd();
        advertAvailable = false;
    }

    public void ContinueGame(){
        GameplayManager.Instance.ContinueGame();
    }
}
