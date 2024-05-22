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

        SpawnArea spawnArea = SpawnArea.Grassland;

        var (normalMonsters, middleMonsters, bossMonsters) = SelectingMonsters(spawnArea, monsterStats);

        NormalSelected.Clear();
        foreach (string normalMonster in normalMonsters)
        {
            Debug.Log("노말 몬스터 딕셔너리 추가.");
            NormalSelected.Add(normalMonster, monsterStats[normalMonster]);
        }

        MiddleSelected.Clear();
        foreach (string middleMonster in middleMonsters)
        {
            Debug.Log("미들 보스 몬스터 딕셔너리 추가.");
            MiddleSelected.Add(middleMonster, monsterStats[middleMonster]);
        }

        BossSelected.Clear();
        foreach (string bossMonster in bossMonsters)
        {
            Debug.Log("보스 몬스터 딕셔너리 추가.");
            BossSelected.Add(bossMonster, monsterStats[bossMonster]);
        }

        // 타일 생성은 전투 생성이 완료된 후에 호출되어야 합니다.
        Debug.Log("전투 생성이 완료되어 타일을 생성합니다.");
        tileGenerator.rows = rows;
        tileGenerator.columns = columns;
        spawnManager.bossCount = bossCount;
        spawnManager.middleCount = middleCount;
        spawnManager.normalCount = normalCount;

        // 반환된 몬스터 스탯을 출력
        foreach (var pair in NormalSelected)
        {
            Debug.Log("Normal Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }

        // 반환된 몬스터 스탯을 출력
        foreach (var pair in MiddleSelected)
        {
            Debug.Log("Middle Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
        }

        // 반환된 몬스터 스탯을 출력
        foreach (var pair in BossSelected)
        {
            Debug.Log("Boss Monster: " + pair.Key + ", Type: " + pair.Value.Type + ", SpawnArea: " + pair.Value.SpawnArea);
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
            Debug.LogWarning("BattleGenerator instance.");
        }
    }

    void Start()
    {
        GenerateBattle();
    }
}