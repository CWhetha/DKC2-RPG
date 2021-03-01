using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayerParty : MonoBehaviour
{
    [SerializeField] List<Character> characters;
    [SerializeField] List<Item> items;
    [SerializeField] List<Armor> armorInventory;
    [SerializeField] List<Weapon> weaponInventory;
    [SerializeField] int startBananas;
    [SerializeField] int startBp;
    [SerializeField] int maxBp;
    [SerializeField] int KongLevels;

    [SerializeField] Vector3 position;


    public void Awake()
    {
        for (int i = 0; i < characters.Count; ++i)
        {
            characters[i].Init();
        }

        KongLettersManifest.Init(KongLevels);
        PlayerParty.Characters = characters;
        PlayerParty.Items = items;
        PlayerParty.Bananas = startBananas;
        PlayerParty.BP = startBp;
        PlayerParty.MaxBP = maxBp;
        PlayerParty.ArmorInventory = armorInventory;
        PlayerParty.WeaponInventory = weaponInventory;
        PlayerParty.Position = position;
    }
}

