using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Menu_Info : MonoBehaviour
{
    [SerializeField] public Text infoText;

    public void WriteData(Character character)
    {
        string text = character.Base.Name + " LvL: " + character.Level.ToString() + " XP: " + character.XP.ToString() + " HP: " + character.HP.ToString() + "/" + character.GetStat(Stat.MaxHP).ToString();
        infoText.text = text;
    }
}
