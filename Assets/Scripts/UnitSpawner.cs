using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public int enemySpawnCount; // 적 스폰 횟수
    public int allySpawnCount; // 아군 스폰 횟수
    private int playerSpanwCount = 1; // 플레이어 스폰 횟수

    void TileInfoCheck() // 타일 정보 가져오기
    {
        TileInfoGenerator tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();

        if (tileInfoGenerator != null)
        {
            // 타일 정보 리스트를 가져옵니다.
            List<TileInfoGenerator.TileInfo> tilePositions = tileInfoGenerator.GetTilePositions();

            if (tilePositions != null)
            {
                int count = tilePositions.Count;

                Debug.Log("타일 정보를 성공적으로 가져왔습니다 - UnitSpawner");
                Debug.Log("가져온 인덱스 개수: " + count);
            }
            else
            {
                Debug.LogError("타일 정보 리스트를 가져올 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("TileInfoGenerator 스크립트를 가진 게임 오브젝트를 찾을 수 없습니다.");
        }

    }

    void EnemySpawn(List<TileInfoGenerator.TileInfo> tilePositions)
    {
        if (tilePositions != null && tilePositions.Count > 0)
        {
            // 가장 작은 행 숫자와 가장 높은 행 숫자를 초기화합니다.
            int minRow = int.MaxValue;
            int maxRow = int.MinValue;

            // 가장 작은 행 숫자와 가장 높은 행 숫자를 찾습니다.
            foreach (TileInfoGenerator.TileInfo info in tilePositions)
            {
                if (info.row < minRow)
                {
                    minRow = info.row;
                }
                if (info.row > maxRow)
                {
                    maxRow = info.row;
                }
            }

            // 가장 높은 행 숫자는 스폰 위치에서 배제합니다.
            maxRow--;

            // 스폰할 위치를 결정합니다.
            List<TileInfoGenerator.TileInfo> spawnPositions = new List<TileInfoGenerator.TileInfo>();
            foreach (TileInfoGenerator.TileInfo info in tilePositions)
            {
                if (info.row != maxRow)
                {
                    spawnPositions.Add(info);
                }
            }

            // 스폰할 위치에서 랜덤하게 선택합니다.
            if (spawnPositions.Count > 0)
            {
                TileInfoGenerator.TileInfo spawnTile = spawnPositions[Random.Range(0, spawnPositions.Count)];

                Debug.Log("적이 스폰되었습니다. 위치: " + spawnTile.name);
                // 여기에 적 스폰 로직을 추가하세요.
            }
            else
            {
                Debug.LogError("적을 스폰할 수 있는 위치가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("타일 정보 리스트를 가져올 수 없거나 비어 있습니다.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TileInfoCheck", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}