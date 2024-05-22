using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // ���� Ÿ�� ���
    public static class UnitType
    {
        public const int Normal = 1;
        public const int Middle = 2;
        public const int Boss = 3;
        public const int Player = 4;
    }

    // ���� ��ġ�� ������ �θ� ������Ʈ
    public Transform spawnParent;

    // Ÿ�� ������ �ҷ��� �θ� ������Ʈ ����
    public TileInfoGenerator tileInfoGenerator;

    // ���� �����յ�
    public GameObject bossPrefab;
    public GameObject middlePrefab;
    public GameObject normalPrefab;
    public GameObject playerPrefab;

    // ������ �ʿ��� ��ȯ ī��Ʈ
    public int bossCount;
    public int middleCount;
    public int normalCount;
    public int playerCount;

    // ���� ��ũ��Ʈ ȣ��
    void Start()
    {
        // 0.1�� �Ŀ� ���� ��ũ��Ʈ ȣ��
        Invoke("SpawnMonsters", 0.1f);
    }

    void SpawnMonsters()
    {
        // TileInfoGenerator ������Ʈ�� null���� Ȯ��
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // TileInfoGenerator Ŭ������ GetTileInfos �޼��带 ����Ͽ� Ÿ�� ���� ��������
        var tileInfos = tileInfoGenerator.GetTileInfos();

        // �θ� ������Ʈ�� Transform ��������
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