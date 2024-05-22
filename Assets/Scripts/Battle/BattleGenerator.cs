using UnityEngine;
using System.Collections.Generic;

public class BattleGenerator : MonoBehaviour
{
    public static BattleGenerator BattleInstance { get; private set; }

    public SpawnManager spawnManager;
    public TileGenerator tileGenerator;
    public StatManager statManager;

    public Dictionary<string, UnitStats> NormalSelected = new Dictionary<string, UnitStats>();
    public Dictionary<string, UnitStats> MiddleSelected = new Dictionary<string, UnitStats>();
    public Dictionary<string, UnitStats> BossSelected = new Dictionary<string, UnitStats>();
    public Dictionary<string, UnitStats> PlayerSelected = new Dictionary<string, UnitStats>();

    public int playerSelectNumber;

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

    public enum PlayerType
    {
        Knight = 1,
        Wizard,
        Thief
    }

    public void GenerateBattle()
    {
        Dictionary<string, UnitStats> monsterStats = statManager.GetMonsterStats();

        int rows = 0;
        int columns = 0;
        int bossCount = 0;
        int middleCount = 0;
        int normalCount = 0;
        int battleTypeRan = UnityEngine.Random.Range(1, 4);

        BattleType battleType = (BattleType)battleTypeRan;

        switch (battleType)
        {
            case BattleType.Boss:
                rows = 5;
                columns = 5;
                bossCount = 1;
                break;

            case BattleType.MiddleBoss:
                rows = 4;
                columns = 4;
                middleCount = 1;
                break;

            case BattleType.Normal:
                rows = UnityEngine.Random.Range(3, 5);
                columns = rows;
                normalCount = UnityEngine.Random.Range(3, rows + columns - 2);
                break;
        }

        PlayerType playerType = (PlayerType)playerSelectNumber;
        UnitStats selectedPlayerStats = null;
        string playerName = "";

        switch (playerType)
        {
            case PlayerType.Knight:
                playerName = "Knight";
                break;

            case PlayerType.Wizard:
                playerName = "Wizard";
                break;

            case PlayerType.Thief:
                playerName = "Thief";
                break;
        }

        if (!string.IsNullOrEmpty(playerName))
        {
            selectedPlayerStats = statManager.GetPlayerStats(playerName);
            if (selectedPlayerStats != null)
            {
                PlayerSelected.Clear();
                PlayerSelected.Add(playerName, selectedPlayerStats);
            }
            else
            {
                Debug.LogError("�ش� �÷��̾��� ������ ã�� �� �����ϴ�: " + playerName);
            }
        }
        else
        {
            Debug.LogError("��ȿ���� ���� �÷��̾� Ÿ���Դϴ�.");
        }

        SpawnArea spawnArea = SpawnArea.Grassland;

        var (normalMonsters, middleMonsters, bossMonsters) = SelectingMonsters(spawnArea, monsterStats);

        NormalSelected.Clear();
        foreach (string normalMonster in normalMonsters)
        {
            Debug.Log("�븻 ���� ��ųʸ� �߰�.");
            NormalSelected.Add(normalMonster, monsterStats[normalMonster]);
        }

        MiddleSelected.Clear();
        foreach (string middleMonster in middleMonsters)
        {
            Debug.Log("�̵� ���� ���� ��ųʸ� �߰�.");
            MiddleSelected.Add(middleMonster, monsterStats[middleMonster]);
        }

        BossSelected.Clear();
        foreach (string bossMonster in bossMonsters)
        {
            Debug.Log("���� ���� ��ųʸ� �߰�.");
            BossSelected.Add(bossMonster, monsterStats[bossMonster]);
        }

        Debug.Log("���� ������ �Ϸ�Ǿ� Ÿ���� �����մϴ�.");
        tileGenerator.rows = rows;
        tileGenerator.columns = columns;
        spawnManager.bossCount = bossCount;
        spawnManager.middleCount = middleCount;
        spawnManager.normalCount = normalCount;

        // ��ȯ�� ���� ������ ���
        foreach (var pair in NormalSelected)
        {
            Debug.Log("Normal Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }

        // ��ȯ�� ���� ������ ���
        foreach (var pair in MiddleSelected)
        {
            Debug.Log("Middle Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }

        // ��ȯ�� ���� ������ ���
        foreach (var pair in BossSelected)
        {
            Debug.Log("Boss Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }

        // ��ȯ�� �÷��̾� ������ ���
        foreach (var pair in PlayerSelected)
        {
            Debug.Log("Player: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }
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

    private void Awake()
    {
        if (BattleInstance == null)
        {
            BattleInstance = this;
        }
        else
        {
            Debug.LogWarning("BattleGenerator instance already exists. Destroying duplicate.");
            Destroy(gameObject); // �ߺ��� �ν��Ͻ��� �ı��մϴ�.
        }
    }

    void Start()
    {
        GenerateBattle();
    }
}