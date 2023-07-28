using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FoodUpdate : MonoBehaviour
{
    public TMP_Text foodText;
    public CharacterAttributes food;

    // initialization
    void Start ()
    {
        foodText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        foodText.text = food.PointsOfPower.ToString();
    }
}
