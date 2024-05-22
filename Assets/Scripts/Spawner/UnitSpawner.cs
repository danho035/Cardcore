using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using static TileInfoGenerator;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance { get; private set; }
    public static BattleGenerator BattleInstance { get; private set; }

    public BattleGenerator battleGenerator;

    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }

    protected int bossMonsterCount = 0;
    protected int middleMonsterCount = 0;

    public int MonsterType { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
        }
    }

    public void SetTileInfos(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        this.tileInfos = tileInfos;
    }

    public virtual void Spawning(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            return;
        }

        if (BattleGenerator.BattleInstance == null)
        {
            Debug.LogError("BattleGenerator.BattleInstance가 null입니다. 올바른 인스턴스를 설정하세요.");
            return;
        }
    }

    public void IncreaseBossMonsterCount()
    {
        bossMonsterCount++;
    }

    public void IncreaseMiddleMonsterCount()
    {
        middleMonsterCount++;
    }

    public int GetBossMonsterCount()
    {
        return bossMonsterCount;
    }

    public int GetMiddleMonsterCount()
    {
        return middleMonsterCount;
    }
}

public class Boss : UnitSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        Dictionary<string, UnitStats> bossMonsters = BattleGenerator.BattleInstance?.BossSelected;

        if (bossMonsters != null && bossMonsters.Count > 0)
        {
            List<string> keys = new List<string>(bossMonsters.Keys);
            int renKey = Random.Range(0, keys.Count);
            string Key = keys[renKey];

            if (tileInfos == null || tileInfos.Count == 0) return;

            if (MonsterCount <= 0) return;

            if (tileInfos[tileInfos.Count - 1].row != 4) return;

            GameObject bossMonster = Instantiate(prefab, spawnParent);
            ShowStats showStats = bossMonster.GetComponent<ShowStats>();

            if (showStats != null)
            {
                if (bossMonsters.ContainsKey(Key))
                {
                    UnitStats unitStats = bossMonsters[Key];
                    showStats.Name = unitStats.Name;
                    showStats.Type = unitStats.Type;
                    showStats.SpawnArea = unitStats.SpawnArea;
                    showStats.Level = unitStats.Level;
                    showStats.HP = unitStats.HP;
                    showStats.Atk = unitStats.Atk;
                    showStats.Def = unitStats.Def;
                    showStats.Shield = unitStats.Shield;
                    showStats.CritChance = unitStats.CritChance;
                    showStats.AtkSpeed = unitStats.AtkSpeed;
                }
                else
                {
                    Debug.LogError("Key not found in dictionary: " + Key);
                    return;
                }
            }
            else
            {
                Debug.LogWarning("ShowStats component not found in the instantiated prefab.");
                return;
            }

            RectTransform rectTransform = bossMonster.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(tileInfos[2].posX, tileInfos[2].posY);
                rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (기본 타일 width * 3) + (타일간격 * 2)
            }
            else
            {
                Debug.LogWarning("보스 몬스터의 RecTransform 컴포넌트를 찾을 수 없습니다.");
                return;
            }

            BoxCollider2D boxCollider2D = bossMonster.GetComponent<BoxCollider2D>();
            if (boxCollider2D != null)
            {
                boxCollider2D.size = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (기본 타일 width * 3) + (타일간격 * 2)
            }
            else
            {
                Debug.LogWarning("보스 몬스터의 BoxCollider2D 컴포넌트를 찾을 수 없습니다.");
                return;
            }

            bossMonster.name = $"Boss_Monster_" + Key;

            Image image = bossMonster.GetComponent<Image>();

            if (image != null)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
            else
            {
                Debug.LogWarning("보스 몬스터의 Image 컴포넌트를 찾을 수 없습니다.");
            }

            UnitSpawner.Instance.IncreaseBossMonsterCount();
            Debug.Log("보스 몬스터를 스폰했습니다.");
        }
        else
        {
            Debug.LogWarning("보스 몬스터 정보가 없습니다.");
        }

    }
}

