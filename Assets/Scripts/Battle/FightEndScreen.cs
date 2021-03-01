using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightEndScreen : MonoBehaviour
{
    [SerializeField] public List<FightEndInfo> bars;
    [SerializeField] public Text XPText;
    [SerializeField] public Text MoneyText;

    public event Action<bool> OnBattleOver;

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Setup(List<Character> characters, int xp, int money)
    {
        XPText.text = "XP: " + xp.ToString();
        MoneyText.text = "Bananas: " + money.ToString();
        for (int i = 0; i < bars.Count; ++i)
        {
            if (i < characters.Count)
            {
                if (characters[i].HP != 0)
                {
                    StartCoroutine(bars[i].WriteData(characters[i], true, xp));
                }
                else
                {
                    StartCoroutine(bars[i].WriteData(characters[i], false, xp));
                }
            }
        }
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Show(false);
            OnBattleOver(true);
        }
    }
}

