using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentState
{
    Character, Type, Equipment
}

public class EquipmentMenu : MonoBehaviour
{
    private int currentSelection = 0;
    private int currentTypeSelection = 0;
    private int currentWeaponSelection = 0;
    private int currentArmorSelection = 0;

    List<string> options = new List<string>()
            {
                "Weapon",
                "Armor"
            };
    [SerializeField] List<Text> nameTexts;
    [SerializeField] List<Text> typeTexts;
    [SerializeField] List<Text> equipmentTexts;
    [SerializeField] List<Text> equipedTexts;

    [SerializeField] List<Text> maxHPText;
    [SerializeField] List<Text> strengthText;
    [SerializeField] List<Text> defenseText;
    [SerializeField] List<Text> specialDefenseText;
    [SerializeField] List<Text> speedText;
    [SerializeField] List<Text> specialPowerText;

    [SerializeField] Text equipedWeaponText;
    [SerializeField] Text equipedArmorText;

    [SerializeField] Color highlightedColor;
    EquipmentState state;

    public void Show(bool show)
    {
        gameObject.SetActive(show);
        ClearTexts(equipmentTexts);
        ClearTexts(equipedTexts);
        ClearEquipedEquipment();
        ClearStats();
        UpdateSelection(nameTexts, currentSelection);
    }

