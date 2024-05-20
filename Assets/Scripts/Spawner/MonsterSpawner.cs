using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MonsterSpawner : MonoBehaviour
{
    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }

    protected int bossMonsterCount = 0;
    protected int middleMonsterCount = 0;


    // 몬스터 타입 상수
    public int MonsterType { get; private set; }

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
}

public class Boss : MonsterSpawner
{
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

        IncreaseBossMonsterCount();
        Debug.Log("보스 몬스터를 스폰했습니다.");
    }

    // 보스 몬스터 카운터 증가 메서드
    protected void IncreaseBossMonsterCount()
    {
        bossMonsterCount++;
        Debug.Log("보스 카운트 업");
        Debug.Log("보스 카운트: " + bossMonsterCount);
    }
}

public class Middle : MonsterSpawner
{
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

        IncreaseMiddleMonsterCount();
        Debug.Log("미들 보스 몬스터를 스폰했습니다.");
    }

    // 미들 보스 몬스터 카운터 증가 메서드
    protected void IncreaseMiddleMonsterCount()
    {
        middleMonsterCount++;
        Debug.Log("미들보스 카운트 업");
        Debug.Log("미들보스 카운트: " + middleMonsterCount);
    }

}

public class Normal : MonsterSpawner
{
    // 중복 스폰 체크를 위한 리스트
    private List<int> spawnedIndices = new List<int>();
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // 타일 인포의 행과 열을 가져옵니다.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // 몬스터 카운터가 있는지 확인
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("노말 몬스터 카운터가 0 이하입니다. 스폰할 노말 몬스터가 없습니다.");
            return;
        }

        // 스폰 위치를 지정하는 리스트 초기화
        spawnedIndices.Clear();

        // 보스스폰, 미들스폰이 전부 0일 경우
        if (bossMonsterCount == 0 && middleMonsterCount == 0)
        {
            Debug.Log("보스 미들보스 없음");
            SpawnMonsterWithRule(maxRow, maxColumn, false, prefab, spawnParent);
        }
        else if (bossMonsterCount > 0 || middleMonsterCount > 0) // 보스스폰, 미들스폰이 둘 중 하나라도 1일 경우
        {
            Debug.Log("보스 미들보스 있음");
            SpawnMonsterWithRule(maxRow, maxColumn, true, prefab, spawnParent);
        }
    }

    // 스폰 규칙에 따라 몬스터 스폰하는 메서드
    private void SpawnMonsterWithRule(int maxRow, int maxColumn, bool bossOrMiddleSpawned, GameObject prefab, Transform spawnParent)
    {
        // 중복 스폰 체크를 위한 리스트 초기화
        spawnedIndices.Clear();
        spawnedMonsters.Clear();

        // 몬스터 스폰
        for (int i = 0; i < MonsterCount; i++)
        {
            int randomRow;
            int randomColumn;

            // 임의의 행과 열 선택
            if (!bossOrMiddleSpawned)
            {
                randomRow = Random.Range(0, maxRow);
                randomColumn = Random.Range(0, maxColumn);
            }
            else
            {
                randomRow = Random.Range(1, maxRow);
                randomColumn = Random.Range(0, maxColumn);
            }

            // 타일 인포 인덱스 번호 계산
            int index = randomRow * maxColumn + randomColumn;

            // 중복 스폰 체크
            while (spawnedIndices.Contains(index))
            {
                // 다시 랜덤한 위치 선택
                if (!bossOrMiddleSpawned)
                {
                    randomRow = Random.Range(0, maxRow);
                    randomColumn = Random.Range(0, maxColumn);
                }
                else
                {
                    randomRow = Random.Range(1, maxRow);
                    randomColumn = Random.Range(0, maxColumn);
                }

                index = randomRow * maxColumn + randomColumn;
            }

            // 중복 스폰 체크 리스트에 추가
            spawnedIndices.Add(index);

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
                rectTransform.anchoredPosition = new Vector2(tileInfos[index].posX, tileInfos[index].posY);
                // 사이즈 설정
                rectTransform.sizeDelta = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
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
}
