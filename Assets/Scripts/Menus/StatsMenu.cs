using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatsState
{
    Stats, Moves
}

public class StatsMenu : MonoBehaviour
{
    private int currentSelection = 0;
    public List<Text> nameTexts;
    public Color highlightedColor;
    StatsState state;
    [SerializeField] Text lvl;
    [SerializeField] Text xp;
    [SerializeField] Text xpNextLvl;
    [SerializeField] Text hp;
    [SerializeField] Text strength;
    [SerializeField] Text defense;
    [SerializeField] Text specialDefense;
    [SerializeField] Text speed;
    [SerializeField] Text specialPower;

    int start = 0;
    private int moveSelection = 0;
    public List<Text> moveTexts;
    [SerializeField] Text description;
    [SerializeField] Text bpCost;
    [SerializeField] Text damageType;
    [SerializeField] Text Targets;
    [SerializeField] Text Damage;
    [SerializeField] Text Accuracy;
    [SerializeField] Text moveType;
    [SerializeField] Text effects;

    public void Show(bool show)
    {
        gameObject.SetActive(show);
        UpdateSelection(currentSelection);
        ClearMovesBox();
    }

    public bool HandleUpdate(List<Character> characters)
    {
        if (state == StatsState.Moves)
        {
            HandleMoveSelection(characters[currentSelection].Moves);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = StatsState.Stats;
                clearMoves();
                ClearMovesBox();
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
        UpdateSelection(currentSelection);
        UpdateStatsBox(characters[currentSelection]);
        SetMoveNames(characters[currentSelection].Moves, 0);

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = StatsState.Moves;
        }
    }

    public void HandleMoveSelection(List<Move> moves)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++moveSelection;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --moveSelection;
        }
        moveSelection = Mathf.Clamp(moveSelection, 0, moves.Count - 1);
        UpdateMovesBox(moves[moveSelection]);

        if (moves.Count > 4 && moveSelection > 3)
        {
            start = moveSelection - 3;
            SetMoveNames(moves, start);
        }
        else
        {
            start = 0;
            SetMoveNames(moves, start);
        }
        int move = moveSelection > 3 ? 3 : moveSelection;
        UpdateMovesSelection(move);

    }

    private void UpdateStatsBox(Character character)
    {
        lvl.text = "LvL: " + character.Level.ToString();
        xp.text = "XP: " + character.XP.ToString();
        xpNextLvl.text = "XP To Next Level: " + character.NextLevel.ToString();
        hp.text = "HP: " + character.HP.ToString() + "/" + character.GetStat(Stat.MaxHP).ToString();
        strength.text = "Strength: " + character.GetStat(Stat.Strength);
        defense.text = "Defense: " + character.GetStat(Stat.Defense);
        specialDefense.text = "Special Defense: " + character.GetStat(Stat.SpecialDefense);
        speed.text = "Speed: " + character.GetStat(Stat.Speed);
        specialPower.text = "Special Attack: " + character.GetStat(Stat.SpecialPower);
    }

    private void UpdateMovesBox(Move move)
    {
        description.text = "Description: " + move.Base.Description;
        bpCost.text = "BP Cost:" + move.Base.BPCost.ToString();

        if (move.Base.SpellType != Type.Buff)
        {
            damageType.text = "Stat Type: " + move.Base.Damage.ToString();
        }
        else
        {
            damageType.text = "";
        }

        Targets.text = "Targets: " + move.Base.Target.ToString();
        if (move.Base.SpellType != Type.Buff)
        {
            Damage.text = "Power: " + move.Base.Power.ToString();
        }
        else
        {
            Damage.text = "";
        }
        if (move.Base.SpellType != Type.Buff && move.Base.SpellType != Type.Heal)
        {
            Accuracy.text = "Accuracy: " + move.Base.Accuracy.ToString();
        }
        else
        {
            Accuracy.text = "";
        }
        moveType.text = "Move Type: " + move.Base.SpellType.ToString();

        if (move.Base.SpellType == Type.Buff)
        {
            effects.text = "Stats Buffed: \n";
            for (int i = 0; i < move.Base.Effects.Boosts.Count; ++i)
            {
                effects.text += move.Base.Effects.Boosts[i].stat.ToString() + "\n";
            }
        }
        else
        {
            effects.text = "";
        }
    }

    private void ClearMovesBox()
    {
        description.text = "";
        bpCost.text = "";
        damageType.text = "";
        Targets.text = "";
        Damage.text = "";
        Accuracy.text = "";
        moveType.text = "";
        effects.text = "";
    }

    private void UpdateSelection(int selectedAction)
    {
        for (int i = 0; i < nameTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                nameTexts[i].color = highlightedColor;
            }
            else
            {
                nameTexts[i].color = Color.white;
            }
        }
    }
    private void UpdateMovesSelection(int selectedAction)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                moveTexts[i].color = highlightedColor;
            }
            else
            {
                moveTexts[i].color = Color.white;
            }
        }
    }
    private void clearMoves()
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            moveTexts[i].color = Color.black;
        }
    }
    public void SetCharacterNames(List<Character> characters)
    {
        for (int i = 0; i < nameTexts.Count; ++i)
        {
            if (i < characters.Count)
            {
                nameTexts[i].text = characters[i].Base.Name;
            }
            else
            {
                nameTexts[i].text = "";
            }
        }
    }

    public void SetMoveNames(List<Move> moves, int start)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i + start].Base.Name;
            }
            else
            {
                moveTexts[i].text = "";
            }
        }
    }
}
