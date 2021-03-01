using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text infoText;
    private Character _character;

    public void SetData(Character character)
    {
        _character = character;
        infoText.text = character.Base.Name + " HP: " + character.HP + "/" + character.MaxHp;
    }

    public void UpdateStats()
    {
        infoText.text = _character.Base.Name + " HP: " + _character.HP + "/" + _character.MaxHp;
    }
}
