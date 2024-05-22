using UnityEngine;
using System;
using System.Collections.Generic;

public class BattleGenerator : MonoBehaviour 
{
    public SpawnManager spawnManager;
    public TileGenerator tileGenerator;
    public StatManager statManager;

    public enum SpawnArea
    {
        Grassland = 1,
        Volcano,
        Mountain
    }

    public enum BattleType 
    {
        Normal = 1, 
        MiddleBoss, 
        Boss
    }

    public void GenerateBattle()
    {
        Debug.Log("���� ���� ���� ����");
        Dictionary<string, UnitStats> monsterStats = statManager.GetMonsterStats();

        int rows = 0;
        int columns = 0;
        int bossCount = 0;
        int middleCount = 0;
        int normalCount = 0;
        int unitType = 0;

        int battleTypeRan = UnityEngine.Random.Range(1, 4);
        BattleType battleType;

        if (battleTypeRan == 3)
        {
            battleType = BattleType.Boss;
        }
        else if (battleTypeRan == 2)
        {
            battleType = BattleType.MiddleBoss;
        }
        else
        {
            battleType = BattleType.Normal;
        }

        Debug.Log("���� ���� Ÿ��: " + battleType);
        SpawnArea spawnArea = SpawnArea.Grassland;
        Debug.Log("���� ����: " + spawnArea);

        Debug.Log("������ ���� - ����: " + battleType + ", ���� ����: " + spawnArea);

        switch (battleType)
        {
            case BattleType.Boss:
                rows = 5;
                columns = 5;
                bossCount = 1;
                unitType = SpawnManager.UnitType.Boss;
                break;

            case BattleType.MiddleBoss:
                rows = 4;
                columns = 4;
                middleCount = 1;
                unitType = SpawnManager.UnitType.Middle;
                break;

            case BattleType.Normal:
                rows = UnityEngine.Random.Range(3, 6);
                columns = rows;
                normalCount = UnityEngine.Random.Range(3, rows + columns - 2);
                unitType = SpawnManager.UnitType.Normal;
                break;
        }

        // SelectedMonster ��ųʸ� �ʱ�ȭ
        Dictionary<string, UnitStats> SelectedMonster = new Dictionary<string, UnitStats>();

        // SelectingMonsters �Լ� ȣ���Ͽ� ��ȯ�� ���� ����Ʈ ���
        (var normalMonsters, var middleMonsters, var bossMonsters) = SelectingMonsters(spawnArea, monsterStats);

        // �Ϲ� ���� �߰�
        foreach (string normalMonster in normalMonsters)
        {
            SelectedMonster.Add(normalMonster, monsterStats[normalMonster]);
        }

        // �߰� ���� �߰�
        foreach (string middleMonster in middleMonsters)
        {
            SelectedMonster.Add(middleMonster, monsterStats[middleMonster]);
        }

        // ���� ���� �߰�
        foreach (string bossMonster in bossMonsters)
        {
            SelectedMonster.Add(bossMonster, monsterStats[bossMonster]);
        }


        // Ÿ�� ������ ���� ������ �Ϸ�� �Ŀ� ȣ��Ǿ�� �մϴ�.
        Debug.Log("���� ������ �Ϸ�Ǿ� Ÿ���� �����մϴ�.");
        tileGenerator.rows = rows;
        tileGenerator.columns = columns;
        spawnManager.bossCount = bossCount;
        spawnManager.middleCount = middleCount;
        spawnManager.normalCount = normalCount;
    }

    private (List<string> normalMonsters, List<string> middleMonsters, List<string> bossMonsters) SelectingMonsters(SpawnArea spawnArea, Dictionary<string, UnitStats> monsterStats)
    {
        List<string> normalMonsters = new List<string>();
        List<string> middleMonsters = new List<string>();
        List<string> bossMonsters = new List<string>();

        foreach (KeyValuePair<string, UnitStats> pair in monsterStats)
        {
            if (pair.Value.SpawnArea == (int)spawnArea || pair.Value.SpawnArea == 0)
            {
                if (pair.Value.Type == 1)
                {
                    normalMonsters.Add(pair.Key);
                }
                else if (pair.Value.Type == 2)
                {
                    middleMonsters.Add(pair.Key);
                }
                else
                {
                    bossMonsters.Add(pair.Key);
                }
            }
        }

        return (normalMonsters, middleMonsters, bossMonsters);
    }

    void Start()
    {
        GenerateBattle();
    }
}