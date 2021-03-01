using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDActions : MonoBehaviour
{
    [SerializeField] public GameObject actions;
    [SerializeField] public GameObject actionOptions;
    [SerializeField] public GameObject targetOptions;
    [SerializeField] public GameObject itemOptions;
    [SerializeField] public GameObject moveName;

    [SerializeField] Color highlightedColor;
    [SerializeField] Color noBPColor;

    [SerializeField] public List<Text> actionTexts;
    [SerializeField] public List<Text> moveTexts;
    [SerializeField] public List<Text> targetTexts;
    [SerializeField] public List<Text> itemTexts;

    [SerializeField] public Text bpText;
    [SerializeField] public Text bpCostText;
    [SerializeField] public GameObject amountBox;
    [SerializeField] public Text amountText;

    [SerializeField] public Text activeCharacter;
    [SerializeField] public Text moveText;


    public void EnableActionBox(bool enabled)
    {
        actions.SetActive(enabled);
        if (enabled)
        { 
            bpCostText.gameObject.SetActive(false);
            amountBox.SetActive(false);
        }
    }

    public void EnableActionOptionsBox(bool enabled)
    {
        actionOptions.SetActive(enabled);
        if (enabled)
        {
            bpCostText.gameObject.SetActive(true);
        }
    }

    public void EnableItemOptionsBox(bool enabled)
    {
        actionOptions.SetActive(enabled);
        amountBox.SetActive(true);
    }

    public void EnableTargetsBox(bool enabled)
    {
        targetOptions.SetActive(enabled);
        if (!enabled)
        {
            bpCostText.gameObject.SetActive(false);
            amountBox.SetActive(false);
        }
    }
     
    public void EnableActiveCharacter(bool enabled)
    {
        if (enabled == false)
        {
            activeCharacter.text = "";
        }
    }

    public void SetActiveCharacter(BattleUnit unit)
    {
        activeCharacter.text = unit.Character.Base.Name;
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }

    public IEnumerator DisplayText(string phrase, float duration)
    {
        moveText.text = phrase;
        moveName.SetActive(true);
        yield return new WaitForSeconds(duration);
        moveName.SetActive(false);
    }

    public void UpdateBP(int bp, int maxBP)
    {
        bpText.text = "BP: " + bp + "/" + maxBP; 
    }

    public void UpdateMoveSelection(int bp, int selectedAction, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (move.Base.BPCost > bp && i == selectedAction)
            {
                moveTexts[i].color = noBPColor;
            }
            else if (i == selectedAction)
            {
                moveTexts[i].color = highlightedColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }

        bpCostText.text = "- " + move.BPCost;
    }

    public void UpdateItemSelection(int selectedItem, Item item)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedItem)
            {
                moveTexts[i].color = highlightedColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }

        amountText.text = "Amount: " + item.Quantity;
    }

    public void UpdateTargetSelection(int selectedAction, List<BattleUnit> enemyUnits)
    {
        for (int i = 0; i < targetTexts.Count; ++i)
        {
            if (i < enemyUnits.Count)
            {
                if (i == selectedAction)
                {
                    targetTexts[i].color = highlightedColor;
                    enemyUnits[i].Target.SetActive(true);
                }
                else
                {
                    targetTexts[i].color = Color.black;
                    enemyUnits[i].Target.SetActive(false);
                }
            }
        }
    }

    public void TargetMultipleSelection(List<BattleUnit> targetUnits, bool isActive)
    {
        for (int i = 0; i < targetUnits.Count; ++i)
        {
            targetUnits[i].Target.SetActive(isActive);
        }
        for (int i = 0; i < targetTexts.Count; ++i)
        {
            targetTexts[i].color = highlightedColor;
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

    public void SetItemNames(List<Item> items, int start)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < items.Count)
            {
                moveTexts[i].text = items[i + start].Base.Name;
            }
            else
            {
                moveTexts[i].text = "";
            }
        }
    }

    public void SetTargetNames(List<BattleUnit> enemyUnits)
    {
        for (int i = 0; i < targetTexts.Count; ++i)
        {
            if (i < enemyUnits.Count)
            {
                targetTexts[i].text = enemyUnits[i].Character.Base.Name;
            }
            else
            {
                targetTexts[i].text = "";
            }
        }
    }
}
