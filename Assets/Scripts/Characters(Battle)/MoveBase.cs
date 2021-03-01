using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Character/Create New Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    //[SerializeField] string type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int bpCost;
    [SerializeField] bool alwaysHits;
    [SerializeField] Targets target;
    [SerializeField] Type type;
    [SerializeField] Damage damage;
    [SerializeField] MoveEffects effects;
    [SerializeField] Sprite sprite;


    public string Name
    {
        get { return name; }
    }
    public Sprite Sprite
    {
        get { return sprite; }
    }
    public string Description
    {
        get { return description; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Accuracy
    {
        get { return accuracy; }
    }
    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }
    public int BPCost
    {
        get { return bpCost; }
    }
    public Targets Target
    {
        get { return target; }
    }
    public Type SpellType
    {
        get { return type; }
    }
    public Damage Damage
    {
        get { return damage; }
    }
    public MoveEffects Effects
    {
        get { return effects; }
    }

}
[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum Targets
{
    Enemy, Ally, Enemies, Allies, All, AllyFainted, AlliesFainted
}

public enum Type
{
    Damage, Heal, Buff
}

public enum Damage
{
    Physical, Special
}

