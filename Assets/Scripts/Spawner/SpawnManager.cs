using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // 유닛 타입 상수
    public static class UnitType
    {
        public const int Normal = 1;
        public const int Middle = 2;
        public const int Boss = 3;
        public const int Player = 4;
    }

    // 스폰 위치를 지정할 부모 오브젝트
    public Transform spawnParent;

    // 타일 인포를 불러올 부모 오브젝트 지정
    public TileInfoGenerator tileInfoGenerator;

    // 몬스터 프리팹들
    public GameObject bossPrefab;
    public GameObject middlePrefab;
    public GameObject normalPrefab;
    public GameObject playerPrefab;

    // 스폰에 필요한 소환 카운트
    public int bossCount;
    public int middleCount;
    public int normalCount;
    public int playerCount;

    // 스폰 스크립트 호출
    void Start()
    {
        // 0.1초 후에 스폰 스크립트 호출
        Invoke("SpawnMonsters", 0.1f);
    }

    void SpawnMonsters()
    {
        // TileInfoGenerator 컴포넌트가 null인지 확인
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // TileInfoGenerator 클래스의 GetTileInfos 메서드를 사용하여 타일 정보 가져오기
        var tileInfos = tileInfoGenerator.GetTileInfos();

        // 부모 오브젝트의 Transform 가져오기
        Transform spawnParentTransform = spawnParent.transform;

        Boss bossSpawner = spawnParent.gameObject.AddComponent<Boss>();
        bossSpawner.MonsterCount = bossCount;
        bossSpawner.SetTileInfos(tileInfos);
        bossSpawner.Spawning(bossPrefab, spawnParentTransform);

        Middle middleSpawner = spawnParent.gameObject.AddComponent<Middle>();
        middleSpawner.MonsterCount = middleCount;
        middleSpawner.SetTileInfos(tileInfos);
        middleSpawner.Spawning(middlePrefab, spawnParentTransform);

        Normal normalSpawner = spawnParent.gameObject.AddComponent<Normal>();
        normalSpawner.MonsterCount = normalCount;
        normalSpawner.SetTileInfos(tileInfos);
        normalSpawner.Spawning(normalPrefab, spawnParentTransform);
        
        Player playerSpawner = spawnParent.gameObject.AddComponent<Player>();
        playerSpawner.MonsterCount = playerCount;
        playerSpawner.SetTileInfos(tileInfos);
        playerSpawner.Spawning(playerPrefab, spawnParentTransform);
    }
}