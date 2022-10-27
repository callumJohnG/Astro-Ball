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
        CycleColourPalette();
    }

    private int paletteIndex = 0;

    public void SetRandomColourPalette(){
        SetColourPalette(colourPalettes[Random.Range(0, colourPalettes.Count)]);
    }

    public void CycleColourPalette(){
        paletteIndex++;
        if(paletteIndex >= colourPalettes.Count)paletteIndex = 0;

        SetColourPalette(colourPalettes[paletteIndex]);
    }

    private void SetColourPalette(ColourPaletteData paletteData){
        material.SetVector("_HighPassCol", paletteData.highPassCol);
        material.SetVector("_HighMidPassCol", paletteData.highMidPassCol);
        material.SetVector("_MidPassCol", paletteData.midPassCol);
        material.SetVector("_LowMidPassCol", paletteData.lowMidPassCol);
        material.SetVector("_LowPassCol", paletteData.lowPassCol);
    }

}
