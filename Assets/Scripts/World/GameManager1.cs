using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gamestate
{
    FreeRoam, Battle, Paused, InfoBox, Shop
}

public class GameManager1 : MonoBehaviour
{
    Gamestate state;
    [SerializeField] PlayerMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] PauseMenu pause_Menu;
    [SerializeField] Camera worldCamera;
    [SerializeField] GameObject endscreen;
    [SerializeField] FightEndScreen matchEndScreen;
    [SerializeField] WorldInfoBox infoBox;
    [SerializeField] CollectibleUI inGameUI;

    [SerializeField] List<Shop> shops;
    [SerializeField] ShopItemMenu itemShopMenu;
    [SerializeField] ShopEquipmentMenu equipmentShopMenu;
    Shoptype shopType;

    [SerializeField] List<EnemyMovement> enemies;
    [SerializeField] List<Chest> chests;

    [SerializeField] EnemyMovement boss;

    public AudioClip worldClip;
    public AudioClip endFailClip;

    public event Action<AudioClip,bool> StartMusic;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;

        if (battleSystem)
        {
            battleSystem.OnBattleOver += EndBattle;
        }

        if (matchEndScreen)
        {
            matchEndScreen.OnBattleOver += EndBattle;
        }

        pause_Menu.UnPause += UnPause;

        if (itemShopMenu)
        {
            itemShopMenu.OpenShop += OpenShop;
            itemShopMenu.CloseShop += CloseShop;
        }

        if (equipmentShopMenu)
        {
            equipmentShopMenu.OpenShop += OpenShop;
            equipmentShopMenu.CloseShop += CloseShop;
        }

        if (infoBox)
        {
            infoBox.Show += DisplayInfoBox;
            infoBox.Hide += HideInfoBox;
        }

        StartMusic(worldClip,true);
    }

    void UnPause()
    {
        state = Gamestate.FreeRoam;
        pause_Menu.gameObject.SetActive(false);
        if (inGameUI)
        {
            inGameUI.Show(true);
        }
    }

void DisplayInfoBox()
    {
        state = Gamestate.InfoBox;
    }

    void HideInfoBox()
    {
        state = Gamestate.FreeRoam;
    }

    void OpenShop(Shoptype type)
    {
        shopType = type;
        state = Gamestate.Shop;
    }

    void CloseShop(Shoptype type)
    {
        state = Gamestate.FreeRoam;
    }

    void StartBattle(EnemyType type)
    {
        if (inGameUI)
        {
            inGameUI.Show(false);
        }
        battleSystem.gameObject.SetActive(true);

        MapArea enemyList = gameObject.GetComponent<MapArea>();
        battleSystem.StartBattle(enemyList, type);

        worldCamera.gameObject.SetActive(false);
        state = Gamestate.Battle;
    }

    void EndBattle(bool won)
    {
        state = Gamestate.FreeRoam;
        if (won == false)
        {
            Time.timeScale = 0f;
            endscreen.SetActive(true);
            StartMusic(endFailClip,false);
        }
        else if (won == true)
        {
            battleSystem.gameObject.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            StartMusic(worldClip,true);
            if (inGameUI)
            {
                inGameUI.Show(true);
            }
        }
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetNewPoint();
        }
    }

    private void Update()
    {
        if (state == Gamestate.FreeRoam)
        {
            playerController.HandleUpdate();
            if (boss != null)
            {
                boss.HandleUpdate();
            }

            for (int i = 0; i < enemies.Count; ++i)
            {
                if (enemies[i] && enemies[i].gameObject.active)
                {
                    enemies[i].HandleUpdate();
                }
            }

            for (int i = 0; i < shops.Count; ++i)
            {
                shops[i].HandleUpdate();
            }

            for (int i = 0; i < chests.Count; ++i)
            {
                chests[i].HandleUpdate();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = Gamestate.Paused;
                if (inGameUI)
                {
                    inGameUI.Show(false);
                }
                pause_Menu.gameObject.SetActive(true);
                pause_Menu.Setup();
            }
        }
        else if (state == Gamestate.Shop)
        {
            if (shopType == Shoptype.Items)
            {
                itemShopMenu.HandleUpdate();
            }
            else if (shopType == Shoptype.Equipment)
            {
                equipmentShopMenu.HandleUpdate();
            }
        }
        else if (state == Gamestate.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == Gamestate.Paused)
        {
            pause_Menu.HandleUpdate();
        }
        else if (state == Gamestate.InfoBox)
        {
            infoBox.HandleUpdate();
        }
    }
}
