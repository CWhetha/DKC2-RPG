using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuStats : MonoBehaviour
{
    [SerializeField] Text hp;

    public void UpdateCharacterBox(Character character)
    {
        hp.text = "HP: " + character.HP.ToString() + "/" + character.MaxHp.ToString();
    }

    public void ClearCharacterBox()
    {
        hp.text = "";
    }
}
