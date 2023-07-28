using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class StatsUpdate : MonoBehaviour
{
    public TMP_Text armorText;
    public TMP_Text damageText;
    public TMP_Text atkSpdText;
    public TMP_Text healthText;
    public CharacterAttributes attributes;

    // initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        armorText.text = attributes.Armor.ToString();
        damageText.text = attributes.AttackDamage.ToString();
        atkSpdText.text = attributes.AttackSpeed.ToString();
        healthText.text = attributes.MaxHealth.ToString();
    }
}
