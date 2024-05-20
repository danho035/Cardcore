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


    // ���� Ÿ�� ���
    public int MonsterType { get; private set; }

    public void SetTileInfos(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        this.tileInfos = tileInfos;
    }

    // �⺻ ���� �ñ״�ó �޼���
    public virtual void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ �����ϴ�. ���͸� ������ �� �����ϴ�.");
            return;
        }

        Debug.LogWarning("�⺻ MonsterSpawner�� SpawnMonster()�� ȣ��Ǿ����ϴ�. �Ļ��� Ŭ�������� �� �޼��带 �������̵��ϼ���");
    }
}

public class Boss : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // Ÿ�� ������ ���� ��� �⺻ �޼��忡�� �����
        if (tileInfos == null || tileInfos.Count == 0) return;

        if (tileInfos[tileInfos.Count - 1].row != 4)
        {
            Debug.Log("�ִ� ���� 5�� �ƴϹǷ� ���� ���͸� ������ �� �����ϴ�.");
            return;
        }

        if (MonsterCount <= 0)
        {
            Debug.Log("���� ���� ī���Ͱ� 0 �����Դϴ�. ������ ���� ���Ͱ� �����ϴ�.");
            return;
        }

        // �θ� ������Ʈ�� ������ �� ���͸� ����
        GameObject bossMonster = Instantiate(prefab, spawnParent);

        // �θ� ������Ʈ�� ��ġ�� �����Ͽ� anchoredPosition�� ����
        RectTransform rectTransform = bossMonster.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(tileInfos[2].posX, tileInfos[2].posY);
            rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (�⺻ Ÿ�� width * 3) + (Ÿ�ϰ��� * 2)
        }
        else
        {
            Debug.LogWarning("���� ������ RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ������ �̸� ����
        bossMonster.name = $"Boss_Monster";

        // ������ �̹��� ������Ʈ�� ������ ������ ����
        Image image = bossMonster.GetComponent<Image>();

        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Debug.LogWarning("���� ������ Image ������Ʈ�� ã�� �� �����ϴ�.");
        }

        IncreaseBossMonsterCount();
        Debug.Log("���� ���͸� �����߽��ϴ�.");
    }

    // ���� ���� ī���� ���� �޼���
    protected void IncreaseBossMonsterCount()
    {
        bossMonsterCount++;
        Debug.Log("���� ī��Ʈ ��");
        Debug.Log("���� ī��Ʈ: " + bossMonsterCount);
    }
}

public class Middle : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // Ÿ�� ������ ���� ��� �⺻ �޼��忡�� �����
        if (tileInfos == null || tileInfos.Count == 0) return;

        if (tileInfos[tileInfos.Count - 1].row != 3)
        {
            Debug.Log("�ִ� ���� 4�� �ƴϹǷ� �̵� ���� ���͸� ������ �� �����ϴ�.");
            return;
        }

        if (MonsterCount <= 0)
        {
            Debug.Log("�̵� ���� ���� ī���Ͱ� 0 �����Դϴ�. ������ ���� ���Ͱ� �����ϴ�.");
            return;
        }

        // �θ� ������Ʈ�� ������ �� ���͸� ����
        GameObject middleMonster = Instantiate(prefab, spawnParent);

        // �θ� ������Ʈ�� ��ġ�� �����Ͽ� anchoredPosition�� ����
        RectTransform rectTransform = middleMonster.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            float middlePosX = (tileInfos[1].posX + tileInfos[2].posX) / 2f;

            rectTransform.anchoredPosition = new Vector2(middlePosX, tileInfos[2].posY);
            rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 2) + tileInfos[0].spacing, tileInfos[0].tileSize.y); // (�⺻ Ÿ�� width * 2) + Ÿ�ϰ���
        }
        else
        {
            Debug.LogWarning("�̵� ���� ������ RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ������ �̸� ����
        middleMonster.name = $"Middle_Monster";

        // ������ �̹��� ������Ʈ�� ������ ������ ����
        Image image = middleMonster.GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Debug.LogWarning("�̵� ���� ������ Image ������Ʈ�� ã�� �� �����ϴ�.");
        }

        IncreaseMiddleMonsterCount();
        Debug.Log("�̵� ���� ���͸� �����߽��ϴ�.");
    }

    // �̵� ���� ���� ī���� ���� �޼���
    protected void IncreaseMiddleMonsterCount()
    {
        middleMonsterCount++;
        Debug.Log("�̵麸�� ī��Ʈ ��");
        Debug.Log("�̵麸�� ī��Ʈ: " + middleMonsterCount);
    }

}

