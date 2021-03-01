using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerParty 
{
    [SerializeField] static List<Character> characters;
    [SerializeField] static List<Item> items;
    [SerializeField] static int bananas, maxBp, bp;
    [SerializeField] static List<Armor> armorInventory;
    [SerializeField] static List<Weapon> weaponInventory;
    [SerializeField] static Vector3 position;

    public static List<Character> Characters
    {
        get { return characters; }
        set { characters = value; }
    }

    public static List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public static List<Armor> ArmorInventory
    {
        get { return armorInventory; }
        set { armorInventory = value; }
    }

    public static List<Weapon> WeaponInventory
    {
        get { return weaponInventory; }
        set { weaponInventory = value; }
    }

    public static Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public static int Bananas
    {
        get { return bananas; }
        set { bananas = value; }
    }

    public static int BP
    {
        get { return bp; }
        set { bp = value; }
    }

    public static int MaxBP
    {
        get { return maxBp; }
        set { maxBp = value; }
    }

    public static int GetItemQuantity(ItemBase item)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].Base == item)
            {
                return items[i].Quantity;
            }
        }
        return 0;
    }

    public static int GetArmorQuantity(EquipmentBase weapon)
    {
        int count = 0;
        for (int i = 0; i < armorInventory.Count; ++i)
        {
            if (armorInventory[i].Base == weapon)
            {
                count++;
            }
        }
        return count;
    }

    public static int GetWeaponQuantity(EquipmentBase armor)
    {
        int count = 0;
        for (int i = 0; i < weaponInventory.Count; ++i)
        {
            if (weaponInventory[i].Base == armor)
            {
                count++;
            }
        }
        return count;
    }

    public static void GiveArmor(EquipmentBase armorBase)
    {
        Armor armor = new Armor();
        armor.Base = armorBase;
        armor.EquipedTo = "";
        ArmorInventory.Add(armor);
    }
    public static void GiveWeapon(EquipmentBase weaponBase)
    {
        Weapon weapon = new Weapon();
        weapon.Base = weaponBase;
        weapon.EquipedTo = "";
        WeaponInventory.Add(weapon);
    }

    public static void GiveItem(ItemBase item, int itemQuantity)
    {
        bool found = false;
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].Base == item)
            {
                found = true;
                items[i].GainItem(itemQuantity);
            }
        }
        if (found == false)
        {
            Item temp = new Item();
            temp.Base = item;
            temp.Quantity = itemQuantity;
            items.Add(temp);
        }
    }
}

