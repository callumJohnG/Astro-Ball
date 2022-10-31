using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderMaterialManager : MonoBehaviour
{

    [SerializeField] private Material material;
    [SerializeField] private List<ColourPaletteData> colourPalettes;

    // Start is called before the first frame update
    void Start()
    {
        SetColourPalette(colourPalettes[PlayerPrefs.GetInt("paletteIndex",0)]);
    }

    private int paletteIndex = 0;

    public void SetRandomColourPalette(){
        SetColourPalette(colourPalettes[Random.Range(0, colourPalettes.Count)]);
    }

    public void CycleColourPalette(){
        paletteIndex = IncrementPaletteIndex();
        SetColourPalette(colourPalettes[paletteIndex]);
    }

    private int IncrementPaletteIndex(){
        int index = PlayerPrefs.GetInt("paletteIndex", 0);
        index ++;
        if(index >= colourPalettes.Count)index = 0;
        PlayerPrefs.SetInt("paletteIndex", index);
        return index;
    }

    private void SetColourPalette(ColourPaletteData paletteData){
        material.SetVector("_HighPassCol", paletteData.highPassCol);
        material.SetVector("_HighMidPassCol", paletteData.highMidPassCol);
        material.SetVector("_MidPassCol", paletteData.midPassCol);
        material.SetVector("_LowMidPassCol", paletteData.lowMidPassCol);
        material.SetVector("_LowPassCol", paletteData.lowPassCol);
    }

}