public class Middle : UnitSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        Dictionary<string, UnitStats> middleMonsters = BattleGenerator.BattleInstance?.MiddleSelected;
        if (middleMonsters != null && middleMonsters.Count > 0)
        {
            List<string> keys = new List<string>(middleMonsters.Keys);
            int renKey = Random.Range(0, keys.Count);
            string Key = keys[renKey];

            if (tileInfos == null || tileInfos.Count == 0) return;

            if (MonsterCount <= 0) return;

            if (tileInfos[tileInfos.Count - 1].row != 3) return;

            GameObject middleMonster = Instantiate(prefab, spawnParent);
            ShowStats showStats = middleMonster.GetComponent<ShowStats>();

            if (showStats != null)
            {
                if (middleMonsters.ContainsKey(Key))
                {
                    UnitStats unitStats = middleMonsters[Key];
                    showStats.Name = unitStats.Name;
                    showStats.Type = unitStats.Type;
                    showStats.SpawnArea = unitStats.SpawnArea;
                    showStats.Level = unitStats.Level;
                    showStats.HP = unitStats.HP;
                    showStats.Atk = unitStats.Atk;
                    showStats.Def = unitStats.Def;
                    showStats.Shield = unitStats.Shield;
                    showStats.CritChance = unitStats.CritChance;
                    showStats.AtkSpeed = unitStats.AtkSpeed;
                }
                else
                {
                    Debug.LogError("Key not found in dictionary: " + Key);
                    return;
                }
            }
            else
            {
                Debug.LogWarning("ShowStats component not found in the instantiated prefab.");
                return;
            }

            RectTransform rectTransform = middleMonster.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float middlePosX = (tileInfos[1].posX + tileInfos[2].posX) / 2f;

                rectTransform.anchoredPosition = new Vector2(middlePosX, tileInfos[2].posY);
                rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 2) + tileInfos[0].spacing, tileInfos[0].tileSize.y); // (기본 타일 width * 2) + 타일간격
            }
            else
            {
                Debug.LogWarning("미들 보스 몬스터의 RectTransform 컴포넌트를 찾을 수 없습니다.");
            }

            BoxCollider2D boxCollider2D = middleMonster.GetComponent<BoxCollider2D>();
            if (boxCollider2D != null)
            {
                boxCollider2D.size = new Vector2((tileInfos[0].tileSize.x * 2) + tileInfos[0].spacing, tileInfos[0].tileSize.y); // (기본 타일 width * 2) + 타일간격
            }
            else
            {
                Debug.LogWarning("미들 보스 몬스터의 BoxCollider2D 컴포넌트를 찾을 수 없습니다.");
            }

            // 몬스터의 이름 설정
            middleMonster.name = $"Middle_Monster_" + Key;

            // 몬스터의 이미지 컴포넌트를 가져와 투명도를 설정
            Image image = middleMonster.GetComponent<Image>();
            if (image != null)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
            else
            {
                Debug.LogWarning("미들 보스 몬스터의 Image 컴포넌트를 찾을 수 없습니다.");
            }

            UnitSpawner.Instance.IncreaseMiddleMonsterCount();
            Debug.Log("미들 보스 몬스터를 스폰했습니다.");
        }
        else
        {
            Debug.LogWarning("미들 보스 몬스터 정보가 없습니다.");
        }
    }
}

public class Normal : UnitSpawner
{
    // 중복 스폰 체크를 위한 리스트
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private TileInfoGenerator tileInfoGenerator;

    protected override void Awake()
    {
        base.Awake();
        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        if (tileInfoGenerator == null) return;


        List<TileInfo> tileList = tileInfoGenerator.GetTileInfos();

        int maxRow = tileList.Max(t => t.row);
        int maxColumn = tileList.Max(t => t.column);

        if (MonsterCount <= 0) return;

        List<int> spawnedIndices = new List<int>();

        int bossCount = UnitSpawner.Instance.GetBossMonsterCount();
        int middleCount = UnitSpawner.Instance.GetMiddleMonsterCount();

        if (bossCount == 0 && middleCount == 0)
        {
            Debug.Log("보스 미들보스 없음");
            SpawnMonsterWithRule(maxRow, maxColumn, false, prefab, spawnParent, spawnedIndices, tileList);
        }
        else
        {
            Debug.Log("보스 미들보스 있음");
            SpawnMonsterWithRule(maxRow, maxColumn, true, prefab, spawnParent, spawnedIndices, tileList);
        }
    }

