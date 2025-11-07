using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tenkoku.Core;  

public class TenkokuUIBridge : MonoBehaviour
{
    public TenkokuModule tenkoku;

    public void SetRain(float value)
    {
        tenkoku.weather_RainAmt = value;  
    }

    public void SetTime(float value)
    {
        tenkoku.currentHour = Mathf.RoundToInt(value);
    }
}
