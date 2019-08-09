using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_LevelCreator : MonoBehaviour
{
    public Color lastColor;
    public Color actualColor;

    void Start()
    {
        lastColor = new Color(0,0,0,1);
        actualColor = lastColor;
    }
    public void SetColor(Color color)
    {
        lastColor = GetColor();
        actualColor = color;
    }
    public Color GetColor()
    {
        return actualColor;
    }
    public Color32 GetColor32()
    {
        return actualColor;
    }
}
