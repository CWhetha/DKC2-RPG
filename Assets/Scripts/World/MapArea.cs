using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Character> enemyCharacters;
    [SerializeField] Character BossCharacter;

    public Character GetRandomCharacter()
    {
        Character enemy = enemyCharacters[Random.Range(0, enemyCharacters.Count)].GetCopy();
        enemy.Init();
        return enemy;
    }

    public Character GetBossCharacter()
    {
        BossCharacter.Init();
        return BossCharacter;
    }
}
