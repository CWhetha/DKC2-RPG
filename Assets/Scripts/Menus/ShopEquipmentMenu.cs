using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipmentMenu : MonoBehaviour
{
    List<EquipmentBase> weaponManifest;
    List<EquipmentBase> armorManifest;

    public List<Text> buttonTexts;

    [SerializeField] Text armorText;
    [SerializeField] Text weaponText;

    [SerializeField] Text money;
    [SerializeField] Text cost;
    [SerializeField] Text amountOwned;

    [SerializeField] Text hp;
    [SerializeField] Text strength;
    [SerializeField] Text defense;
    [SerializeField] Text specialPower;
    [SerializeField] Text specialDefense;
    [SerializeField] Text speed;


    public Color highlightedColor;

    public event Action<Shoptype> OpenShop;
    public event Action<Shoptype> CloseShop;
    int currentWeaponSelection;
    int currentArmorSelection;

    int equipmentStart;
    EquipmentType etype;

    public void LoadShop(List<EquipmentBase> weapons, List<EquipmentBase> armor)
    {
        weaponManifest = weapons;
        armorManifest = armor;
        equipmentStart = 0;
        currentWeaponSelection = 0;
        currentArmorSelection = 0;
        etype = EquipmentType.Weapon;
        weaponText.color = highlightedColor;

        OpenShop(Shoptype.Equipment);
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
        CloseShop(Shoptype.Equipment);
    }

    public void HandleItemSelection()
    {
        List<EquipmentBase> manifest = new List<EquipmentBase>();
        int selection = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            etype = EquipmentType.Weapon;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            etype = EquipmentType.Armor;
        }

        if (etype == EquipmentType.Weapon)
        {
            manifest = weaponManifest;
            selection = currentWeaponSelection;
            armorText.color = Color.white;
            weaponText.color = highlightedColor;
        }
        else
        {
            manifest = armorManifest;
            selection = currentArmorSelection;
            armorText.color = highlightedColor;
            weaponText.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++selection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --selection;
        }
        selection = Mathf.Clamp(selection, 0, manifest.Count - 1);
        RedrawEquipmentInfo(manifest[selection]);
        if (manifest.Count > buttonTexts.Count && selection > buttonTexts.Count - 1)
        {

            equipmentStart = selection - (buttonTexts.Count - 1);
            SetEquipmentNames(equipmentStart, manifest);
        }
        else
        {
            equipmentStart = 0;
            SetEquipmentNames(equipmentStart, manifest);
        }
        int move = selection > buttonTexts.Count - 1 ? buttonTexts.Count - 1 : selection;
        UpdateItemSelection(move);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerParty.Bananas >= manifest[selection].Cost)
            {
                PlayerParty.Bananas -= manifest[selection].Cost;
                if (etype == EquipmentType.Weapon)
                {
                    PlayerParty.GiveWeapon(manifest[selection]);
                }
                else
                {
                    PlayerParty.GiveArmor(manifest[selection]);
                }
                RedrawEquipmentInfo(manifest[selection]);
            }
        }

        if (etype == EquipmentType.Weapon)
        {
            currentWeaponSelection = selection;
        }
        else
        {
            currentArmorSelection = selection;
        }
    }

    void RedrawEquipmentInfo(EquipmentBase equipment)
    {
        if (etype == EquipmentType.Weapon)
        {
            amountOwned.text = "Amount: " + PlayerParty.GetWeaponQuantity(equipment).ToString();
        }
        else
        {
            amountOwned.text = "Amount: " + PlayerParty.GetArmorQuantity(equipment).ToString();
        }

        money.text = "Bananas: " + PlayerParty.Bananas.ToString();
        cost.text = "Cost: " + equipment.Cost.ToString();
        hp.text = "HP +" + equipment.Boosts.HpBoost.ToString();
        strength.text = "Strength +" + equipment.Boosts.Strength.ToString();
        defense.text = "Defense +" + equipment.Boosts.Defense.ToString();
        specialPower.text = "SpecialPower +" + equipment.Boosts.SpecialPower.ToString();
        specialDefense.text = "SpecialDefense +" + equipment.Boosts.SpecialDefense.ToString();
        speed.text = "Speed +" + equipment.Boosts.Speed.ToString();
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

    public void SetEquipmentNames(int start, List<EquipmentBase> manifest)
    {
        for (int i = 0; i < buttonTexts.Count; ++i)
        {
            if (i < manifest.Count)
            {
                buttonTexts[i].text = manifest[i + start].Name;
            }
            else
            {
                buttonTexts[i].text = "";
            }
        }
    }
}

public enum EquipmentType
{
    Armor, Weapon
}
