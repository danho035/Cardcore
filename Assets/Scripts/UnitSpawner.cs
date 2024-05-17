using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator; // 타일 정보를 제공하는 TileInfoGenerator 스크립트를 참조

    // 타일 정보 리스트
    private List<TileInfoGenerator.TileInfo> tileInfos;

    // 몬스터 카운트
    public int Monster11Count;
    public int Monster21Count;
    public int Monster31Count;

    // 몬스터 프리팹
    public GameObject monster31Prefab; // 몬스터 유형 31의 프리팹
    public GameObject monster21Prefab; // 몬스터 유형 21의 프리팹
    public GameObject monster11Prefab; // 몬스터 유형 11의 프리팹

    void Start()
    {
        Invoke("GetTileInfosDelayed", 0.01f);
    }

    void GetTileInfosDelayed()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator가 설정되지 않았습니다.");
            return;
        }

        // 타일 정보 리스트 받아오기
        tileInfos = tileInfoGenerator.GetTileInfos();

        // 몬스터 스폰
        MonsterSpawn();
    }
    public void MonsterSpawn()
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogError("타일 정보가 없습니다.");
            return;
        }

        // 몬스터 타입 순서: 31 > 21 > 11
        int[] monsterCounts = { Monster31Count, Monster21Count, Monster11Count };
        for (int monsterType = 0; monsterType < monsterCounts.Length; monsterType++)
        {
            int count = monsterCounts[monsterType];
            if (count <= 0) continue;

            List<int> availableIndices = new List<int>();
            if (monsterType == 2) // 몬스터 타입이 11일 때
            {
                // 전체 타일 중에서 랜덤하게 선택
                for (int i = 0; i < tileInfos.Count; i++)
                {
                    availableIndices.Add(i);
                }
            }
            else // 몬스터 타입이 21이나 31일 때
            {
                // 가장 낮은 행의 리스트 인덱스만 사용
                int lowestRow = int.MaxValue;
                foreach (var tileInfo in tileInfos)
                {
                    if (tileInfo.row < lowestRow)
                    {
                        lowestRow = tileInfo.row;
                    }
                }

                for (int i = 0; i < tileInfos.Count; i++)
                {
                    if (tileInfos[i].row == lowestRow)
                    {
                        availableIndices.Add(i);
                    }
                }
            }

            // 가장 높은 행의 리스트 인덱스 배제
            int highestRow = int.MinValue;
            foreach (var tileInfo in tileInfos)
            {
                if (tileInfo.row > highestRow)
                {
                    highestRow = tileInfo.row;
                }
            }

            availableIndices.RemoveAll(index => tileInfos[index].row == highestRow);

            // 몬스터 스폰
            List<int> spawnIndices = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, availableIndices.Count);
                spawnIndices.Add(availableIndices[index]);
                availableIndices.RemoveAt(index);
            }

            // 인덱스를 기반으로 몬스터 스폰
            foreach (var spawnIndex in spawnIndices)
            {
                var tileInfo = tileInfos[spawnIndex];
                GameObject monsterPrefab;
                if (monsterType == 0)
                {
                    monsterPrefab = monster31Prefab;
                }
                else if (monsterType == 1)
                {
                    monsterPrefab = monster21Prefab;
                }
                else
                {
                    monsterPrefab = monster11Prefab;
                }

                // 프리팹 클론 생성 및 이름 붙이기
                GameObject monster = Instantiate(monsterPrefab, transform);
                monster.name = monsterPrefab.name + "_" + monsterType.ToString() + "_" + spawnIndex.ToString();

                RectTransform rectTransform = monster.GetComponent<RectTransform>();

                // 타일 정보에 따라 width, height 설정
                float width;
                float height;
                if (monsterType == 1)
                {
                    // 21타입인 경우
                    width = tileInfo.width * 2 + tileInfo.spacing;
                    height = tileInfo.height;
                }
                else if (monsterType == 0)
                {
                    // 31타입인 경우
                    width = tileInfo.width * 3 + tileInfo.spacing * 2;
                    height = tileInfo.height;
                }
                else
                {
                    // 11타입인 경우
                    width = tileInfo.width;
                    height = tileInfo.height;
                }

                // 프리팹 위치 설정
                float posX;
                if (monsterType == 1)
                {
                    posX = (tileInfos[spawnIndex].posX + tileInfos[spawnIndex + 1].posX) / 2;
                }
                else if (monsterType == 0)
                {
                    posX = tileInfos[spawnIndex + 1].posX;
                }
                else
                {
                    posX = tileInfo.posX;
                }
                rectTransform.sizeDelta = new Vector2(width, height);
                rectTransform.anchoredPosition = new Vector2(posX, tileInfo.posY);

                // TileCheck 스크립트 체크
                var tileCheck = monster.GetComponent<TileCheck>();
                if (tileCheck == null)
                {
                    Debug.LogError("Monster에 TileCheck 스크립트가 없습니다.");
                    return;
                }

                // 스폰된 몬스터의 이미지 컴포넌트를 가져와서 alpha 값을 변경
                var image = monster.GetComponent<Image>();
                Color color = image.color;
                color.a = 1f; // 최대 투명도로 설정
                image.color = color;
            }
        }
    }
}
