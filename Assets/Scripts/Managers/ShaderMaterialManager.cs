using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShaderMaterialManager : MonoBehaviour
{

    [SerializeField] private Material material;
    [field:SerializeField] public List<ColourPaletteData> colourPalettes {get; private set;}
    private const string PALLETTE_KEY = "CurrentPalette";
    private ColourPaletteData currentPalette;

    // Start is called before the first frame update
    void Awake()
    {
        SortList();
        InitialiseMaterial();
    }

    private void SortList(){
        List<ColourPaletteData> tempList = new List<ColourPaletteData>();
        List<ColourPaletteData> cloneList = colourPalettes;
        
        //Put all the free ones at the front
        for(int i = 0; i < cloneList.Count; i++){
            ColourPaletteData data = cloneList[i];
            if(data.free){
                tempList.Add(data);
                cloneList.Remove(data);
                i--;
            }
        }

        //Sort the rest of the list
        List<ColourPaletteData> sortedList = cloneList.OrderBy(o=>o.price).ToList();
    
        //Then concatonate them
        tempList.AddRange(sortedList);

        colourPalettes = tempList;
    }

    private void InitialiseMaterial(){
        //WIPES THE CURRENT PALETTE AT THE START OF EACH PLAY
        //DONT FORGET THIS IS HERE - ITS JUST A TEST
        //PlayerPrefs.SetString(PALLETTE_KEY, "");

        string targetName = PlayerPrefs.GetString(PALLETTE_KEY, "");

        foreach(ColourPaletteData data in colourPalettes){
            if(data.name != targetName)continue;

            SetColourPalette(data);
            return;
        }

        SetColourPalette(colourPalettes[0]);
    }

    public ColourPaletteData GetColourPalette(){
        return currentPalette;
    }

    public void SetColourPalette(ColourPaletteData paletteData){
        material.SetVector("_HighPassCol", paletteData.highPassCol);
        material.SetVector("_HighMidPassCol", paletteData.highMidPassCol);
        material.SetVector("_MidPassCol", paletteData.midPassCol);
        material.SetVector("_LowMidPassCol", paletteData.lowMidPassCol);
        material.SetVector("_LowPassCol", paletteData.lowPassCol);

        currentPalette = paletteData;

        PlayerPrefs.SetString(PALLETTE_KEY, paletteData.name);
    }

    public void SetPixelation(float value){
        material.SetFloat("_Pixelation", value);
    }

}
