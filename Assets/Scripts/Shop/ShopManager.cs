using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    [SerializeField] private GameObject shopButtonPrefab;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform shopButtonParent;
    private List<ColourPaletteData> allPalettes;
    private List<ColourSchemeShopButton> allButtons = new List<ColourSchemeShopButton>();

    private void Start() {
        allPalettes = shaderMaterialManager.colourPalettes;

        //FOR TESTING ONLY
        //WipeProgress();

        //Spawn all the buttons
        SpawnButtons();

        //Set up all the buttons
        SetUpButtons();

        currentPalette = shaderMaterialManager.GetColourPalette();
        SetButtonSelected(currentPalette);
    }

    private void WipeProgress(){
        Debug.LogError("WIPING THE PROGRESS");
        foreach(ColourPaletteData data in allPalettes){
            PlayerPrefs.SetInt(data.ToString(), 0);
        }
    }

    private void SpawnButtons(){
        /*
        foreach(ColourPaletteData data in allPalettes){
            ColourSchemeShopButton button = Instantiate(shopButtonPrefab, transform.position, transform.rotation, shopButtonParent).GetComponent<ColourSchemeShopButton>();
            button.SetPalette(data);
            allButtons.Add(button);
        }*/

        int paletteCounter = 0;
        //Loop through each palette in the list
        while(paletteCounter < allPalettes.Count){
            //Spawn a new row
            Transform row = Instantiate(rowPrefab, transform.position, transform.rotation, shopButtonParent).transform;
            
            //Spawn 3 buttons in that row
            for(int i = 0; i < 3; i++){
                //Get the next palette
                ColourPaletteData data = allPalettes[paletteCounter];
                
                //Spawn a button and add to list
                ColourSchemeShopButton button = Instantiate(shopButtonPrefab, transform.position, transform.rotation, row).GetComponent<ColourSchemeShopButton>();
                button.SetPalette(data);
                allButtons.Add(button);

                //Increment palette counter (and break if we are done)
                paletteCounter++;
                if(paletteCounter >= allPalettes.Count)break;
            }
        }
    }

    public void SetUpButtons(){
        foreach(ColourSchemeShopButton button in allButtons){
            button.InitialiseButton();
        }
    }


    public bool CheckPaletteOwned(ColourPaletteData data){
        //Check if we have purchased this (with our player prefs)
        bool purchased = PlayerPrefs.GetInt(data.ToString(), 0) == 1;

        //1 == purchased
        //0 == not purchased

        //If its free, its always owned
        return purchased || data.free;

    }
    [SerializeField] private ShaderMaterialManager shaderMaterialManager;

    public void SelectPalette(ColourPaletteData data){
        shaderMaterialManager.SetColourPalette(data);
        SetButtonSelected(data);
    }

    public void SetButtonSelected(ColourPaletteData data){
        selectedButton?.SetSelected(false);

        //Find the button that has this data
        foreach(ColourSchemeShopButton button in allButtons){
            if(button.GetPalette() != data)continue;

            //This button has the palette
            button.SetSelected(true);
            selectedButton = button;
            return;
        }
    }

    [SerializeField] private GameObject purchaseScreen;
    [SerializeField] private TextMeshProUGUI purchaseText;
    [SerializeField] private GameObject notEnoughCoinsContainer;
    [SerializeField] private Button yesButton;
    private ColourPaletteData currentPalette;
    private ColourPaletteData realSelectedPalette;
    private ColourSchemeShopButton selectedButton;

    public void OpenPurchaseScreen(ColourPaletteData data){
        purchaseScreen.SetActive(true);
        purchaseText.text = "Buy " + data.paletteName + " for " + data.price + " coins?";
        currentPalette = data;

        bool canPurchase = PlayerXPManager.Instance.GetXP() >= currentPalette.price;
        yesButton.interactable = canPurchase;
        notEnoughCoinsContainer.SetActive(!canPurchase);

        //So the player can preview the palette - temporarily select the palette
        realSelectedPalette = shaderMaterialManager.GetColourPalette();
        SelectPalette(data);
    }

    public void ResetPalette(){
        SelectPalette(realSelectedPalette);
    }

    public void PurchasePalette(){
        Debug.LogError("PURCHASING PALLETTE");
        PlayerPrefs.SetInt(currentPalette.ToString(), 1);
        PlayerXPManager.Instance.SpendXP(currentPalette.price);
        purchaseScreen.SetActive(false);
        SetUpButtons();
        SelectPalette(currentPalette);
    }
}
