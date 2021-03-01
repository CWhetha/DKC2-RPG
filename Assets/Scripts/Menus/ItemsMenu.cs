using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemsState
{
    Items, Use
}
public class ItemsMenu : MonoBehaviour
{
    private int currentItemSelection = 0;
    private int currentCharacterSelection = 0;

    public List<Text> itemTexts;
    public List<Text> characterTexts;

    int itemStart = 0;
    int useStart = 0;

    [SerializeField] Text description;
    [SerializeField] Text hpHeal;
    [SerializeField] Text bpRestore;
    [SerializeField] Text targets;
    [SerializeField] Text itemType;
    [SerializeField] Text effects;
    [SerializeField] Text amount;
    [SerializeField] Text useInMenu;

    ItemsState state;
    public Color highlightedColor;
    public Color invalidColor;
    [SerializeField] List<ItemMenuStats> stats;
    [SerializeField] Text partyBP;

    public void Show(bool show)
    {
        gameObject.SetActive(show);
        UpdateItemSelection(currentItemSelection);
        for (int i = 0; i < stats.Count; ++i)
        {
            stats[i].ClearCharacterBox();
        }
    }

    public bool HandleUpdate(List<Character> characters, List<Item> items)
    {
        if (state == ItemsState.Use)
        {
            if (items[currentItemSelection].Base.Targets == Targets.Allies)
            {
                HandleAllUse(characters, items[currentItemSelection]);
            }
            else if (items[currentItemSelection].Base.Targets == Targets.Ally || items[currentItemSelection].Base.Targets == Targets.AllyFainted)
            {
                HandleUseSelection(characters, items[currentItemSelection]);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = ItemsState.Items;
                ClearCharacters();
                for (int i = 0; i < stats.Count; ++i)
                {
                    stats[i].ClearCharacterBox();
                }
            }
        }
        else
        {
            HandleItemSelection(items);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }
        }
        return false;
    }

    public void HandleItemSelection(List<Item> items)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentItemSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentItemSelection;
        }
        currentItemSelection = Mathf.Clamp(currentItemSelection, 0, items.Count - 1);
        UpdateItemBox(items[currentItemSelection]);
        if (items.Count > 4 && currentItemSelection > 3)
        {
            itemStart = currentItemSelection - 3;
            SetItemNames(items, itemStart);
        }
        else
        {
            itemStart = 0;
            SetItemNames(items, itemStart);
        }
        int move = currentItemSelection > 3 ? 3 : currentItemSelection;
        UpdateItemSelection(move);

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = ItemsState.Use;
        }
    }

    public void HandleAllUse(List<Character> characters, Item item)
    {
        SetCharacterNames(characters, useStart);
        UpdateUseAllSelection();
        if (Input.GetKeyDown(KeyCode.E) && item.Base.IsUsedInMenu)
        {
            if (item.Quantity > 0)
            {
                if (item.Base.Type == ItemType.BP || item.Base.Type == ItemType.Both)
                {
                    int restore = item.Base.BpGain;
                    if (item.Base.IsFullBP)
                    {
                        PlayerParty.BP = PlayerParty.MaxBP;
                    }
                    else
                    {
                        if (PlayerParty.BP + restore >= PlayerParty.MaxBP)
                        {
                            PlayerParty.BP = PlayerParty.MaxBP;
                        }
                        else
                        {
                            PlayerParty.BP += restore;
                        }
                    }
                }
                item.UseItem(1);
                for (int i = 0; i < characters.Count; ++i)
                {
                    if (characters[i].HP != 0)
                    {
                        characters[i].TakeItemHeal(item);
                    }
                }
                UpdateItemBox(item);
                SetCharacterNames(characters, useStart);
                SetBPText();
            }
        }
    }

    public void HandleUseSelection(List<Character> characters, Item item)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentCharacterSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentCharacterSelection;
        }
        currentCharacterSelection = Mathf.Clamp(currentCharacterSelection, 0, characters.Count - 1);
        if (characters.Count > 4 && currentCharacterSelection > 3)
        {
            useStart = currentCharacterSelection - 3;
            SetCharacterNames(characters, useStart);
        }
        else
        {
            useStart = 0;
            SetCharacterNames(characters, useStart);
        }
        int move = currentCharacterSelection > 3 ? 3 : currentCharacterSelection;
        UpdateUseSelection(currentCharacterSelection);

        if (Input.GetKeyDown(KeyCode.E) && item.Base.IsUsedInMenu)
        {
            if (item.Quantity > 0)
            {
                if ((item.Base.Targets == Targets.Ally && characters[currentCharacterSelection].HP != 0) || (item.Base.Targets == Targets.AllyFainted && characters[currentCharacterSelection].HP == 0))
                {
                    item.UseItem(1);
                    characters[currentCharacterSelection].TakeItemHeal(item);
                    UpdateItemBox(item);
                    SetCharacterNames(characters, useStart);
                }
            }
        }
    }

    private void UpdateItemBox(Item item)
    {
        description.text = "Description: " + item.Base.Description;
        if (item.Base.Type == ItemType.HP || item.Base.Type == ItemType.Both)
        {
            if (item.Base.IsFullHP)
            {
                hpHeal.text = "Heals: Full HP";
            }
            else
            {
                hpHeal.text = "Heals: " + item.Base.HpGain.ToString() + " HP";
            }
        }
        else
        {
            hpHeal.text = "";
        }

        if (item.Base.Type == ItemType.BP || item.Base.Type == ItemType.Both)
        {
            if (item.Base.IsFullBP)
            {
                bpRestore.text = "Restores: Full BP";
            }
            else
            {
                bpRestore.text = "Restores: " + item.Base.BpGain.ToString() + " BP";
            }
        }
        else
        {
            bpRestore.text = "";
        }
        targets.text = "Targets: " + item.Base.Targets.ToString();

        if (item.Base.Type == ItemType.Both)
        {
            itemType.text = "Targets: HP/BP";
        }
        else
        {
            itemType.text = "Effects:" + item.Base.Type.ToString();
        }

        if (item.Base.Type == ItemType.Buff)
        {
            effects.text = "Stats Buffed: \n";
            for (int i = 0; i < item.Base.Effects.Boosts.Count; ++i)
            {
                effects.text += item.Base.Effects.Boosts[i].stat.ToString() + "\n";
            }
        }
        else
        {
            effects.text = "";
        }

        amount.text = "Amount: " + item.Quantity.ToString();
        useInMenu.text = "Can Use in Menu: " + item.Base.IsUsedInMenu.ToString();
    }



    private void UpdateItemSelection(int selectedAction)
    {
        for (int i = 0; i < itemTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                itemTexts[i].color = highlightedColor;
            }
            else
            {
                itemTexts[i].color = Color.white;
            }
        }
    }
    private void UpdateUseSelection(int selectedAction)
    {
        for (int i = 0; i < characterTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                characterTexts[i].color = highlightedColor;
            }
            else
            {
                characterTexts[i].color = Color.white;
            }
        }
    }

    private void UpdateUseAllSelection()
    {
        for (int i = 0; i < characterTexts.Count; ++i)
        {
            characterTexts[i].color = highlightedColor;
        }
    }

    public void SetItemNames(List<Item> items, int start)
    {
        for (int i = 0; i < itemTexts.Count; ++i)
        {
            if (i < items.Count)
            {
                itemTexts[i].text = items[i + start].Base.Name;
            }
            else
            {
                itemTexts[i].text = "";
            }
        }
    }

    public void SetBPText()
    {
        partyBP.text = "BP: " + PlayerParty.BP + "/" + PlayerParty.MaxBP;
    }

    public void SetCharacterNames(List<Character> characters, int start)
    {
        for (int i = 0; i < characterTexts.Count; ++i)
        {
            if (i < characters.Count)
            {
                characterTexts[i].text = characters[i + start].Base.Name;
                stats[i].UpdateCharacterBox(characters[i + start]);
            }
            else
            {
                characterTexts[i].text = "";
                stats[i].ClearCharacterBox();
            }
        }
    }

    private void ClearCharacters()
    {
        for (int i = 0; i < characterTexts.Count; ++i)
        {
            characterTexts[i].color = Color.black;
        }
    }
}

