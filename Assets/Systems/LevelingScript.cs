using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelingScript : MonoBehaviour
{
    public TextMesh ArmorTotal;
    public TextMesh DamageTotal;
    public TextMesh HealthTotal;
    public TextMesh SpeedTotal;

    public void ButtonClick()
    {
        print("click");
    }

    public void OnClick()
    {
        print("click");
    }

    public void UpdateText (TextMesh textToChange, int value)
    {
        textToChange.text = value.ToString();
    }
}