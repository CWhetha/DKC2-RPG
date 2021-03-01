using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightEndInfo : MonoBehaviour
{
    [SerializeField] public Text characterName;
    [SerializeField] public Text currentLVL;
    [SerializeField] public Text currentXP;
    [SerializeField] public Text nextLvlXP;
    [SerializeField] public Text movesText;

    public IEnumerator WriteData(Character character, bool alive, int xp)
    {
        characterName.text = character.Base.Name;
        currentLVL.text = "LvL: " + character.Level.ToString();
        currentXP.text = "XP: " + character.XP.ToString();
        nextLvlXP.text = "Next Lvl XP: " + character.NextLevel.ToString();
        movesText.text = "";

        yield return new WaitForSeconds(0.75f);
        if (alive)
        {
            character.AddXP(xp);
            currentXP.text = "XP: " + character.XP.ToString();
            while (character.CheckifLevelUp())
            {
                currentLVL.text = "LvL: " + character.Level.ToString();
                nextLvlXP.text = "Next Lvl XP: " + character.NextLevel.ToString();
                string newMove = character.CheckNewMove();
                if (newMove != "")
                {
                    movesText.text = "Learned Move: " + newMove;
                }
                yield return new WaitForSeconds(0.5f);
            }

        }
    }
}