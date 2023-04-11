using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColourSchemeShopButton : MonoBehaviour
{
    private ColourPaletteData myPalette;
    [SerializeField] private List<TextMeshProUGUI> nameTexts;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject ownedContainer;
    [SerializeField] private GameObject notOwnedContainer;
    [SerializeField] private GameObject selectedContainer;
    private bool owned;

    public void SetPalette(ColourPaletteData data){
        myPalette = data;
    }

    public ColourPaletteData GetPalette(){
        return myPalette;
    }

    public void InitialiseButton(){
        foreach(TextMeshProUGUI text in nameTexts)text.text = myPalette.paletteName;
        priceText.text = myPalette.price.ToString();

        //Check if we are "free"
        //Or if we have been purchased
        owned = ShopManager.Instance.CheckPaletteOwned(myPalette);
        SetOwned(owned);
    }

    private void SetOwned(bool owned){
        ownedContainer.SetActive(owned);
        notOwnedContainer.SetActive(!owned);
    }

    public void Pressed(){
        //We have been pressed in the shop

        AudioManager.Instance.PlayButtonClick();
        
        if(ShopManager.Instance.CheckPaletteOwned(myPalette)){
            ShopManager.Instance.SelectPalette(myPalette);
        } else {
            //Open the purchase screen
            ShopManager.Instance.OpenPurchaseScreen(myPalette);
        }   
    }

    public void SetSelected(bool selected){
        selectedContainer.SetActive(selected);
    }
}
