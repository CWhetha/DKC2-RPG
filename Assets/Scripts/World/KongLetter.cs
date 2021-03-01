using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongLetter : MonoBehaviour
{
    public Letter letter; 
    public int level;

    public event Action<bool> ShowUI;

    private void Start()
    {
        if (KongLettersManifest.KongLetters[level].getLetter(letter))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            KongLettersManifest.KongLetters[level].collectLetter(letter);
            if (KongLettersManifest.KongLetters[level].collectedAll())
            {
                Bonus();
                ShowUI(true);
            }
            {
                ShowUI(false);
            }
            gameObject.SetActive(false);
        }
    }
    private void Bonus()
    {
        PlayerParty.MaxBP += 3;
        if (PlayerParty.BP + 6 > PlayerParty.MaxBP)
        {
            PlayerParty.BP = PlayerParty.MaxBP;
        }
        else
        {
            PlayerParty.BP += 6;
        }
    }
}
