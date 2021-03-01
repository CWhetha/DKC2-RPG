using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Create New Equipment")]
public class EquipmentBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] EquipmentBoosts boosts;
    [SerializeField] int cost;

    public string Name
    {
        get { return name; }
    }
    public EquipmentBoosts Boosts
    {
        get { return boosts; }
    }
    public int Cost
    {
        get { return cost; }
    }
}

[System.Serializable]
public class EquipmentBoosts
{
    [SerializeField] int hpBoost = 0;
    [SerializeField] int strength = 0;
    [SerializeField] int defense = 0;
    [SerializeField] int specialDefense = 0;
    [SerializeField] int specialPower = 0;
    [SerializeField] int speed = 0;

    public int HpBoost
    {
        get { return hpBoost; }
    }
    public int Strength
    {
        get { return strength; }
    }
    public int Defense
    {
        get { return defense; }
    }
    public int SpecialDefense
    {
        get { return specialDefense; }
    }
    public int SpecialPower
    {
        get { return specialPower; }
    }
    public int Speed
    {
        get { return speed; }
    }
}
