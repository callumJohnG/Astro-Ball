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
    [SerializeField] private int continuePrice;
    [SerializeField] private int continueCostIncrement;

    [SerializeField] private GameObject payButtonCross;
    [SerializeField] private GameObject adButtonCross;

    private int currentCostMultiplier = 0;
    private bool advertAvailable = true;
    private bool paymentAvailable = true;

    public void LoadButtons(){
        //Paying Button
        //CalculateCurrentCost();
        continueCostText.text = (continuePrice + " coins");
        continuePromptText.text = "Pay " + continuePrice + " coins to continue?";

        int playerXP = PlayerXPManager.Instance.GetXP();
        bool canPay = continuePrice <= playerXP;
        continueButton.interactable = canPay && paymentAvailable;
        payButtonCross.SetActive(!(canPay && paymentAvailable));


        bool canPlayAd = advertAvailable && rewardedAdsButton.adLoaded;
        adButtonCross.SetActive(!canPlayAd);
        advertButton.interactable = canPlayAd;
    }

    public void ResetButtons(){
        currentCostMultiplier = 0;
        advertAvailable = true;
        paymentAvailable = true;
    }

    public void PurchaseContinue(){
        //CalculateCurrentCost();
        if(!PlayerXPManager.Instance.SpendXP(continuePrice))return;
        //currentCostMultiplier ++;
        paymentAvailable = false;
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
