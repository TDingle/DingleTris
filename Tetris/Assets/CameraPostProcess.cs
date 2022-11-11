using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProcess : MonoBehaviour
{
    public SingleBlock level;
    public Material material;
    private void Awake()
    {
        SetPalette(10);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);

    }
    Color[,] palettes =
    {
        {new Color(240,240,179), new Color(244,147,110), new Color(232,80,111), new Color(129,29,95) },
        {new Color(255,255,199), new Color(212,152,74), new Color(78,73,76), new Color(0,48,59) },
        {new Color(255,255,255), new Color(74,237,255), new Color(255,138,205), new Color(176,62,128) }

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
            Debug.Log(colVec);
            material.SetVector("_color" + i.ToString(), colVec);
        }

    }
}
