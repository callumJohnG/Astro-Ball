using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColourSchemeShopButton : MonoBehaviour
{
    private ColourPaletteData myPalette;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject cross;

    public void SetPalette(ColourPaletteData data){
        myPalette = data;
    }

    public void InitialiseButton(){
        cross.SetActive(false);
        nameText.text = myPalette.paletteName;
        priceText.text = myPalette.price.ToString();

        //Check if we are "free"
        //Or if we have been purchased
        if(ShopManager.Instance.CheckPaletteOwned(myPalette)){
            priceText.gameObject.SetActive(false);
        } else if(PlayerXPManager.Instance.GetXP() < myPalette.price){
            //Check if we have enough coins
            cross.SetActive(true);
        }
    }

    public void Pressed(){
        //We have been pressed in the shop
        
        if(ShopManager.Instance.CheckPaletteOwned(myPalette)){
            ShopManager.Instance.SelectPalette(myPalette);
            return;
        }

        //Check if player can afford us
        if(PlayerXPManager.Instance.GetXP() >= myPalette.price){
            ShopManager.Instance.OpenPurchaseScreen(myPalette);
        }
    }


}
