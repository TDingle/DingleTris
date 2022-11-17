using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProcess : MonoBehaviour
{
    public SingleBlock level;
    public Material material;
    private void Update()
    {
        SetPalette(level.currentLevel) ;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);

    }
    Color[,] palettes =
    {
        {new Color(240,240,179), new Color(244,147,110), new Color(232,80,111), new Color(129,29,95) },
        {new Color(255,230,234), new Color(230,161,207), new Color(77,77,128), new Color(19,22,38) },
        {new Color(255,255,199), new Color(212,152,74), new Color(78,73,76), new Color(0,48,59) },
        {new Color(229,216,172), new Color(125,179,171), new Color(124,113,74), new Color(38,75,56) },
        {new Color(244,245,233), new Color(168,191,76), new Color(68,42,140), new Color(9,9,26) },
        {new Color(255,221,201), new Color(255,93,24), new Color(167,20,66), new Color(9,13,33) },
        {new Color(212,201,195), new Color(211,174,33), new Color(99,86,80), new Color(28,20,18) }

    };
    public void SetPalette(int currentLevel)
    { 

        int paletteIndex = currentLevel / 5;
        for (int i = 1; i <= 4; i++)
        {
            Color col = palettes[paletteIndex, i-1];
            Vector4 colVec = new Vector4(col.r, col.g, col.b);
            colVec.x = colVec.x / 255.0f;
            colVec.y = colVec.y / 255.0f;
            colVec.z = colVec.z / 255.0f;
           
            material.SetVector("_color" + i.ToString(), colVec);
        }

    }
}
