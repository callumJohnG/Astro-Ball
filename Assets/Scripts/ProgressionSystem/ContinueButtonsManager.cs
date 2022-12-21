using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContinueButtonsManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueCostText;
    [SerializeField] private TextMeshProUGUI continuePromptText;
    [SerializeField] private Button advertButton;
    [SerializeField] private RewardedAdsButton rewardedAdsButton;
    [SerializeField] private int baseContinueCost;
    [SerializeField] private int continueCostIncrement;

    [SerializeField] private GameObject payButtonCross;
    [SerializeField] private GameObject adButtonCross;

    private int currentCostMultiplier = 0;
    private int currentCost;
    private bool advertAvailable = true;

    public void LoadButtons(){
        //Paying Button
        Debug.LogError("loading buttons");
        currentCost = baseContinueCost + (continueCostIncrement * currentCostMultiplier);
        continueCostText.text = (currentCost + " coins");
        continuePromptText.text = "Pay " + currentCost + " coins to continue?";

        int playerXP = PlayerXPManager.Instance.GetXP();
        bool canPay = currentCost <= playerXP;
        continueButton.interactable = canPay;
        payButtonCross.SetActive(!canPay);
        adButtonCross.SetActive(!advertAvailable);

        Debug.LogError(currentCost);

    }

    public void ResetButtons(){
        currentCostMultiplier = 0;
        advertAvailable = true;
        Debug.LogError("Reset the buttons");
    }

    public void PurchaseContinue(){
        if(!PlayerXPManager.Instance.SpendXP(currentCost))return;
        currentCostMultiplier ++;
        Debug.LogError("Spent Coins");

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