    private void SpawnMonsterWithRule(int maxRow, int maxColumn, bool bossOrMiddleSpawned, GameObject prefab, Transform spawnParent, List<int> spawnedIndices, List<TileInfo> tileList)
    {
        Dictionary<string, UnitStats> normalMonsters = BattleGenerator.BattleInstance?.NormalSelected;
        if (normalMonsters != null && normalMonsters.Count > 0)
        {
            List<string> keys = new List<string>(normalMonsters.Keys);

            for (int i = 0; i < MonsterCount; i++)
            {
                int randomIndex;
                int randomRow;
                int randomColumn;
                int renKey = Random.Range(0, keys.Count);
                string Key = keys[renKey];

                do
                {
                    if (!bossOrMiddleSpawned)
                    {
                        randomRow = Random.Range(0, maxRow + 1);
                        randomColumn = Random.Range(0, maxColumn);
                    }
                    else
                    {
                        randomRow = Random.Range(0, maxRow + 1);
                        randomColumn = Random.Range(1, maxColumn);
                    }

                    randomIndex = FindIndexByRowAndColumn(randomRow, randomColumn, tileList);

                } while (spawnedIndices.Contains(randomIndex));

                spawnedIndices.Add(randomIndex);



                GameObject normalMonster = Instantiate(prefab, spawnParent);
                RectTransform rectTransform = normalMonster.GetComponent<RectTransform>();
                BoxCollider2D boxCollider2D = normalMonster.GetComponent<BoxCollider2D>();
                ShowStats showStats = normalMonster.GetComponent<ShowStats>();

                normalMonster.name = "Normal_Monster_" + Key + $"{i + 1}";

                if (showStats != null)
                {
                    if (normalMonsters.ContainsKey(Key))
                    {
                        UnitStats unitStats = normalMonsters[Key];
                        showStats.Name = unitStats.Name;
                        showStats.Type = unitStats.Type;
                        showStats.SpawnArea = unitStats.SpawnArea;
                        showStats.Level = unitStats.Level;
                        showStats.HP = unitStats.HP;
                        showStats.Atk = unitStats.Atk;
                        showStats.Def = unitStats.Def;
                        showStats.Shield = unitStats.Shield;
                        showStats.CritChance = unitStats.CritChance;
                        showStats.AtkSpeed = unitStats.AtkSpeed;
                    }
                    else
                    {
                        Debug.LogError("Key not found in dictionary: " + Key);
                    }
                }
                else
                {
                    Debug.LogWarning("ShowStats component not found in the instantiated prefab.");
                }

                spawnedMonsters.Add(normalMonster);

                if (rectTransform != null)
                {
                    TileInfo tileInfo = tileList[randomIndex];
                    rectTransform.anchoredPosition = new Vector2(tileInfo.posX, tileInfo.posY);

                    rectTransform.sizeDelta = new Vector2(tileInfo.tileSize.x, tileInfo.tileSize.y);
                }
                else
                {
                    Debug.LogWarning($"노말 몬스터 {i + 1}의 RectTransform 컴포넌트를 찾을 수 없습니다.");
                }

                if (boxCollider2D != null)
                {
                    boxCollider2D.size = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
                }
                else
                {
                    Debug.LogWarning($"노말 몬스터 {i + 1}의 BoxCollider2D 컴포넌트를 찾을 수 없습니다.");
                }

                Image image = normalMonster.GetComponent<Image>();
                if (image != null)
                {
                    Color color = image.color;
                    color.a = 1f;
                    image.color = color;
                }
                else
                {
                    Debug.LogWarning($"노말 몬스터 {i + 1}의 Image 컴포넌트를 찾을 수 없습니다.");
                }

                Debug.Log($"노말 몬스터를 스폰했습니다: {i + 1}");
            }

            bossMonsterCount = 0;
            middleMonsterCount = 0;
            Debug.Log("노말 몬스터를 모두 스폰했습니다. 카운터 초기화");
        }
        else
        {
            Debug.LogWarning("노말 몬스터 정보가 없습니다.");
        }

    }

    public int FindIndexByRowAndColumn(int row, int column, List<TileInfo> tileList)
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].row == row && tileList[i].column == column)
            {
                return i;
            }
        }

        return -1;
    }
}

public class Player : UnitSpawner
{
    private TileInfoGenerator tileInfoGenerator;

    protected override void Awake()
    {
        base.Awake();
        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        List<TileInfo> tileList = tileInfoGenerator.GetTileInfos();

        // 타일 정보가 없는 경우 기본 메서드에서 종료됨
        if (tileInfos == null || tileInfos.Count == 0) return;

        int randomIndex;
        float maxRow = tileList.Max(t => t.row);
        int maxColumn = tileList.Max(t => t.column);
        int middleRow = (int)((maxRow - 1) / 2f + 1);

        if (maxRow == 3)
        {
            int randomValue = Random.Range(0, 2);

            middleRow = middleRow - randomValue;
        }

        randomIndex = FindIndexByRowAndColumn(middleRow, maxColumn, tileList);


        GameObject PlayerUnit = Instantiate(prefab, spawnParent);
        RectTransform rectTransform = PlayerUnit.GetComponent<RectTransform>();
        BoxCollider2D boxCollider2D = PlayerUnit.GetComponent<BoxCollider2D>();

        PlayerUnit.name = "Player_Unit";

        if (rectTransform != null)
        {
            // 인덱스를 사용하여 타일 인포의 위치 정보를 가져옴
            TileInfo tileInfo = tileList[randomIndex];
            rectTransform.anchoredPosition = new Vector2(tileInfo.posX, tileInfo.posY);
            // 사이즈 설정
            rectTransform.sizeDelta = new Vector2(tileInfo.tileSize.x, tileInfo.tileSize.y);
        }
        else
        {
            Debug.LogWarning("플레이어 유닛의 RectTransform 컴포넌트를 찾을 수 없습니다.");
        }

        if (boxCollider2D != null)
        {
            boxCollider2D.size = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
        }
        else
        {
            Debug.LogWarning("플레이어 유닛의 BoxCollider2D 컴포넌트를 찾을 수 없습니다.");
        }

        // 플레이어 유닛의 이미지 컴포넌트를 가져와 투명도를 설정
        Image image = PlayerUnit.GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Debug.LogWarning("플레이어 유닛의 Image 컴포넌트를 찾을 수 없습니다.");
        }

        Debug.Log("플레이어 유닛을 스폰했습니다");
    }

    public int FindIndexByRowAndColumn(int row, int column, List<TileInfo> tileList)
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            // 행과 열을 반대로 비교합니다.
            if (tileList[i].row == row && tileList[i].column == column)
            {
                return i; // 일치하는 요소의 인덱스 반환
            }
        }

        return -1; // 일치하는 요소가 없을 경우 -1 반환
    }
}