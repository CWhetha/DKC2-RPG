using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemMenu : MonoBehaviour
{
    List<ItemBase> itemManifest;

    public List<Text> buttonTexts;

    [SerializeField] Text money;
    [SerializeField] Text description;
    [SerializeField] Text cost;
    [SerializeField] Text hpFill;
    [SerializeField] Text bpFill;
    [SerializeField] Text targets;
    [SerializeField] Text effects;
    [SerializeField] Text amountOwned;

    public Color highlightedColor;

    public event Action<Shoptype> OpenShop;
    public event Action<Shoptype> CloseShop;
    int currentItemSelection;
    int itemStart;

    public void LoadShop(List<ItemBase> items)
    {
        itemManifest = items;
        currentItemSelection = 0;
        itemStart = 0;
        OpenShop(Shoptype.Items);
        gameObject.SetActive(true);
    }

    public void HandleUpdate()
    {
        HandleItemSelection();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Return();
        }
    }
    public void Return()
    {
        gameObject.SetActive(false);
        CloseShop(Shoptype.Items);
    }

    public void HandleItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentItemSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentItemSelection;
        }
        currentItemSelection = Mathf.Clamp(currentItemSelection, 0, itemManifest.Count - 1);
        RedrawItemInfo(itemManifest[currentItemSelection]);
        if (itemManifest.Count > buttonTexts.Count && currentItemSelection > buttonTexts.Count - 1)
        {

            itemStart = currentItemSelection - (buttonTexts.Count - 1);
            SetItemNames(itemStart);
        }
        else
        {
            itemStart = 0;
            SetItemNames(itemStart);
        }
        int move = currentItemSelection > buttonTexts.Count - 1 ? buttonTexts.Count - 1 : currentItemSelection;
        UpdateItemSelection(move);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerParty.Bananas >= itemManifest[currentItemSelection].Cost)
            {
                PlayerParty.GiveItem(itemManifest[currentItemSelection], 1);
                PlayerParty.Bananas -= itemManifest[currentItemSelection].Cost;
                RedrawItemInfo(itemManifest[currentItemSelection]);
            }
        }
    }

    void RedrawItemInfo(ItemBase item)
    {
        money.text = "Bananas: " + PlayerParty.Bananas.ToString();
        description.text = item.Description;
        if (item.Type == ItemType.HP || item.Type == ItemType.Both)
        {
            if (item.IsFullHP)
            {
                hpFill.text = "Heals: Full HP";
            }
            else
            {
                hpFill.text = "Heals: " + item.HpGain.ToString() + " HP";
            }
        }
        else
        {
            hpFill.text = "";
        }

        if (item.Type == ItemType.BP || item.Type == ItemType.Both)
        {
            if (item.IsFullBP)
            {
                bpFill.text = "Restores: Full BP";
            }
            else
            {
                bpFill.text = "Restores: " + item.BpGain.ToString() + " BP";
            }
        }
        else
        {
            bpFill.text = "";
        }

        targets.text = "Targets: " + item.Targets.ToString();

        if (item.Type == ItemType.Buff)
        {
            effects.text = "Stats Buffed: \n";
            for (int i = 0; i < item.Effects.Boosts.Count; ++i)
            {
                effects.text += item.Effects.Boosts[i].stat.ToString() + "\n";
            }
        }
        else
        {
            effects.text = "";
        }
        cost.text = "Cost: " + item.Cost.ToString();
        amountOwned.text = "Owned: " + PlayerParty.GetItemQuantity(item).ToString();
    }

    private void UpdateItemSelection(int selectedAction)
    {
        for (int i = 0; i < buttonTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                buttonTexts[i].color = highlightedColor;
            }
            else
            {
                buttonTexts[i].color = Color.white;
            }
        }
    }

    public void SetItemNames(int start)
    {
        for (int i = 0; i < buttonTexts.Count; ++i)
        {
            if (i < itemManifest.Count)
            {
                buttonTexts[i].text = itemManifest[i + start].Name;
            }
            else
            {
                buttonTexts[i].text = "";
            }
        }
    }
}

