using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // 생성할 적 오브젝트 프리팹 배열
    public int monsterCount; // 적 스폰 횟수
    public int allySpawnCount; // 아군 스폰 횟수
    private int playerSpawnCount = 1; // 플레이어 스폰 횟수

    void Start()
    {
        StartCoroutine(StartEnemySpawnDelayed()); // 일정 시간 뒤에 적 스폰 시작
    }

    IEnumerator StartEnemySpawnDelayed()
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 후에 실행

        TileInfoCheck(); // 타일 정보 가져오기
    }

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

                // 적 스폰
                EnemySpawn(tilePositions, monsterCount);
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

    void EnemySpawn(List<TileInfoGenerator.TileInfo> tilePositions, int monsterCount)
    {
        List<TileInfoGenerator.TileInfo> eligibleTiles = new List<TileInfoGenerator.TileInfo>(tilePositions);
        List<TileInfoGenerator.TileInfo> selectedTiles = new List<TileInfoGenerator.TileInfo>();

        // 타일 정보 리스트에서 가장 높은 행 값 찾기
        int highestRow = -1;
        foreach (TileInfoGenerator.TileInfo tileInfo in tilePositions)
        {
            if (tileInfo.row > highestRow)
            {
                highestRow = tileInfo.row;
            }
        }

        // 가장 높은 행 값을 제외한 모든 행을 가진 타일의 인덱스 찾기
        List<int> eligibleIndexes = new List<int>();
        for (int i = 0; i < tilePositions.Count; i++)
        {
            if (tilePositions[i].row != highestRow)
            {
                eligibleIndexes.Add(i);
            }
        }

        // 몬스터 스폰
        for (int i = 0; i < monsterCount; i++)
        {
            TileInfoGenerator.TileInfo spawnTile = null;

            // eligibleIndexes 리스트에서 중복되지 않는 랜덤한 인덱스 선택
            int randomIndex = Random.Range(0, eligibleIndexes.Count);
            int selectedIndex = eligibleIndexes[randomIndex];

            // 선택된 인덱스의 타일 정보 가져오기
            spawnTile = tilePositions[selectedIndex];

            // 해당 위치의 오브젝트 가져오기
            GameObject tileObject = GameObject.Find(spawnTile.name);

            if (tileObject != null)
            {
                // 해당 오브젝트의 TileCheck 스크립트를 가져옴
                TileCheck tileCheck = tileObject.GetComponent<TileCheck>();

                // 해당 오브젝트의 퍼블릭 동적 변수(MeleeAtk, RangeAtk, Player, Enemy) 전부 false인지 확인
                if (!TileCheck.MeleeAtk && !TileCheck.RangeAtk && !TileCheck.Player && !TileCheck.Enemy)
                {
                    // 디버그 로그 출력
                    Debug.Log("몬스터 " + (i + 1) + " 스폰 위치: " + spawnTile.name);

                    // 선택한 위치를 기록
                    selectedTiles.Add(spawnTile);

                    // 해당 위치에 몬스터 생성 (리스트의 x, y 좌표를 참조하여 생성)
                    GameObject monsterPrefab = monsterPrefabs[0]; // 프리팹 리스트의 첫 번째 프리팹 사용
                    Vector3 spawnPosition = new Vector3(spawnTile.x, spawnTile.y, 0f); // 타일의 x, y 좌표를 사용하여 스폰 위치 설정
                    Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, tileObject.transform); // 타일을 부모로 설정하여 생성
                }
            }
            else
            {
                // 해당 위치의 오브젝트를 찾을 수 없는 경우 오류 메시지 출력
                Debug.LogError("이름 '" + spawnTile.name + "'을 가진 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
}