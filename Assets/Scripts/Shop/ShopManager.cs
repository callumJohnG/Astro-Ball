using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    [SerializeField] private GameObject shopButtonPrefab;
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
    }

    private void WipeProgress(){
        Debug.LogError("WIPING THE PROGRESS");
        foreach(ColourPaletteData data in allPalettes){
            PlayerPrefs.SetInt(data.ToString(), 0);
        }
    }

    private void SpawnButtons(){
        foreach(ColourPaletteData data in allPalettes){
            ColourSchemeShopButton button = Instantiate(shopButtonPrefab, transform.position, transform.rotation, shopButtonParent).GetComponent<ColourSchemeShopButton>();
            button.SetPalette(data);
            allButtons.Add(button);
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
    }

    [SerializeField] private GameObject purchaseScreen;
    [SerializeField] private TextMeshProUGUI purchaseText;
    private ColourPaletteData currentPalette;


    public void OpenPurchaseScreen(ColourPaletteData data){
        purchaseScreen.SetActive(true);
        purchaseText.text = "Buy " + data.ToString() + " for " + data.price + " coins?";
        currentPalette = data;
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
