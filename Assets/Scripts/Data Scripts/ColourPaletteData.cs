using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColourPaletteData", menuName = "ScriptableObjects/ColourPaletteData", order = 1)]
public class ColourPaletteData : ScriptableObject
{
    public Color highPassCol;
    public Color highMidPassCol;
    public Color midPassCol;
    public Color lowMidPassCol;
    public Color lowPassCol;

    public int price = 200;
    public bool free;
}
