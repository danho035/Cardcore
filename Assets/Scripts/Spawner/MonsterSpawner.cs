using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using static TileInfoGenerator;

public class MonsterSpawner : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static MonsterSpawner Instance { get; private set; }

    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }

    protected int bossMonsterCount = 0;
    protected int middleMonsterCount = 0;

    // 몬스터 타입 상수
    public int MonsterType { get; private set; }

    // Awake 메서드에서 싱글톤 인스턴스 설정
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

    // 기본 스폰 시그니처 메서드
    public virtual void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("타일 정보가 없습니다. 몬스터를 스폰할 수 없습니다.");
            return;
        }

        Debug.LogWarning("기본 MonsterSpawner의 SpawnMonster()가 호출되었습니다. 파생된 클래스에서 이 메서드를 오버라이드하세요");
    }

    // 보스 몬스터 카운터 증가 메서드
    public void IncreaseBossMonsterCount()
    {
        bossMonsterCount++;
    }

    // 미들 몬스터 카운터 증가 메서드
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

public class Boss : MonsterSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // 타일 정보가 없는 경우 기본 메서드에서 종료됨
        if (tileInfos == null || tileInfos.Count == 0) return;

        if (tileInfos[tileInfos.Count - 1].row != 4)
        {
            Debug.Log("최대 행이 5가 아니므로 보스 몬스터를 스폰할 수 없습니다.");
            return;
        }

        if (MonsterCount <= 0)
        {
            Debug.Log("보스 몬스터 카운터가 0 이하입니다. 스폰할 보스 몬스터가 없습니다.");
            return;
        }

        // 부모 오브젝트를 변경한 후 몬스터를 스폰
        GameObject bossMonster = Instantiate(prefab, spawnParent);

        // 부모 오브젝트의 위치를 변경하여 anchoredPosition을 설정
        RectTransform rectTransform = bossMonster.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(tileInfos[2].posX, tileInfos[2].posY);
            rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (기본 타일 width * 3) + (타일간격 * 2)
        }
        else
        {
            Debug.LogWarning("보스 몬스터의 RectTransform 컴포넌트를 찾을 수 없습니다.");
        }

        // 몬스터의 이름 설정
        bossMonster.name = $"Boss_Monster";

        // 몬스터의 이미지 컴포넌트를 가져와 투명도를 설정
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

        MonsterSpawner.Instance.IncreaseBossMonsterCount();
        Debug.Log("보스 몬스터를 스폰했습니다.");
    }
}

public class Middle : MonsterSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // 타일 정보가 없는 경우 기본 메서드에서 종료됨
        if (tileInfos == null || tileInfos.Count == 0) return;

        if (tileInfos[tileInfos.Count - 1].row != 3)
        {
            Debug.Log("최대 행이 4가 아니므로 미들 보스 몬스터를 스폰할 수 없습니다.");
            return;
        }

        if (MonsterCount <= 0)
        {
            Debug.Log("미들 보스 몬스터 카운터가 0 이하입니다. 스폰할 보스 몬스터가 없습니다.");
            return;
        }

        // 부모 오브젝트를 변경한 후 몬스터를 스폰
        GameObject middleMonster = Instantiate(prefab, spawnParent);

        // 부모 오브젝트의 위치를 변경하여 anchoredPosition을 설정
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

        // 몬스터의 이름 설정
        middleMonster.name = $"Middle_Monster";

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

        MonsterSpawner.Instance.IncreaseMiddleMonsterCount();
        Debug.Log("미들 보스 몬스터를 스폰했습니다.");
    }
}

public class Normal : MonsterSpawner
{
    // 중복 스폰 체크를 위한 리스트
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private TileInfoGenerator tileInfoGenerator;

    protected override void Awake()
    {
        base.Awake();
        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>(); // TileInfoGenerator 객체 참조
    }

    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        if (tileInfoGenerator == null)
        {
            Debug.LogWarning("TileInfoGenerator를 찾을 수 없습니다.");
            return;
        }

        List<TileInfo> tileList = tileInfoGenerator.GetTileInfos(); // tileList 가져오기

        // 타일 인포의 행과 열을 가져옵니다.
        int maxRow = tileList.Max(t => t.row);
        int maxColumn = tileList.Max(t => t.column);

        // 몬스터 카운터가 있는지 확인
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("노말 몬스터 카운터가 0 이하입니다. 스폰할 노말 몬스터가 없습니다.");
            return;
        }

        // 스폰 위치를 지정하는 리스트 초기화
        List<int> spawnedIndices = new List<int>();

        // 싱글톤 인스턴스를 통해 보스와 미들 보스 카운트 가져오기
        int bossCount = MonsterSpawner.Instance.GetBossMonsterCount();
        int middleCount = MonsterSpawner.Instance.GetMiddleMonsterCount();

        // 보스스폰, 미들스폰이 전부 0일 경우
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

    // 스폰 규칙에 따라 몬스터 스폰하는 메서드
    private void SpawnMonsterWithRule(int maxRow, int maxColumn, bool bossOrMiddleSpawned, GameObject prefab, Transform spawnParent, List<int> spawnedIndices, List<TileInfo> tileList)
    {
        // 타일 인포 리스트에서 랜덤하게 타일을 선택하고 중복을 피하며 스폰
        for (int i = 0; i < MonsterCount; i++)
        {
            int randomIndex;
            int randomRow;
            int randomColumn;

            // 타일 인포 리스트에서 랜덤하게 타일 선택
            do
            {
                if (!bossOrMiddleSpawned)
                {
                    randomRow = Random.Range(0, maxRow + 1);
                    randomColumn = Random.Range(0, maxColumn);
                    Debug.Log("(생성된 난수 - 보스몬스터 존재하지 않음)" + "열: " + randomColumn  + "행: " + randomRow);
                }
                else
                {
                    // 보스 미들보스가 있을 경우 가장 마지막 열 값을 가진 타일을 제외하고 랜덤한 타일 선택
                    randomRow = Random.Range(0, maxRow + 1);
                    randomColumn = Random.Range(1, maxColumn); // 가장 마지막 열 값을 가진 타일을 제외
                    Debug.Log("(생성된 난수 - 보스몬스터 존재)" + "열: " + randomColumn + "행: " + randomRow);
                }

                // 선택된 행과 열에 해당하는 타일의 인덱스 찾기
                randomIndex = FindIndexByRowAndColumn(randomRow, randomColumn, tileList);

            } while (spawnedIndices.Contains(randomIndex));

            // 중복 스폰 체크 리스트에 추가
            spawnedIndices.Add(randomIndex);

            // 부모 오브젝트를 변경한 후 몬스터를 스폰
            GameObject normalMonster = Instantiate(prefab, spawnParent);

            // 몬스터의 이름 설정
            normalMonster.name = $"Normal_Monster_{i + 1}";

            // 몬스터 목록에 추가
            spawnedMonsters.Add(normalMonster);

            // 부모 오브젝트의 위치를 변경하여 anchoredPosition을 설정
            RectTransform rectTransform = normalMonster.GetComponent<RectTransform>();
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
                Debug.LogWarning($"노말 몬스터 {i + 1}의 RectTransform 컴포넌트를 찾을 수 없습니다.");
            }

            // 몬스터의 이미지 컴포넌트를 가져와 투명도를 설정
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

        // 노말 몬스터를 스폰했으므로 카운터 초기화
        bossMonsterCount = 0;
        middleMonsterCount = 0;
        Debug.Log("노말 몬스터를 모두 스폰했습니다. 카운터 초기화");
    }

    // 리스트에서 특정 행과 열 값과 일치하는 요소의 인덱스를 찾는 함수
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