public class Normal : MonsterSpawner
{
    // �ߺ� ���� üũ�� ���� ����Ʈ
    private List<int> spawnedIndices = new List<int>();
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        base.SpawnMonster(prefab, spawnParent);

        // Ÿ�� ������ ��� ���� �����ɴϴ�.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // ���� ī���Ͱ� �ִ��� Ȯ��
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("�븻 ���� ī���Ͱ� 0 �����Դϴ�. ������ �븻 ���Ͱ� �����ϴ�.");
            return;
        }

        // ���� ��ġ�� �����ϴ� ����Ʈ �ʱ�ȭ
        spawnedIndices.Clear();

        // ��������, �̵齺���� ���� 0�� ���
        if (bossMonsterCount == 0 && middleMonsterCount == 0)
        {
            Debug.Log("���� �̵麸�� ����");
            SpawnMonsterWithRule(maxRow, maxColumn, false, prefab, spawnParent);
        }
        else if (bossMonsterCount > 0 || middleMonsterCount > 0) // ��������, �̵齺���� �� �� �ϳ��� 1�� ���
        {
            Debug.Log("���� �̵麸�� ����");
            SpawnMonsterWithRule(maxRow, maxColumn, true, prefab, spawnParent);
        }
    }

    // ���� ��Ģ�� ���� ���� �����ϴ� �޼���
    private void SpawnMonsterWithRule(int maxRow, int maxColumn, bool bossOrMiddleSpawned, GameObject prefab, Transform spawnParent)
    {
        // �ߺ� ���� üũ�� ���� ����Ʈ �ʱ�ȭ
        spawnedIndices.Clear();
        spawnedMonsters.Clear();

        // ���� ����
        for (int i = 0; i < MonsterCount; i++)
        {
            int randomRow;
            int randomColumn;

            // ������ ��� �� ����
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

            // Ÿ�� ���� �ε��� ��ȣ ���
            int index = randomRow * maxColumn + randomColumn;

            // �ߺ� ���� üũ
            while (spawnedIndices.Contains(index))
            {
                // �ٽ� ������ ��ġ ����
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

            // �ߺ� ���� üũ ����Ʈ�� �߰�
            spawnedIndices.Add(index);

            // �θ� ������Ʈ�� ������ �� ���͸� ����
            GameObject normalMonster = Instantiate(prefab, spawnParent);

            // ������ �̸� ����
            normalMonster.name = $"Normal_Monster_{i + 1}";

            // ���� ��Ͽ� �߰�
            spawnedMonsters.Add(normalMonster);

            // �θ� ������Ʈ�� ��ġ�� �����Ͽ� anchoredPosition�� ����
            RectTransform rectTransform = normalMonster.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(tileInfos[index].posX, tileInfos[index].posY);
                // ������ ����
                rectTransform.sizeDelta = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
            }
            else
            {
                Debug.LogWarning($"�븻 ���� {i + 1}�� RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
            }

            // ������ �̹��� ������Ʈ�� ������ ������ ����
            Image image = normalMonster.GetComponent<Image>();
            if (image != null)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
            else
            {
                Debug.LogWarning($"�븻 ���� {i + 1}�� Image ������Ʈ�� ã�� �� �����ϴ�.");
            }

            Debug.Log($"�븻 ���͸� �����߽��ϴ�: {i + 1}");
        }

        // �븻 ���͸� ���������Ƿ� ī���� �ʱ�ȭ
        bossMonsterCount = 0;
        middleMonsterCount = 0;
        Debug.Log("�븻 ���͸� ��� �����߽��ϴ�. ī���� �ʱ�ȭ");
    }
}
