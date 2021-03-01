using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Character
{
    [SerializeField] CharacterBase _base;
    [SerializeField] int level;
    [SerializeField] int xp;
    [SerializeField] Armor equipedArmor;
    [SerializeField] Weapon equipedWeapon;

    public CharacterBase Base
    {
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }

    public int XP
    {
        get
        {
            return xp;
        }
    }

    public Armor EquipedArmor
    {
        get { return equipedArmor; }
        set { equipedArmor = value; }

    }
    public Weapon EquipedWeapon
    {
        get { return equipedWeapon; }
        set { equipedWeapon = value; }
    }

    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Dictionary<Stat, int> EquipmentStats { get; private set; }
    public int HP { get; set; }
    public bool IsGaurd { get; set; }

    public void Init()
    {
        CheckAvalibleMoves();
        SetEquipmentBoosts();
        calculateStats();
        ResetBoosts();

        HP = MaxHp;
    }

    public void CheckAvalibleMoves()
    {
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
        }
    }

    public string CheckNewMove()
    {
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level == Level)
            {
                return move.Base.Name;
            }
        }
        return "";
    }

    public bool CheckifLevelUp()
    {
        if (XP >= NextLevel)
        {
            level += 1;
            HP += ((Mathf.FloorToInt((Base.MaxHp * Level) / 20f) + 10) - (Mathf.FloorToInt((Base.MaxHp * (Level - 1)) / 20f) + 10));
            calculateStats();
            CheckAvalibleMoves();
            return true;
        }
        return false;
    }

    public void AddXP(int val)
    {
        xp += val;
    }

    public void ResetBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, 0},
            {Stat.Strength, 0},
            {Stat.Defense, 0},
            {Stat.SpecialDefense, 0},
            {Stat.Speed, 0},
            {Stat.SpecialPower, 0}
        };
    }

    public void SetEquipmentBoosts()
    {
        EquipmentStats = new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, 0},
            {Stat.Strength, 0},
            {Stat.Defense, 0},
            {Stat.Speed, 0},
            {Stat.SpecialDefense, 0},
            {Stat.SpecialPower, 0}
        };

        if (equipedArmor != null && equipedArmor.Base != null)
        {
            EquipmentBoosts armor = equipedArmor.Base.Boosts;

            EquipmentStats[Stat.MaxHP] = armor.HpBoost;
            EquipmentStats[Stat.Strength] = armor.Strength;
            EquipmentStats[Stat.Defense] = armor.Defense;
            EquipmentStats[Stat.SpecialDefense] = armor.SpecialDefense;
            EquipmentStats[Stat.SpecialPower] = armor.SpecialPower;
            EquipmentStats[Stat.Speed] = armor.Speed;
        }

        if (equipedWeapon != null && equipedWeapon.Base != null)
        {
            EquipmentBoosts weapon = equipedWeapon.Base.Boosts;

            EquipmentStats[Stat.MaxHP] = EquipmentStats[Stat.MaxHP] + weapon.HpBoost;
            EquipmentStats[Stat.Strength] = EquipmentStats[Stat.Strength] + weapon.Strength;
            EquipmentStats[Stat.Defense] = EquipmentStats[Stat.Defense] + weapon.Defense;
            EquipmentStats[Stat.SpecialDefense] = EquipmentStats[Stat.SpecialDefense] + weapon.SpecialDefense;
            EquipmentStats[Stat.Speed] = EquipmentStats[Stat.Speed] + weapon.Speed;
            EquipmentStats[Stat.SpecialPower] = EquipmentStats[Stat.SpecialPower] + weapon.SpecialPower;
        }
    }

    public Dictionary<Stat, int> CompareArmor(EquipmentBoosts testArmor)
    {
        Dictionary<Stat, int> tempStats = new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, testArmor.HpBoost},
            {Stat.Strength, testArmor.Strength},
            {Stat.Defense, testArmor.Defense},
            {Stat.SpecialDefense, testArmor.SpecialDefense},
            {Stat.Speed, testArmor.Speed},
            {Stat.SpecialPower, testArmor.SpecialPower},
        };

        if (equipedWeapon != null && equipedWeapon.Base != null)
        {
            EquipmentBoosts weapon = equipedWeapon.Base.Boosts;

            tempStats[Stat.MaxHP] = tempStats[Stat.MaxHP] + weapon.HpBoost;
            tempStats[Stat.Strength] = tempStats[Stat.Strength] + weapon.Strength;
            tempStats[Stat.Defense] = tempStats[Stat.Defense] + weapon.Defense;
            tempStats[Stat.SpecialDefense] = tempStats[Stat.SpecialDefense] + weapon.SpecialDefense;
            tempStats[Stat.Speed] = tempStats[Stat.Speed] + weapon.Speed;
            tempStats[Stat.SpecialPower] = tempStats[Stat.SpecialPower] + weapon.SpecialPower;
        }
        return new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, tempStats[Stat.MaxHP] + Stats[Stat.MaxHP]},
            {Stat.Strength, tempStats[Stat.Strength] + Stats[Stat.Strength]},
            {Stat.Defense, tempStats[Stat.Defense] + Stats[Stat.Defense]},
            {Stat.SpecialDefense, tempStats[Stat.SpecialDefense] + Stats[Stat.SpecialDefense]},
            {Stat.Speed, tempStats[Stat.Speed] + Stats[Stat.Speed]},
            {Stat.SpecialPower, tempStats[Stat.SpecialPower] + Stats[Stat.SpecialPower]},
        };
    }

    public Dictionary<Stat, int> CompareWeapon(EquipmentBoosts testWeapon)
    { 
        Dictionary<Stat, int> tempStats = new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, testWeapon.HpBoost},
            {Stat.Strength, testWeapon.Strength},
            {Stat.Defense, testWeapon.Defense},
            {Stat.SpecialDefense, testWeapon.SpecialDefense},
            {Stat.Speed, testWeapon.Speed},
            {Stat.SpecialPower, testWeapon.SpecialPower},
        };

        if (equipedArmor != null && equipedArmor.Base != null)
        {
            EquipmentBoosts armor = equipedArmor.Base.Boosts;

            tempStats[Stat.MaxHP] = tempStats[Stat.MaxHP] + armor.HpBoost;
            tempStats[Stat.Strength] = tempStats[Stat.Strength] + armor.Strength;
            tempStats[Stat.Defense] = tempStats[Stat.Defense] + armor.Defense;
            tempStats[Stat.SpecialDefense] = tempStats[Stat.SpecialDefense] + armor.SpecialDefense;
            tempStats[Stat.Speed] = tempStats[Stat.Speed] + armor.Speed;
            tempStats[Stat.SpecialPower] = tempStats[Stat.SpecialPower] + armor.SpecialPower;
        }
        return new Dictionary<Stat, int>()
        {
            {Stat.MaxHP, tempStats[Stat.MaxHP] + Stats[Stat.MaxHP]},
            {Stat.Strength, tempStats[Stat.Strength] + Stats[Stat.Strength]},
            {Stat.Defense, tempStats[Stat.Defense] + Stats[Stat.Defense]},
            {Stat.SpecialDefense, tempStats[Stat.SpecialDefense] + Stats[Stat.SpecialDefense]},
            {Stat.Speed, tempStats[Stat.Speed] + Stats[Stat.Speed]},
            {Stat.SpecialPower, tempStats[Stat.SpecialPower] + Stats[Stat.SpecialPower]},
        };
    }

    public void calculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.MaxHP, Mathf.FloorToInt((Base.MaxHp * Level) / 5f) + 5);
        Stats.Add(Stat.Strength, Mathf.FloorToInt((Base.Strength * Level) / 7f) + 3);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 11f) + 2);
        Stats.Add(Stat.SpecialDefense, Mathf.FloorToInt((Base.SpecialDefense * Level) / 11f) + 2);
        Stats.Add(Stat.Speed, Mathf.FloorToInt(Base.Speed));
        Stats.Add(Stat.SpecialPower, Mathf.FloorToInt((Base.SpecialPower * Level) / 7f) + 3);

    }

    public Character GetCopy()
    {
        Character copy = new Character();
        copy._base = _base;
        copy.level = level;
        return copy;
    }

    public int GetStat(Stat stat)
    {
        int statVal = Stats[stat] + EquipmentStats[stat];
        int boost = StatBoosts[stat];
        float[] boostValues = new float[] { 1f, 1.25f, 1.5f, 1.75f, 2f, 2.25f, 2.5f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }
        return statVal;
    }

    public int NextLevel
    {
        get { return (150 * Level); }
    }
    public int MonsterXP
    {
        get { return (Base.DefeatXP * Level); }
    }
    public int MonsterBananas
    {
        get { return (Base.DefeatBananas * Level); }
    }
    public int MaxHp
    {
        get { return GetStat(Stat.MaxHP); }
    }
    public int Strength
    {
        get { return GetStat(Stat.Strength); }
    }
    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }
    public int SpecialDefense
    {
        get { return GetStat(Stat.SpecialDefense); }
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public int SpecialPower
    {
        get { return GetStat(Stat.SpecialPower); }
    }


    public int CalcAttack(Damage damageType)
    {
        if (damageType == Damage.Physical)
        {
            return Strength;
        }
        else if (damageType == Damage.Special)
        {
            return SpecialPower;
        }
        return 1;
    }

    public int CalcDefense(Damage damageType)
    {
        if (damageType == Damage.Physical)
        {
            return Defense;
        }
        else if (damageType == Damage.Special)
        {
            return SpecialDefense;
        }
        return 1;
    }

    public DamageDetails TakeDamage(Move move, Character attacker)
    {
        float critical = 1f;
        float gaurded = 1f;

        if ((Random.value * 100f) <= 6)
        {
            critical = 2f;
        }

        if (IsGaurd)
        {
            gaurded = 0.6f;
        }

        var damageDetails = new DamageDetails()
        {
            Critical = critical,
            Fainted = false,
            Damage = 0,
            BP = 0
        };

        float modifiers = Random.Range(0.85f, 1f) * critical * gaurded;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = 0;

        d = a * move.Base.Power * ((float)attacker.CalcAttack(move.Base.Damage) / CalcDefense(move.Base.Damage)) + 2;

        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        damageDetails.Damage = damage;
        return damageDetails;
    }

    public DamageDetails TakeAttackDamage(Character attacker)
    {
        float critical = 1f;
        float gaurded = 1f;
        if ((Random.value * 100f) <= 6)
        {
            critical = 2f;
        }
        if (IsGaurd)
        {
            gaurded = 0.6f;
        }
        var damageDetails = new DamageDetails()
        {
            Critical = critical,
            Fainted = false,
            Damage = 0,
            BP = 0
        };

        float modifiers = Random.Range(0.85f, 1f) * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * 20 * ((float)attacker.Strength / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        damageDetails.Damage = damage;
        return damageDetails;
    }

    public DamageDetails TakeHeal(Move move, Character healer)
    {
        var damageDetails = new DamageDetails()
        {
            Critical = 0f,
            Fainted = false,
            Damage = 0,
            BP = 0
        };
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * healer.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)healer.CalcAttack(move.Base.Damage) / 10) + 2;
        int heal = Mathf.FloorToInt(d * modifiers);

        if (HP + heal >= MaxHp)
        {
            heal = MaxHp - HP;
            HP = MaxHp;
        }
        else
        {
            HP += heal;
        }
        damageDetails.Damage = heal;
        return damageDetails;
    }
    public DamageDetails TakeItemHeal(Item item)
    {
        var damageDetails = new DamageDetails()
        {
            Critical = 0f,
            Fainted = false,
            Damage = 0,
            BP = 0
        };
        if (item.Base.Type == ItemType.HP || item.Base.Type == ItemType.Both)
        {
            int heal = item.Base.HpGain;
            if (item.Base.IsFullHP)
            {
                heal = MaxHp - HP;
                HP = MaxHp;
            }
            else
            {
                if (HP + heal >= MaxHp)
                {
                    heal = MaxHp - HP;
                    HP = MaxHp;
                }
                else
                {
                    HP += heal;
                }
            }
            damageDetails.Damage = heal;
        }
        if (item.Base.Type == ItemType.BP || item.Base.Type == ItemType.Both)
        {
            damageDetails.BP = 0;
        }
        return damageDetails;
    }

    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach (StatBoost statBoost in statBoosts)
        {
            Stat stat = statBoost.stat;
            int boost = statBoost.boost;

            StatBoosts[stat] = boost;
        }
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public int Damage { get; set; }
    public int BP { get; set; }
    public float Critical { get; set; }
    public bool Fainted { get; set; }
}