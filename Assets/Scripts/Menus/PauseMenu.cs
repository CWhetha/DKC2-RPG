using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PauseState
{
    UnPaused, Paused, Stats, Item, Equipment
}

public class PauseMenu : MonoBehaviour
{
    public string mainMenu;
    public Text bP;
    public Text currency;
    public GameObject pauseMenu;
    public GameObject battle;
    public bool isPaused;
    private int currentSelection = 0;
    public Color highlightedColor;
    public List<Text> actionTexts;

    List<Character> allies;
    List<Item> items;
    PauseState state;

    public GameObject baseMenu;
    public StatsMenu statsMenu;
    public ItemsMenu itemsMenu;
    public EquipmentMenu equipmentMenu;

    public event Action UnPause;

    public void Setup()
    {
        allies = PlayerParty.Characters;
        items = PlayerParty.Items;
        state = PauseState.Paused;
        PrintStats();
        Time.timeScale = 0f;
        UpdateSelection(currentSelection);
    }

    public void HandleUpdate()
    {
        if (state == PauseState.Paused)
        {
            Paused();
        }
        else if (state == PauseState.Stats)
        {
            Stats();
        }
        else if (state == PauseState.Item)
        {
            Items();
        }
        else if (state == PauseState.Equipment)
        {
            Equipment();
        }
    }

    void Stats()
    {
        bool exit = statsMenu.HandleUpdate(allies);
        if (exit)
        {
            baseMenu.SetActive(true);
            statsMenu.Show(false);
            state = PauseState.Paused;
        }
    }

    void Items()
    {
        bool exit = itemsMenu.HandleUpdate(allies, items);
        itemsMenu.SetBPText();
        if (exit)
        {
            baseMenu.SetActive(true);
            itemsMenu.Show(false);
            PrintStats();
            state = PauseState.Paused;
        }
    }

    public void Equipment()
    {
        bool exit = equipmentMenu.HandleUpdate(allies);
        if (exit)
        {
            baseMenu.SetActive(true);
            equipmentMenu.Show(false);
            PrintStats();
            state = PauseState.Paused;
        }
    }

    void Paused()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentSelection < actionTexts.Count - 1)
            {
                ++currentSelection;
                UpdateSelection(currentSelection);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentSelection > 0)
            {
                --currentSelection;
                UpdateSelection(currentSelection);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentSelection == 0)
            {
                Resume();
            }
            else if (currentSelection == 1)
            {
                StartItems();
            }
            else if (currentSelection == 2)
            {
                StartStats();
            }
            else if (currentSelection == 3)
            {
                StartEquipment();
            }
            else if (currentSelection == 4)
            {
                ExitToMain();
            }
        }
    }

    public void PrintStats()
    {
        currency.text = "Bananas: " + PlayerParty.Bananas.ToString();
        bP.text = "BP: " + PlayerParty.BP.ToString() + "/" + PlayerParty.MaxBP.ToString();
    }

    public void Resume()
    {
        UnPause();
        Time.timeScale = 1f;
    }

    public void StartStats()
    {
        baseMenu.SetActive(false);
        statsMenu.Show(true);
        statsMenu.SetCharacterNames(allies);
        state = PauseState.Stats;
    }

    public void StartItems()
    {
        itemsMenu.SetItemNames(items, 0);
        itemsMenu.SetCharacterNames(allies, 0);
        baseMenu.SetActive(false);
        itemsMenu.Show(true);
        state = PauseState.Item;
    }

    public void StartEquipment()
    {
        baseMenu.SetActive(false);
        equipmentMenu.SetCharacterNames(allies, 0);
        equipmentMenu.Show(true);
        state = PauseState.Equipment;
    }

    public void ExitToMain()
    {
        state = PauseState.UnPaused;
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void UpdateSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                actionTexts[i].color = Color.white;
            }
        }
    }
}

