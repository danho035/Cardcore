using UnityEngine;
using System.Collections.Generic;

public class StatManager : MonoBehaviour
{
    // ������ ������ ������ ��ųʸ�
    private Dictionary<string, UnitStats> monsterStatsDictionary = new Dictionary<string, UnitStats>();
    private Dictionary<string, UnitStats> playerStatsDictionary = new Dictionary<string, UnitStats>();

    // ���� ������ �߰��ϴ� �޼���
    public void AddMonsterStats(string name, UnitStats stats)
    {
        monsterStatsDictionary[name] = stats;
    }

    // ��� ������ ������ ��ȯ�ϴ� �޼���
    public Dictionary<string, UnitStats> GetMonsterStats()
    {
        return monsterStatsDictionary;
    }

    // Ư�� ������ ������ ��ȯ�ϴ� �޼���
    public UnitStats GetMonsterStats(string name)
    {
        if (monsterStatsDictionary.ContainsKey(name))
        {
            return monsterStatsDictionary[name];
        }
        else
        {
            Debug.LogError("�ش��ϴ� ������ ������ ã�� �� �����ϴ�: " + name);
            return null;
        }
    }

    // �÷��̾� ������ �߰��ϴ� �޼���
    public void AddPlayerStats(string name, UnitStats stats)
    {
        playerStatsDictionary[name] = stats;
    }

    // �÷��̾� ������ ��ȯ�ϴ� �޼���
    public Dictionary<string, UnitStats> GetPlayerStats()
    {
        return playerStatsDictionary;
    }

    // Ư�� �÷��̾��� ������ ��ȯ�ϴ� �޼���
    public UnitStats GetPlayerStats(string name)
    {
        if (playerStatsDictionary.ContainsKey(name))
        {
            return playerStatsDictionary[name];
        }
        else
        {
            Debug.LogError("�ش��ϴ� �÷��̾��� ������ ã�� �� �����ϴ�: " + name);
            return null;
        }
    }
}