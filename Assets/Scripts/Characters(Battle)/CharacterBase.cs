using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    MaxHP,
    Strength,
    Defense,
    SpecialDefense,
    SpecialPower,
    Speed
}

[CreateAssetMenu(fileName = "Character", menuName = "Character/Create New Character")]
public class CharacterBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite faintSprite;

    [SerializeField] int maxHp;
    [SerializeField] int strength;
    [SerializeField] int defense;
    [SerializeField] int specialDefense;
    [SerializeField] int specialPower;
    [SerializeField] int speed;
    [SerializeField] int defeatXp;
    [SerializeField] int defeatBananas;

    [SerializeField] List<LearnableMove> learnableMoves;


    public string Name
    {
        get { return name; }
    }
    public Sprite CharacterSprite
    {
        get { return sprite; }
    }
    public Sprite FaintSprite
    {
        get { return faintSprite; }
    }
    public int MaxHp
    {
        get { return maxHp; }
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
    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
    public int DefeatXP
    {
        get { return defeatXp; }
    }
    public int DefeatBananas
    {
        get { return defeatBananas; }
    }
}
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }
    public int Level
    {
        get { return level; }
    }
}
