using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ������ �� ������Ʈ ������ �迭
    public int monsterCount; // �� ���� Ƚ��
    public int allySpawnCount; // �Ʊ� ���� Ƚ��
    private int playerSpawnCount = 1; // �÷��̾� ���� Ƚ��

    void Start()
    {
        StartCoroutine(StartEnemySpawnDelayed()); // ���� �ð� �ڿ� �� ���� ����
    }

    IEnumerator StartEnemySpawnDelayed()
    {
        yield return new WaitForSeconds(0.1f); // 0.1�� �Ŀ� ����

        TileInfoCheck(); // Ÿ�� ���� ��������
    }

    void TileInfoCheck() // Ÿ�� ���� ��������
    {
        TileInfoGenerator tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();

        if (tileInfoGenerator != null)
        {
            // Ÿ�� ���� ����Ʈ�� �����ɴϴ�.
            List<TileInfoGenerator.TileInfo> tilePositions = tileInfoGenerator.GetTilePositions();

            if (tilePositions != null)
            {
                int count = tilePositions.Count;

                Debug.Log("Ÿ�� ������ ���������� �����Խ��ϴ� - UnitSpawner");
                Debug.Log("������ �ε��� ����: " + count);

                // �� ����
                EnemySpawn(tilePositions, monsterCount);
            }
            else
            {
                Debug.LogError("Ÿ�� ���� ����Ʈ�� ������ �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("TileInfoGenerator ��ũ��Ʈ�� ���� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void EnemySpawn(List<TileInfoGenerator.TileInfo> tilePositions, int monsterCount)
    {
        List<TileInfoGenerator.TileInfo> eligibleTiles = new List<TileInfoGenerator.TileInfo>(tilePositions);
        List<TileInfoGenerator.TileInfo> selectedTiles = new List<TileInfoGenerator.TileInfo>();

        // Ÿ�� ���� ����Ʈ���� ���� ���� �� �� ã��
        int highestRow = -1;
        foreach (TileInfoGenerator.TileInfo tileInfo in tilePositions)
        {
            if (tileInfo.row > highestRow)
            {
                highestRow = tileInfo.row;
            }
        }

        // ���� ���� �� ���� ������ ��� ���� ���� Ÿ���� �ε��� ã��
        List<int> eligibleIndexes = new List<int>();
        for (int i = 0; i < tilePositions.Count; i++)
        {
            if (tilePositions[i].row != highestRow)
            {
                eligibleIndexes.Add(i);
            }
        }

        // ���� ����
        for (int i = 0; i < monsterCount; i++)
        {
            TileInfoGenerator.TileInfo spawnTile = null;

            // eligibleIndexes ����Ʈ���� �ߺ����� �ʴ� ������ �ε��� ����
            int randomIndex = Random.Range(0, eligibleIndexes.Count);
            int selectedIndex = eligibleIndexes[randomIndex];

            // ���õ� �ε����� Ÿ�� ���� ��������
            spawnTile = tilePositions[selectedIndex];

            // �ش� ��ġ�� ������Ʈ ��������
            GameObject tileObject = GameObject.Find(spawnTile.name);

            if (tileObject != null)
            {
                // �ش� ������Ʈ�� TileCheck ��ũ��Ʈ�� ������
                TileCheck tileCheck = tileObject.GetComponent<TileCheck>();

                // �ش� ������Ʈ�� �ۺ� ���� ����(MeleeAtk, RangeAtk, Player, Enemy) ���� false���� Ȯ��
                if (!TileCheck.MeleeAtk && !TileCheck.RangeAtk && !TileCheck.Player && !TileCheck.Enemy)
                {
                    // ����� �α� ���
                    Debug.Log("���� " + (i + 1) + " ���� ��ġ: " + spawnTile.name);

                    // ������ ��ġ�� ���
                    selectedTiles.Add(spawnTile);

                    // �ش� ��ġ�� ���� ���� (����Ʈ�� x, y ��ǥ�� �����Ͽ� ����)
                    GameObject monsterPrefab = monsterPrefabs[0]; // ������ ����Ʈ�� ù ��° ������ ���
                    Vector3 spawnPosition = new Vector3(spawnTile.x, spawnTile.y, 0f); // Ÿ���� x, y ��ǥ�� ����Ͽ� ���� ��ġ ����
                    Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, tileObject.transform); // Ÿ���� �θ�� �����Ͽ� ����
                }
            }
            else
            {
                // �ش� ��ġ�� ������Ʈ�� ã�� �� ���� ��� ���� �޽��� ���
                Debug.LogError("�̸� '" + spawnTile.name + "'�� ���� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }
}