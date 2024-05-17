using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator; // Ÿ�� ������ �����ϴ� TileInfoGenerator ��ũ��Ʈ�� ����

    // Ÿ�� ���� ����Ʈ
    private List<TileInfoGenerator.TileInfo> tileInfos;

    // ���� ī��Ʈ
    public int Monster11Count;
    public int Monster21Count;
    public int Monster31Count;

    // ���� ������
    public GameObject monster31Prefab; // ���� ���� 31�� ������
    public GameObject monster21Prefab; // ���� ���� 21�� ������
    public GameObject monster11Prefab; // ���� ���� 11�� ������

    void Start()
    {
        Invoke("GetTileInfosDelayed", 0.01f);
    }

    void GetTileInfosDelayed()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // Ÿ�� ���� ����Ʈ �޾ƿ���
        tileInfos = tileInfoGenerator.GetTileInfos();

        // ���� ����
        MonsterSpawn();
    }
    public void MonsterSpawn()
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogError("Ÿ�� ������ �����ϴ�.");
            return;
        }

        // ���� Ÿ�� ����: 31 > 21 > 11
        int[] monsterCounts = { Monster31Count, Monster21Count, Monster11Count };
        for (int monsterType = 0; monsterType < monsterCounts.Length; monsterType++)
        {
            int count = monsterCounts[monsterType];
            if (count <= 0) continue;

            List<int> availableIndices = new List<int>();
            if (monsterType == 2) // ���� Ÿ���� 11�� ��
            {
                // ��ü Ÿ�� �߿��� �����ϰ� ����
                for (int i = 0; i < tileInfos.Count; i++)
                {
                    availableIndices.Add(i);
                }
            }
            else // ���� Ÿ���� 21�̳� 31�� ��
            {
                // ���� ���� ���� ����Ʈ �ε����� ���
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

            // ���� ���� ���� ����Ʈ �ε��� ����
            int highestRow = int.MinValue;
            foreach (var tileInfo in tileInfos)
            {
                if (tileInfo.row > highestRow)
                {
                    highestRow = tileInfo.row;
                }
            }

            availableIndices.RemoveAll(index => tileInfos[index].row == highestRow);

            // ���� ����
            List<int> spawnIndices = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, availableIndices.Count);
                spawnIndices.Add(availableIndices[index]);
                availableIndices.RemoveAt(index);
            }

            // �ε����� ������� ���� ����
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

                // ������ Ŭ�� ���� �� �̸� ���̱�
                GameObject monster = Instantiate(monsterPrefab, transform);
                monster.name = monsterPrefab.name + "_" + monsterType.ToString() + "_" + spawnIndex.ToString();

                RectTransform rectTransform = monster.GetComponent<RectTransform>();

                // Ÿ�� ������ ���� width, height ����
                float width;
                float height;
                if (monsterType == 1)
                {
                    // 21Ÿ���� ���
                    width = tileInfo.width * 2 + tileInfo.spacing;
                    height = tileInfo.height;
                }
                else if (monsterType == 0)
                {
                    // 31Ÿ���� ���
                    width = tileInfo.width * 3 + tileInfo.spacing * 2;
                    height = tileInfo.height;
                }
                else
                {
                    // 11Ÿ���� ���
                    width = tileInfo.width;
                    height = tileInfo.height;
                }

                // ������ ��ġ ����
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

                // TileCheck ��ũ��Ʈ üũ
                var tileCheck = monster.GetComponent<TileCheck>();
                if (tileCheck == null)
                {
                    Debug.LogError("Monster�� TileCheck ��ũ��Ʈ�� �����ϴ�.");
                    return;
                }

                // ������ ������ �̹��� ������Ʈ�� �����ͼ� alpha ���� ����
                var image = monster.GetComponent<Image>();
                Color color = image.color;
                color.a = 1f; // �ִ� ������ ����
                image.color = color;
            }
        }
    }
}