    public bool HandleUpdate(List<Character> characters)
    {
        if (state == EquipmentState.Type)
        {
            SetTypeNames(characters[currentSelection]);
            HandleTypeSelection(options);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearEquipedEquipment();
                state = EquipmentState.Character;
                currentTypeSelection = 0;
            }
        }
        else if (state == EquipmentState.Equipment)
        {
            if (options[currentTypeSelection] == "Weapon")
            {
                SetWeaponNames(PlayerParty.WeaponInventory, 0);
                HandleWeaponSelection(characters[currentSelection], PlayerParty.WeaponInventory);
            }
            else
            {
                SetArmorNames(PlayerParty.ArmorInventory, 0);
                HandleArmorSelection(characters[currentSelection], PlayerParty.ArmorInventory);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearTexts(equipmentTexts);
                ClearTexts(equipedTexts);
                ClearStats();
                state = EquipmentState.Type;
                currentWeaponSelection = 0;
                currentArmorSelection = 0;
            }
        }
        else
        {
            HandleSelection(characters);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }
        }

        return false;
    }

    public void HandleSelection(List<Character> characters)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentSelection;
        }
        currentSelection = Mathf.Clamp(currentSelection, 0, characters.Count - 1);
        UpdateSelection(nameTexts, currentSelection);
        SetTypeNames(characters[currentSelection]);

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = EquipmentState.Type;
        }
    }

    public void HandleTypeSelection(List<string> options)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentTypeSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentTypeSelection;
        }
        currentTypeSelection = Mathf.Clamp(currentTypeSelection, 0, options.Count - 1);
        UpdateSelection(typeTexts, currentTypeSelection);

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = EquipmentState.Equipment;
        }
    }

    public void HandleWeaponSelection(Character character, List<Weapon> weapon)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentWeaponSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentWeaponSelection;
        }
        currentWeaponSelection = Mathf.Clamp(currentWeaponSelection, 0, weapon.Count - 1);
        UpdateSelection(equipmentTexts, currentWeaponSelection);
        SetEquipmentStats("Weapon", character, weapon[currentWeaponSelection].Base.Boosts);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weapon[currentWeaponSelection].EquipedTo == "")
            {
                character.EquipedWeapon.EquipedTo = "";
                character.EquipedWeapon = weapon[currentWeaponSelection];
                weapon[currentWeaponSelection].EquipedTo = character.Base.Name;
                character.SetEquipmentBoosts();
                SetTypeNames(character);
            }
            else if (weapon[currentWeaponSelection].EquipedTo == character.Base.Name)
            {
                character.EquipedWeapon.EquipedTo = "";
                character.EquipedWeapon = null;
                weapon[currentWeaponSelection].EquipedTo = "";
                character.SetEquipmentBoosts();
                SetTypeNames(character);
            }
        }
    }

    public void HandleArmorSelection(Character character, List<Armor> armor)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentArmorSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentArmorSelection;
        }
        currentArmorSelection = Mathf.Clamp(currentArmorSelection, 0, armor.Count - 1);
        UpdateSelection(equipmentTexts, currentArmorSelection);
        SetEquipmentStats("Armor", character, armor[currentArmorSelection].Base.Boosts);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (armor[currentArmorSelection].EquipedTo == "")
            {
                character.EquipedArmor.EquipedTo = "";
                character.EquipedArmor = armor[currentArmorSelection];
                armor[currentArmorSelection].EquipedTo = character.Base.Name;
                character.SetEquipmentBoosts();
                SetTypeNames(character);
            }
            else if (armor[currentArmorSelection].EquipedTo == character.Base.Name)
            {
                character.EquipedArmor.EquipedTo = "";
                character.EquipedArmor = null;
                armor[currentArmorSelection].EquipedTo = "";
                character.SetEquipmentBoosts();
                SetTypeNames(character);
            }
        }
    }
    private void UpdateSelection(List<Text> texts, int selectedAction)
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            if (i == selectedAction)
            {
                texts[i].color = highlightedColor;
            }
            else
            {
                texts[i].color = Color.white;
            }
        }
    }

    public void SetCharacterNames(List<Character> characters, int start)
    {
        for (int i = 0; i < nameTexts.Count; ++i)
        {
            if (i < characters.Count)
            {
                nameTexts[i].text = characters[i + start].Base.Name;
            }
            else
            {
                nameTexts[i].text = "";
            }
        }
    }

    void SetTypeNames(Character character)
    {
        if (character.EquipedWeapon != null && character.EquipedWeapon.Base != null)
        {
            equipedWeaponText.text = character.EquipedWeapon.Base.Name;
        }
        else
        {
            equipedWeaponText.text = "";
        }

        if (character.EquipedArmor != null && character.EquipedArmor.Base)
        {
            equipedArmorText.text = character.EquipedArmor.Base.Name;
        }
        else
        {
            equipedArmorText.text = "";
        }

        for (int i = 0; i < typeTexts.Count; ++i)
        {
            typeTexts[i].text = options[i];
        }
    }

    void SetWeaponNames(List<Weapon> weapons, int start)
    {
        for (int i = 0; i < equipmentTexts.Count; ++i)
        {
            if (i < weapons.Count)
            {
                equipmentTexts[i].text = weapons[i + start].Base.Name;
                if (weapons[i + start].EquipedTo != "")
                {
                    equipedTexts[i].text = weapons[i + start].EquipedTo;
                }
                else
                {
                    equipedTexts[i].text = "";
                }
            }
            else
            {
                equipmentTexts[i].text = "";
                equipedTexts[i].text = "";
            }
        }
    }

    void SetArmorNames(List<Armor> armor, int start)
    {
        for (int i = 0; i < equipmentTexts.Count; ++i)
        {
            if (i < armor.Count)
            {
                equipmentTexts[i].text = armor[i + start].Base.Name;
                if (armor[i + start].EquipedTo != "")
                {
                    equipedTexts[i].text = armor[i + start].EquipedTo;
                }
                else
                {
                    equipedTexts[i].text = "";
                }
            }
            else
            {
                equipmentTexts[i].text = "";
                equipedTexts[i].text = "";
            }
        }
    }

    void SetEquipmentStats(string type, Character character, EquipmentBoosts boosts)
    {
        Dictionary<Stat, int> statbuffs;
        if (type == "Weapon")
        {
            statbuffs = character.CompareWeapon(boosts);
        }
        else
        {
            statbuffs = character.CompareArmor(boosts);
        }

        maxHPText[0].text = "HP " + character.GetStat(Stat.MaxHP).ToString() + " -> ";
        strengthText[0].text = "Strength " + character.GetStat(Stat.Strength).ToString() + " -> ";
        defenseText[0].text = "Defense " + character.GetStat(Stat.Defense).ToString() + " -> ";
        specialDefenseText[0].text = "Special Defense " + character.GetStat(Stat.SpecialDefense).ToString() + " -> ";
        speedText[0].text = "Speed " + character.GetStat(Stat.Speed).ToString() + " -> ";
        specialPowerText[0].text = "Special Attack " + character.GetStat(Stat.SpecialPower).ToString() + " -> ";

        maxHPText[1].text = " " + statbuffs[Stat.MaxHP].ToString();
        strengthText[1].text = " " + statbuffs[Stat.Strength].ToString();
        defenseText[1].text = " " + statbuffs[Stat.Defense].ToString();
        specialDefenseText[1].text = " " + statbuffs[Stat.SpecialDefense].ToString();
        speedText[1].text = " " + statbuffs[Stat.Speed].ToString();
        specialPowerText[1].text = " " + statbuffs[Stat.SpecialPower].ToString();

        maxHPText[1].color = getColor(character.GetStat(Stat.MaxHP), statbuffs[Stat.MaxHP]);
        strengthText[1].color = getColor(character.GetStat(Stat.Strength), statbuffs[Stat.Strength]); ;
        defenseText[1].color = getColor(character.GetStat(Stat.Defense), statbuffs[Stat.Defense]); ;
        specialDefenseText[1].color = getColor(character.GetStat(Stat.SpecialDefense), statbuffs[Stat.SpecialDefense]); ;
        speedText[1].color = getColor(character.GetStat(Stat.Speed), statbuffs[Stat.Speed]); ;
        specialPowerText[1].color = getColor(character.GetStat(Stat.SpecialPower), statbuffs[Stat.SpecialPower]); ;
    }

    Color getColor(int baseStat, int newStat)
    {
        if (baseStat < newStat)
        {
            return Color.green;
        }
        else if (baseStat > newStat)
        {
            return Color.red;
        }
        else
        {
            return Color.black;
        }
    }

    void ClearEquipedEquipment()
    {
        for (int i = 0; i < typeTexts.Count; ++i)
        {
            typeTexts[i].color = Color.white;
        }
        equipedWeaponText.text = "";
        equipedArmorText.text = "";
    }

    void ClearTexts(List<Text> texts)
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].text = "";
        }
    }

    void ClearStats()
    {
        maxHPText[0].text = "";
        strengthText[0].text = "";
        defenseText[0].text = "";
        specialDefenseText[0].text = "";
        speedText[0].text = "";
        specialPowerText[0].text = "";

        maxHPText[1].text = "";
        strengthText[1].text = "";
        defenseText[1].text = "";
        specialDefenseText[1].text = "";
        speedText[1].text = "";
        specialPowerText[1].text = "";
    }
}