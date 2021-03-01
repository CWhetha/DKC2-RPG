using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    Start, FindNextUser, PlayerMove, PlayerAction, PlayerSelectionAttack, PlayerItem, PlayerSelectionMagic, PlayerSelectionItem, PlayerSelectAll, PlayerItemSelectAll, EnemyMove, PerformMove, Flee, End
}

public enum SelectionType
{
    Magic, Attack, Item
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] public List<BattleUnit> playerUnits;
    [SerializeField] public List<BattleUnit> enemyUnits;
    [SerializeField] public List<BattleUnit> ActiveEnemyUnits;
    [SerializeField] public BattleUnit bossUnit;

    [SerializeField] public HUDActions dialogbox;
    [SerializeField] public FightEndScreen EndScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentTarget;
    int currentItem;
    int checkpriority;

    private List<Character> playerParty;
    private List<Item> partyItems;
    private MapArea enemyPool;

    BattleUnit activeUnit;
    int moveAdd = 0;
    int itemAdd = 0;

    EnemyType battletype;

    public AudioClip winclip;
    public AudioClip battleClip;
    public AudioClip bossClip;

    public event Action<AudioClip,bool> StartMusic;

    public void StartBattle(MapArea enemyPool, EnemyType type)
    {
        ActiveEnemyUnits = new List<BattleUnit>();
        this.playerParty = PlayerParty.Characters;
        this.partyItems = PlayerParty.Items;
        this.enemyPool = enemyPool;
        battletype = type;
        dialogbox.UpdateBP(PlayerParty.BP, PlayerParty.MaxBP);
        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        for (int i = 0; i < playerParty.Count; i++)
        {
            if (i < 4)
            {
                playerUnits[i].Setup(playerParty[i]);
                playerUnits[i].Time = 100;
                playerUnits[i].HUD.SetData(playerUnits[i].Character);
                if (playerUnits[i].Character.HP == 0)
                {
                    playerUnits[i].FaintAnimation();
                }
            }
        }
        if (battletype == EnemyType.Boss)
        {
            StartMusic(bossClip,true);
            bossUnit.Show(true);
            bossUnit.Setup(enemyPool.GetBossCharacter());
            bossUnit.Time = 100;
            ActiveEnemyUnits.Add(bossUnit);
        }
        else
        {
            StartMusic(battleClip,true);
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                enemyUnits[i].Show(true);
                enemyUnits[i].Setup(enemyPool.GetRandomCharacter());
                enemyUnits[i].Time = 100;
                ActiveEnemyUnits.Add(enemyUnits[i]);
            }
        }

        checkpriority = 0;
        yield return new WaitForSeconds(0.5f);
        state = BattleState.FindNextUser;
    }

    public bool CheckWinner()
    {
        if (getActiveAllies().Count == 0)
        {
            dialogbox.EnableActiveCharacter(false);
            StartCoroutine(EndGame(false));
            state = BattleState.PerformMove;
            return true;
        }
        else if (getActiveEnemies().Count == 0)
        {
            StartMusic(winclip,false);
            StartCoroutine(EndGame(true));
            state = BattleState.PerformMove;
            return true;
        }
        return false;

    }

    IEnumerator EndGame(bool win)
    {
        yield return new WaitForSeconds(1f);
        if (win)
        {
            int xp = 0;
            int bananas = 0;
            for (int i = 0; i < ActiveEnemyUnits.Count; i++)
            {
                if (ActiveEnemyUnits[i].Character.HP == 0)
                {
                    xp += ActiveEnemyUnits[i].Character.MonsterXP;
                    bananas += ActiveEnemyUnits[i].Character.MonsterBananas;
                }
                enemyUnits[i].Show(false);

            }
            for (int i = 0; i < playerParty.Count; i++)
            {
                playerParty[i].ResetBoosts();
            }

            EndScreen.Show(true);
            EndScreen.Setup(playerParty, xp, bananas);
            PlayerParty.Bananas += bananas;
        }
        else
        {
            OnBattleOver(false);
        }
        state = BattleState.End;
    }

    public void PriorityUnit()
    {
        List<BattleUnit> activeUnits = getAllUnits();

        bool found = false;
        bool win = CheckWinner();

        if (win == false)
        {
            while (found == false)
            {
                if (checkpriority >= activeUnits.Count)
                {
                    checkpriority = 0;
                }
                for (int i = checkpriority; checkpriority < activeUnits.Count; ++checkpriority)
                {
                    if (activeUnits[checkpriority].Character.HP != 0)
                    {
                        activeUnits[checkpriority].Time -= Mathf.FloorToInt(activeUnits[checkpriority].Character.Speed * UnityEngine.Random.Range(0.8f, 1f));
                        if (activeUnits[checkpriority].Time <= 0)
                        {
                            found = true;
                            activeUnit = activeUnits[checkpriority];
                            break;
                        }
                    }
                }
            }

            activeUnit.Arrow.SetActive(true);
            activeUnit.Character.IsGaurd = false;
            dialogbox.SetActiveCharacter(activeUnit);
            activeUnit.Time = 100;

            if (activeUnit.IsAlly)
            {
                dialogbox.SetMoveNames(activeUnit.Character.Moves, 0);
                dialogbox.SetItemNames(getAllItems(), 0);
                dialogbox.SetTargetNames(getActiveEnemies());
                ActionSelection();
            }
            else
            {
                StartCoroutine(EnemyMove());
            }
        }
    }

    void ActionSelection()
    {
        state = BattleState.PlayerAction;
        dialogbox.EnableActionBox(true);
    }

    void MoveSelection()
    {
        state = BattleState.PlayerMove;
        dialogbox.EnableActionBox(false);
        dialogbox.EnableActionOptionsBox(true);
    }

    void ItemSelection()
    {
        state = BattleState.PlayerItem;
        dialogbox.EnableActionBox(false);
        dialogbox.EnableItemOptionsBox(true);
    }

    void Flee()
    {
        state = BattleState.Flee;
        dialogbox.EnableActionBox(false);
    }

    void PlayerGaurd()
    {
        activeUnit.Character.IsGaurd = true;
        dialogbox.EnableActionBox(false);
        activeUnit.Arrow.SetActive(false);
        state = BattleState.FindNextUser;
    }

    void PlayerAttack()
    {
        state = BattleState.PlayerSelectionAttack;
        dialogbox.EnableActionBox(false);
        dialogbox.EnableTargetsBox(true);
    }

    void TargetSelection(Targets target, SelectionType type)
    {
        if (target == Targets.Ally || target == Targets.Enemy || target == Targets.AllyFainted)
        {
            if (type == SelectionType.Item)
            {
                state = BattleState.PlayerSelectionItem;
            }
            else
            {
                state = BattleState.PlayerSelectionMagic;
            }
        }
        else if (target == Targets.Enemies || target == Targets.Allies || target == Targets.All || target == Targets.AlliesFainted)
        {
            if (type == SelectionType.Item)
            {
                state = BattleState.PlayerItemSelectAll;
            }
            else
            {
                state = BattleState.PlayerSelectAll;
            }
        }
        dialogbox.EnableActionOptionsBox(false);
        dialogbox.EnableTargetsBox(true);
    }

    public IEnumerator PerformPlayerMove(SelectionType type, List<BattleUnit> targets)
    {
        state = BattleState.PerformMove;
        targets[currentTarget].Target.SetActive(false);

        if (type == SelectionType.Magic)
        {
            var move = activeUnit.Character.Moves[currentMove];
            SpendCost(move);
            //activeUnit.PlayAttackAnimation();
            StartCoroutine(dialogbox.DisplayText(move.Base.Name, 1.5f));
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(RunMove(activeUnit, targets[currentTarget], move));

        }
        else if (type == SelectionType.Attack)
        {
            if (CheckIfAttackHits(activeUnit.Character, targets[currentTarget].Character))
            {
                yield return new WaitForSeconds(0.25f);
                var damageDetails = targets[currentTarget].Character.TakeAttackDamage(activeUnit.Character);
                StartCoroutine(targets[currentTarget].AttackedPhysicalAnimation());
                StartCoroutine(targets[currentTarget].Damage.setDamageDetails(damageDetails.Damage, damageDetails.Critical));
                yield return new WaitForSeconds(0.5f);
                CheckIfFainted(targets[currentTarget], damageDetails);
            }
            else
            {
                StartCoroutine(targets[currentTarget].Damage.setMiss());
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (type == SelectionType.Item)
        {
            Item item = getAllItems()[currentItem];
            StartCoroutine(dialogbox.DisplayText(item.Base.Name, 1.5f));
            item.UseItem(1);
            yield return new WaitForSeconds(0.75f);
            StartCoroutine(RunItem(targets[currentTarget], item));
        }
        yield return new WaitForSeconds(0.75f);
        activeUnit.Arrow.SetActive(false);
        state = BattleState.FindNextUser;
    }

    public IEnumerator PerformPlayerMoveTargetAll(List<BattleUnit> targets, SelectionType type)
    {
        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].Target.SetActive(false);
        }

        state = BattleState.PerformMove;
        if (type == SelectionType.Item)
        {
            Item item = getAllItems()[currentItem];
            StartCoroutine(dialogbox.DisplayText(item.Base.Name, 1.5f));
            item.UseItem(1);

            yield return new WaitForSeconds(0.75f);

            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(RunItem(targets[i], item));
            }

            if (item.Base.Type == ItemType.BP || item.Base.Type == ItemType.Both)
            {
                GainBP(item);
                dialogbox.UpdateBP(PlayerParty.BP, PlayerParty.MaxBP);
            }
        }
        else
        {
            Move move = activeUnit.Character.Moves[currentMove];
            StartCoroutine(dialogbox.DisplayText(move.Base.Name, 1.5f));
            SpendCost(move);

            yield return new WaitForSeconds(0.75f);

            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(RunMove(activeUnit, targets[i], move));
            }
        }
        yield return new WaitForSeconds(0.75f);

        activeUnit.Arrow.SetActive(false);
        state = BattleState.FindNextUser;
    }

    public IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;
        yield return new WaitForSeconds(0.75f);
        var move = activeUnit.Character.GetRandomMove();
        StartCoroutine(dialogbox.DisplayText(move.Base.Name, 1.5f));
        //activeUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        List<BattleUnit> targets = getActiveAllies();

        if (move.Base.Target == Targets.Ally || move.Base.Target == Targets.Allies)
        {
            targets = getActiveEnemies();
        }

        if (move.Base.Target == Targets.Enemies || move.Base.Target == Targets.Allies)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(RunMove(activeUnit, targets[i], move));
            }
        }
        else if (move.Base.Target == Targets.Enemy || move.Base.Target == Targets.Ally)
        {
            BattleUnit target = targets[UnityEngine.Random.Range(0, targets.Count)];
            StartCoroutine(RunMove(activeUnit, target, move));
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitForSeconds(0.75f);

        activeUnit.Arrow.SetActive(false);
        state = BattleState.FindNextUser;
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        if (move.Base.SpellType == Type.Heal)
        {
            var healDetails = targetUnit.Character.TakeHeal(move, sourceUnit.Character);
            StartCoroutine(targetUnit.AttackedAnimation(move.Base.Sprite));
            StartCoroutine(targetUnit.Damage.setHealDetails(healDetails.Damage));
            if (move.Base.Target == Targets.AllyFainted || move.Base.Target == Targets.AlliesFainted)
            {
                targetUnit.ReviveAnimation();
            }
        }
        else if (move.Base.SpellType == Type.Damage)
        {
            if (CheckIfMoveHits(move, sourceUnit.Character, targetUnit.Character))
            {
                var damageDetails = targetUnit.Character.TakeDamage(move, sourceUnit.Character);
                StartCoroutine(targetUnit.AttackedAnimation(move.Base.Sprite));
                StartCoroutine(targetUnit.Damage.setDamageDetails(damageDetails.Damage, damageDetails.Critical));
                yield return new WaitForSeconds(0.5f);
                CheckIfFainted(targetUnit, damageDetails);
            }
            else
            {
                StartCoroutine(targetUnit.Damage.setMiss());
                yield return new WaitForSeconds(0.5f);
            }

        }
        else if (move.Base.SpellType == Type.Buff)
        {
            if ((sourceUnit.IsAlly == targetUnit.IsAlly) || CheckIfMoveHits(move, sourceUnit.Character, targetUnit.Character))
            {
                if (move.Base.Effects.Boosts != null)
                {
                    StartCoroutine(targetUnit.AttackedAnimation(move.Base.Sprite));
                    targetUnit.Character.ApplyBoost(move.Base.Effects.Boosts);
                }
            }
            else
            {
                StartCoroutine(targetUnit.Damage.setMiss());
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (targetUnit.IsAlly)
        {
            targetUnit.HUD.UpdateStats();
        }
    }

    IEnumerator RunItem(BattleUnit targetUnit, Item item)
    {
        if (item.Base.Type == ItemType.Buff)
        {
            if (item.Base.Effects.Boosts != null)
            {
                StartCoroutine(targetUnit.AttackedAnimation(item.Base.Sprite));
                targetUnit.Character.ApplyBoost(item.Base.Effects.Boosts);
            }
        }
        else
        {
            StartCoroutine(targetUnit.AttackedAnimation(item.Base.Sprite));
            if (item.Base.Type == ItemType.HP || item.Base.Type == ItemType.Both)
            {
                var healDetails = targetUnit.Character.TakeItemHeal(item);
                StartCoroutine(targetUnit.Damage.setHealDetails(healDetails.Damage));
            }
            if (item.Base.Targets == Targets.AllyFainted || item.Base.Targets == Targets.AlliesFainted)
            {
                targetUnit.ReviveAnimation();
            }
        }
        yield return new WaitForSeconds(0.5f);

        if (targetUnit.IsAlly)
        {
            targetUnit.HUD.UpdateStats();
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.FindNextUser)
        {
            PriorityUnit();
        }
        else if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PlayerSelectionMagic)
        {
            HandleTargetSelection(SelectionType.Magic);
        }
        else if (state == BattleState.PlayerSelectionItem)
        {
            HandleTargetSelection(SelectionType.Item);
        }
        else if (state == BattleState.PlayerItemSelectAll)
        {
            HandleTargetSelectionAll(SelectionType.Item);
        }
        else if (state == BattleState.PlayerSelectAll)
        {
            HandleTargetSelectionAll(SelectionType.Magic);
        }
        else if (state == BattleState.PlayerSelectionAttack)
        {
            HandleTargetSelection(SelectionType.Attack);
        }
        else if (state == BattleState.PlayerItem)
        {
            HandleItemSelection();
        }
        else if (state == BattleState.Flee)
        {
            FleeBattle();
        }
        else if (state == BattleState.End)
        {
            EndScreen.HandleUpdate();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentAction;
        }
        currentAction = Mathf.Clamp(currentAction, 0, 4);

        dialogbox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentAction == 0)
            {
                PlayerAttack();
            }
            if (currentAction == 1)
            {
                MoveSelection();
            }
            if (currentAction == 2)
            {
                if (getAllItems().Count != 0)
                {
                    ItemSelection();
                }
                else
                {
                    StartCoroutine(dialogbox.DisplayText("No Items To Use", 2f));
                }
            }
            if (currentAction == 3)
            {
                PlayerGaurd();
            }
            if (currentAction == 4)
            {
                Flee();
            }
        }
    }

    void FleeBattle()
    {
        if (battletype == EnemyType.Enemy)
        {
            List<BattleUnit> allies = getActiveAllies();
            List<BattleUnit> enemies = getActiveEnemies();

            float allySpeed = 0;
            float enemySpeed = 1;

            for (int i = 0; i < allies.Count; ++i)
            {
                allySpeed += allies[i].Character.GetStat(Stat.Speed);
            }
            for (int i = 0; i < enemies.Count; ++i)
            {
                enemySpeed += enemies[i].Character.GetStat(Stat.Speed);
            }
            float escape = 50f * (allySpeed / enemySpeed);

            if (UnityEngine.Random.Range(1, 101) <= escape)
            {
                for (int i = 0; i < allies.Count; ++i)
                {
                    allies[i].FleeAnimation();
                }
                StartCoroutine(dialogbox.DisplayText("Escaped", 1f));
                StartCoroutine(EndGame(true));
                state = BattleState.PerformMove;
            }
            else
            {
                StartCoroutine(dialogbox.DisplayText("Failed To Escape", 1f));
                state = BattleState.FindNextUser;
            }
            activeUnit.Arrow.SetActive(false);
        }
        else
        {
            StartCoroutine(dialogbox.DisplayText("Can't Flee", 1f));
            ActionSelection();
        }
    }

    void HandleMoveSelection()
    {
        int move;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentMove;
        }

        currentMove = Mathf.Clamp(currentMove, 0, activeUnit.Character.Moves.Count - 1);

        if (activeUnit.Character.Moves.Count > 4 && currentMove > 3)
        {
            moveAdd = currentMove - 3;
            dialogbox.SetMoveNames(activeUnit.Character.Moves, moveAdd);
        }
        else
        {
            moveAdd = 0;
            dialogbox.SetMoveNames(activeUnit.Character.Moves, moveAdd);
        }
        move = currentMove > 3 ? 3 : currentMove;
        dialogbox.UpdateMoveSelection(PlayerParty.BP, move, activeUnit.Character.Moves[currentMove]);
        Move chosenMove = activeUnit.Character.Moves[currentMove];
        if (Input.GetKeyDown(KeyCode.E) && ((chosenMove.Base.Target == Targets.AllyFainted || chosenMove.Base.Target == Targets.AlliesFainted) && getFaintedAllies().Count == 0))
        {
            StartCoroutine(dialogbox.DisplayText("No Fainted Allies", 1f));
        }
        else if (Input.GetKeyDown(KeyCode.E) && chosenMove.Base.BPCost <= PlayerParty.BP)
        {
            TargetSelection(chosenMove.Base.Target, SelectionType.Magic);
        }
        else if (Input.GetKeyDown(KeyCode.E) && chosenMove.Base.BPCost > PlayerParty.BP)
        {
            StartCoroutine(dialogbox.DisplayText("Not Enough BP", 2f));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dialogbox.EnableActionOptionsBox(false);
            ActionSelection();
        }

    }

    void HandleItemSelection()
    {
        int item;
        List<Item> avalibleItems = getAllItems();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentItem;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentItem;
        }

        currentItem = Mathf.Clamp(currentItem, 0, avalibleItems.Count - 1);
        if (avalibleItems.Count > 4 && currentItem > 3)
        {
            itemAdd = currentItem - 3;
            dialogbox.SetItemNames(avalibleItems, itemAdd);
        }
        else
        {
            itemAdd = 0;
            dialogbox.SetItemNames(avalibleItems, itemAdd);
        }

        item = currentItem > 3 ? 3 : currentItem;
        dialogbox.UpdateItemSelection(item, avalibleItems[currentItem]);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if ((avalibleItems[currentItem].Base.Targets == Targets.AllyFainted || avalibleItems[currentItem].Base.Targets == Targets.AlliesFainted) && getFaintedAllies().Count == 0)
            {
                StartCoroutine(dialogbox.DisplayText("No Fainted Allies", 1f));
            }
            else
            {
                dialogbox.EnableItemOptionsBox(false);
                TargetSelection(avalibleItems[currentItem].Base.Targets, SelectionType.Item);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dialogbox.EnableItemOptionsBox(false);
            ActionSelection();
        }

    }

    void HandleTargetSelection(SelectionType type)
    {
        List<BattleUnit> activeTargets = getActiveEnemies();
        if (type == SelectionType.Item)
        {
            List<Item> activeItems = getAllItems();
            if (activeItems[currentItem].Base.Targets == Targets.Ally)
            {
                activeTargets = getActiveAllies();
            }
            else if (activeItems[currentItem].Base.Targets == Targets.AllyFainted)
            {
                activeTargets = getFaintedAllies();
            }
        }
        else if (type == SelectionType.Magic)
        {
            if (activeUnit.Character.Moves[currentMove].Base.Target == Targets.Ally)
            {
                activeTargets = getActiveAllies();
            }
            else if (activeUnit.Character.Moves[currentMove].Base.Target == Targets.AllyFainted)
            {
                activeTargets = getFaintedAllies();
            }
        }


        dialogbox.SetTargetNames(activeTargets);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentTarget;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --currentTarget;
        }
        currentTarget = Mathf.Clamp(currentTarget, 0, activeTargets.Count - 1);
        dialogbox.UpdateTargetSelection(currentTarget, activeTargets);

        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogbox.EnableTargetsBox(false);
            StartCoroutine(PerformPlayerMove(type, activeTargets));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activeTargets[currentTarget].Target.SetActive(false);
            dialogbox.EnableTargetsBox(false);
            if (type == SelectionType.Attack)
            {
                ActionSelection();
            }
            else if (type == SelectionType.Magic)
            {
                MoveSelection();
            }
            else if (type == SelectionType.Item)
            {
                ItemSelection();
            }
        }
    }

    void HandleTargetSelectionAll(SelectionType type)
    {
        List<BattleUnit> activeTargets = getActiveEnemies();
        if (type == SelectionType.Item)
        {
            List<Item> activeItems = getAllItems();
            if (activeItems[currentItem].Base.Targets == Targets.Allies)
            {
                activeTargets = getActiveAllies();
            }
            else if (activeItems[currentItem].Base.Targets == Targets.AlliesFainted)
            {
                activeTargets = getFaintedAllies();
            }
        }
        else
        {
            if (activeUnit.Character.Moves[currentMove].Base.Target == Targets.Allies)
            {
                activeTargets = getActiveAllies();
            }
            if (activeUnit.Character.Moves[currentMove].Base.Target == Targets.AlliesFainted)
            {
                activeTargets = getFaintedAllies();
            }
        }

        dialogbox.SetTargetNames(activeTargets);

        dialogbox.TargetMultipleSelection(activeTargets, true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogbox.EnableTargetsBox(false);
            StartCoroutine(PerformPlayerMoveTargetAll(activeTargets, type));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dialogbox.TargetMultipleSelection(activeTargets, false);
            dialogbox.EnableTargetsBox(false);
            if (type == SelectionType.Magic)
            {
                MoveSelection();
            }
            else if (type == SelectionType.Item)
            {
                ItemSelection();
            }
        }
    }

    bool CheckIfMoveHits(Move move, Character source, Character target)
    {
        if (move.Base.AlwaysHits)
        {
            return true;
        }
        float moveAccuracy = move.Base.Accuracy - (7 * (target.GetStat(Stat.Speed) / source.GetStat(Stat.Speed)));
        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    bool CheckIfAttackHits(Character source, Character target)
    {
        float moveAccuracy = 100 - (7 * (target.GetStat(Stat.Speed) / source.GetStat(Stat.Speed)));
        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    public List<BattleUnit> getActiveAllies()
    {
        List<BattleUnit> active = new List<BattleUnit>();
        for (int i = 0; i < playerUnits.Count; ++i)
        {
            if (playerUnits[i].Character.HP != 0)
            {
                active.Add(playerUnits[i]);
            }
        }
        return active;
    }

    public List<BattleUnit> getFaintedAllies()
    {
        List<BattleUnit> active = new List<BattleUnit>();
        for (int i = 0; i < playerUnits.Count; ++i)
        {
            if (playerUnits[i].Character.HP == 0)
            {
                active.Add(playerUnits[i]);
            }
        }
        return active;
    }

    public List<BattleUnit> getActiveEnemies()
    {
        List<BattleUnit> active = new List<BattleUnit>();
        for (int i = 0; i < ActiveEnemyUnits.Count; ++i)
        {
            if (ActiveEnemyUnits[i].Character.HP != 0)
            {
                active.Add(ActiveEnemyUnits[i]);
            }
        }
        return active;
    }

    public List<BattleUnit> getAllUnits()
    {
        List<BattleUnit> active = new List<BattleUnit>();
        for (int i = 0; i < playerUnits.Count; ++i)
        {
            active.Add(playerUnits[i]);
        }
        for (int i = 0; i < ActiveEnemyUnits.Count; ++i)
        {
            active.Add(ActiveEnemyUnits[i]);
        }
        return active;
    }

    public List<Item> getAllItems()
    {
        List<Item> active = new List<Item>();
        for (int i = 0; i < partyItems.Count; ++i)
        {
            if (partyItems[i].Quantity != 0)
            {
                active.Add(partyItems[i]);
            }
        }
        return active;
    }

    public void SpendCost(Move move)
    {
        PlayerParty.BP -= move.BPCost;
        dialogbox.UpdateBP(PlayerParty.BP, PlayerParty.MaxBP);
    }

    void GainBP(Item item)
    {
        int restore = item.Base.BpGain;
        if (item.Base.IsFullBP)
        {
            PlayerParty.BP = PlayerParty.MaxBP;
        }
        else
        {
            if (PlayerParty.BP + restore >= PlayerParty.MaxBP)
            {
                PlayerParty.BP = PlayerParty.MaxBP;
            }
            else
            {
                PlayerParty.BP += restore;
            }
        }
    }

    public void CheckIfFainted(BattleUnit attacked, DamageDetails damageDetails)
    {
        if (damageDetails.Fainted == true)
        {
            attacked.Target.SetActive(false);
            attacked.Character.ResetBoosts();
            attacked.FaintAnimation();
        }
    }
}
