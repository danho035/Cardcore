using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public int enemySpawnCount; // �� ���� Ƚ��
    public int allySpawnCount; // �Ʊ� ���� Ƚ��
    private int playerSpanwCount = 1; // �÷��̾� ���� Ƚ��

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

    void EnemySpawn(List<TileInfoGenerator.TileInfo> tilePositions)
    {
        if (tilePositions != null && tilePositions.Count > 0)
        {
            // ���� ���� �� ���ڿ� ���� ���� �� ���ڸ� �ʱ�ȭ�մϴ�.
            int minRow = int.MaxValue;
            int maxRow = int.MinValue;

            // ���� ���� �� ���ڿ� ���� ���� �� ���ڸ� ã���ϴ�.
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

            // ���� ���� �� ���ڴ� ���� ��ġ���� �����մϴ�.
            maxRow--;

            // ������ ��ġ�� �����մϴ�.
            List<TileInfoGenerator.TileInfo> spawnPositions = new List<TileInfoGenerator.TileInfo>();
            foreach (TileInfoGenerator.TileInfo info in tilePositions)
            {
                if (info.row != maxRow)
                {
                    spawnPositions.Add(info);
                }
            }

            // ������ ��ġ���� �����ϰ� �����մϴ�.
            if (spawnPositions.Count > 0)
            {
                TileInfoGenerator.TileInfo spawnTile = spawnPositions[Random.Range(0, spawnPositions.Count)];

                Debug.Log("���� �����Ǿ����ϴ�. ��ġ: " + spawnTile.name);
                // ���⿡ �� ���� ������ �߰��ϼ���.
            }
            else
            {
                Debug.LogError("���� ������ �� �ִ� ��ġ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Ÿ�� ���� ����Ʈ�� ������ �� ���ų� ��� �ֽ��ϴ�.");
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