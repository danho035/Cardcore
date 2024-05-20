using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MonsterSpawner : MonoBehaviour
{
    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }


    // ���� Ÿ�� ���
    public int MonsterType { get; private set; }

    public void SetTileInfos(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        this.tileInfos = tileInfos;
    }

    // SpawnMonster �޼��� �ñ״�ó ����
    public virtual void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        Debug.LogWarning("�⺻ MonsterSpawner�� SpawnMonster()�� ȣ��Ǿ����ϴ�. �Ļ��� Ŭ�������� �� �޼��带 �������̵��ϼ���");
    }
}

public class BossMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ �����ϴ�. ���� ���͸� ������ �� �����ϴ�.");
            return;
        }

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

        float posX = tileInfos[2].posX;
        float posY = tileInfos[2].posY;
        Vector3 spawnPosition = new Vector3(posX, posY, 0f);

        for (int i = 0; i < MonsterCount; i++)
        {
            GameObject bossMonster = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);
            bossMonster.name = $"Boss_Monster_{i + 1}";

            SpriteRenderer spriteRenderer = bossMonster.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }

            Debug.Log("���� ���͸� �����߽��ϴ�.");

            if (MonsterCount > 1)
            {
                Debug.Log("���� ���ʹ� 1ȸ�� ���� �����մϴ�.");
                return;
            }
        }
    }
}

public class MiddleMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        // Ÿ�� ������ ������ ����
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ �����ϴ�. �̵� ���͸� ������ �� �����ϴ�.");
            return;
        }

        // Ÿ�� ������ ��� ���� �����ɴϴ�.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // ���� ī���Ͱ� �ִ��� Ȯ��
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("�̵� ���� ī���Ͱ� 0 �����Դϴ�. ������ �̵� ���Ͱ� �����ϴ�.");
            return;
        }

        // ���� ����
        for (int i = 0; i < MonsterCount; i++)
        {
            // �̵� ���͸� �����մϴ�.
            Debug.Log("�̵� ���͸� �����߽��ϴ�.");
        }
    }
}

public class NormalMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        // Ÿ�� ������ ������ ����
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ �����ϴ�. �븻 ���͸� ������ �� �����ϴ�.");
            return;
        }

        // Ÿ�� ������ ��� ���� �����ɴϴ�.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // ���� ī���Ͱ� �ִ��� Ȯ��
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("�븻 ���� ī���Ͱ� 0 �����Դϴ�. ������ �븻 ���Ͱ� �����ϴ�.");
            return;
        }

        // ���� ����
        for (int i = 0; i < MonsterCount; i++)
        {
            // �븻 ���͸� �����մϴ�.
            Debug.Log("�븻 ���͸� �����߽��ϴ�.");
        }
    }
}