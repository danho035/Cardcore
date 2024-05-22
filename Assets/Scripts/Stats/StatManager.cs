using UnityEngine;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{
    // 몬스터 스탯을 저장할 딕셔너리
    private Dictionary<string, UnitStats> monsterStatsDictionary = new Dictionary<string, UnitStats>();

    // 몬스터 스탯을 추가하는 메서드
    public void AddMonsterStats(string name, UnitStats stats)
    {
        monsterStatsDictionary[name] = stats;
    }

    // 모든 몬스터의 스탯을 반환하는 메서드
    public Dictionary<string, UnitStats> GetMonsterStats()
    {
        return monsterStatsDictionary;
    }

    // 특정 몬스터의 스탯을 반환하는 메서드
    public UnitStats GetMonsterStats(string name)
    {
        if (monsterStatsDictionary.ContainsKey(name))
        {
            return monsterStatsDictionary[name];
        }
        else
        {
            Debug.LogError("해당하는 몬스터의 스탯을 찾을 수 없습니다: " + name);
            return null;
        }
    }
}