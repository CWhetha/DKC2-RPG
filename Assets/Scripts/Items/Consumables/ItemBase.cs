using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Create New Item")]
public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] int hpGain;
    [SerializeField] int bpGain;
    [SerializeField] int cost;
    [SerializeField] bool isFullHP;
    [SerializeField] bool isFullBP;
    [SerializeField] bool isUsedInMenu;
    [SerializeField] ItemType type;
    [SerializeField] Targets targets;
    [SerializeField] Sprite sprite;
    [SerializeField] MoveEffects effects;


    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public Sprite Sprite
    {
        get { return sprite; }
    }
    public int Cost
    {
        get { return cost; }
    }
    public int HpGain
    {
        get { return hpGain; }
    }
    public int BpGain
    {
        get { return bpGain; }
    }
    public bool IsFullHP
    {
        get { return isFullHP; }
    }
    public bool IsFullBP
    {
        get { return isFullBP; }
    }
    public bool IsUsedInMenu
    {
        get { return isUsedInMenu; }
    }
    public ItemType Type
    {
        get { return type; }
    }
    public Targets Targets
    {
        get { return targets; }
    }
    public MoveEffects Effects
    {
        get { return effects; }
    }
}

public enum ItemType
{
    HP,
    BP,
    Both,
    Buff
}
