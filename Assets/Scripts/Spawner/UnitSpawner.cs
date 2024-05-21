using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using static TileInfoGenerator;

public class UnitSpawner : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static UnitSpawner Instance { get; private set; }

    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }

    protected int bossMonsterCount = 0;
    protected int middleMonsterCount = 0;

    // ���� Ÿ�� ���
    public int MonsterType { get; private set; }

    // Awake �޼��忡�� �̱��� �ν��Ͻ� ����
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
        }
    }

    public void SetTileInfos(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        this.tileInfos = tileInfos;
    }

    // �⺻ ���� �ñ״�ó �޼���
    public virtual void Spawning(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ �����ϴ�. ���͸� ������ �� �����ϴ�.");
            return;
        }

        Debug.LogWarning("�⺻ MonsterSpawner�� SpawnMonster()�� ȣ��Ǿ����ϴ�. �Ļ��� Ŭ�������� �� �޼��带 �������̵��ϼ���");
    }

    // ���� ���� ī���� ���� �޼���
    public void IncreaseBossMonsterCount()
    {
        bossMonsterCount++;
    }

    // �̵� ���� ī���� ���� �޼���
    public void IncreaseMiddleMonsterCount()
    {
        middleMonsterCount++;
    }

    public int GetBossMonsterCount()
    {
        return bossMonsterCount;
    }

    public int GetMiddleMonsterCount()
    {
        return middleMonsterCount;
    }
}

public class Boss : UnitSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

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

        // ������Ʈ�� BoxCollider2D�� ����
        BoxCollider2D boxCollider2D = bossMonster.GetComponent<BoxCollider2D>();


        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(tileInfos[2].posX, tileInfos[2].posY);
            rectTransform.sizeDelta = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (�⺻ Ÿ�� width * 3) + (Ÿ�ϰ��� * 2)
        }
        else
        {
            Debug.LogWarning("���� ������ RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (boxCollider2D != null)
        {
            boxCollider2D.size = new Vector2((tileInfos[0].tileSize.x * 3) + (tileInfos[0].spacing * 2), tileInfos[0].tileSize.y); // (�⺻ Ÿ�� width * 3) + (Ÿ�ϰ��� * 2)
        }
        else
        {
            Debug.LogWarning("���� ������ BoxCollider2D ������Ʈ�� ã�� �� �����ϴ�.");
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

        UnitSpawner.Instance.IncreaseBossMonsterCount();
        Debug.Log("���� ���͸� �����߽��ϴ�.");
    }
}

public class Middle : UnitSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

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

        // ������Ʈ�� BoxCollider2D�� ����
        BoxCollider2D boxCollider2D = middleMonster.GetComponent<BoxCollider2D>();

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

        if (boxCollider2D != null)
        {
            boxCollider2D.size = new Vector2((tileInfos[0].tileSize.x * 2) + tileInfos[0].spacing, tileInfos[0].tileSize.y); // (�⺻ Ÿ�� width * 2) + Ÿ�ϰ���
        }
        else
        {
            Debug.LogWarning("�̵� ���� ������ BoxCollider2D ������Ʈ�� ã�� �� �����ϴ�.");
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

        UnitSpawner.Instance.IncreaseMiddleMonsterCount();
        Debug.Log("�̵� ���� ���͸� �����߽��ϴ�.");
    }
}

public class Normal : UnitSpawner
{
    // �ߺ� ���� üũ�� ���� ����Ʈ
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private TileInfoGenerator tileInfoGenerator;

    protected override void Awake()
    {
        base.Awake();
        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>(); // TileInfoGenerator ��ü ����
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        if (tileInfoGenerator == null)
        {
            Debug.LogWarning("TileInfoGenerator�� ã�� �� �����ϴ�.");
            return;
        }

        List<TileInfo> tileList = tileInfoGenerator.GetTileInfos(); // tileList ��������

        // Ÿ�� ������ ��� ���� �����ɴϴ�.
        int maxRow = tileList.Max(t => t.row);
        int maxColumn = tileList.Max(t => t.column);

        // ���� ī���Ͱ� �ִ��� Ȯ��
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("�븻 ���� ī���Ͱ� 0 �����Դϴ�. ������ �븻 ���Ͱ� �����ϴ�.");
            return;
        }

        // ���� ��ġ�� �����ϴ� ����Ʈ �ʱ�ȭ
        List<int> spawnedIndices = new List<int>();

        // �̱��� �ν��Ͻ��� ���� ������ �̵� ���� ī��Ʈ ��������
        int bossCount = UnitSpawner.Instance.GetBossMonsterCount();
        int middleCount = UnitSpawner.Instance.GetMiddleMonsterCount();

        // ��������, �̵齺���� ���� 0�� ���
        if (bossCount == 0 && middleCount == 0)
        {
            Debug.Log("���� �̵麸�� ����");
            SpawnMonsterWithRule(maxRow, maxColumn, false, prefab, spawnParent, spawnedIndices, tileList);
        }
        else
        {
            Debug.Log("���� �̵麸�� ����");
            SpawnMonsterWithRule(maxRow, maxColumn, true, prefab, spawnParent, spawnedIndices, tileList);
        }
    }

    // ���� ��Ģ�� ���� ���� �����ϴ� �޼���
    private void SpawnMonsterWithRule(int maxRow, int maxColumn, bool bossOrMiddleSpawned, GameObject prefab, Transform spawnParent, List<int> spawnedIndices, List<TileInfo> tileList)
    {
        // Ÿ�� ���� ����Ʈ���� �����ϰ� Ÿ���� �����ϰ� �ߺ��� ���ϸ� ����
        for (int i = 0; i < MonsterCount; i++)
        {
            int randomIndex;
            int randomRow;
            int randomColumn;

            // Ÿ�� ���� ����Ʈ���� �����ϰ� Ÿ�� ����
            do
            {
                if (!bossOrMiddleSpawned)
                {
                    randomRow = Random.Range(0, maxRow + 1);
                    randomColumn = Random.Range(0, maxColumn);
                    Debug.Log("(������ ���� - �������� �������� ����)" + "��: " + randomColumn  + "��: " + randomRow);
                }
                else
                {
                    // ���� �̵麸���� ���� ��� ���� ������ �� ���� ���� Ÿ���� �����ϰ� ������ Ÿ�� ����
                    randomRow = Random.Range(0, maxRow + 1);
                    randomColumn = Random.Range(1, maxColumn); // ���� ������ �� ���� ���� Ÿ���� ����
                    Debug.Log("(������ ���� - �������� ����)" + "��: " + randomColumn + "��: " + randomRow);
                }

                // ���õ� ��� ���� �ش��ϴ� Ÿ���� �ε��� ã��
                randomIndex = FindIndexByRowAndColumn(randomRow, randomColumn, tileList);

            } while (spawnedIndices.Contains(randomIndex));

            // �ߺ� ���� üũ ����Ʈ�� �߰�
            spawnedIndices.Add(randomIndex);

            // �θ� ������Ʈ�� ������ �� ���͸� ����
            GameObject normalMonster = Instantiate(prefab, spawnParent);

            // ������ �̸� ����
            normalMonster.name = $"Normal_Monster_{i + 1}";

            // ���� ��Ͽ� �߰�
            spawnedMonsters.Add(normalMonster);

            // �θ� ������Ʈ�� ��ġ�� �����Ͽ� anchoredPosition�� ����
            RectTransform rectTransform = normalMonster.GetComponent<RectTransform>();

            // ������Ʈ�� BoxCollider2D�� ����
            BoxCollider2D boxCollider2D = normalMonster.GetComponent<BoxCollider2D>();

            if (rectTransform != null)
            {
                // �ε����� ����Ͽ� Ÿ�� ������ ��ġ ������ ������
                TileInfo tileInfo = tileList[randomIndex];
                rectTransform.anchoredPosition = new Vector2(tileInfo.posX, tileInfo.posY);
                // ������ ����
                rectTransform.sizeDelta = new Vector2(tileInfo.tileSize.x, tileInfo.tileSize.y);
            }
            else
            {
                Debug.LogWarning($"�븻 ���� {i + 1}�� RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
            }

            if (boxCollider2D != null)
            {
                boxCollider2D.size = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
            }
            else
            {
                Debug.LogWarning($"�븻 ���� {i + 1}�� BoxCollider2D ������Ʈ�� ã�� �� �����ϴ�.");
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

    // ����Ʈ���� Ư�� ��� �� ���� ��ġ�ϴ� ����� �ε����� ã�� �Լ�
    public int FindIndexByRowAndColumn(int row, int column, List<TileInfo> tileList)
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            // ��� ���� �ݴ�� ���մϴ�.
            if (tileList[i].row == row && tileList[i].column == column)
            {
                return i; // ��ġ�ϴ� ����� �ε��� ��ȯ
            }
        }

        return -1; // ��ġ�ϴ� ��Ұ� ���� ��� -1 ��ȯ
    }
}

public class Player : UnitSpawner
{
    private TileInfoGenerator tileInfoGenerator;

    protected override void Awake()
    {
        base.Awake();
        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>(); // TileInfoGenerator ��ü ����
    }

    public override void Spawning(GameObject prefab, Transform spawnParent)
    {
        base.Spawning(prefab, spawnParent);

        List<TileInfo> tileList = tileInfoGenerator.GetTileInfos(); // tileList ��������

        // Ÿ�� ������ ���� ��� �⺻ �޼��忡�� �����
        if (tileInfos == null || tileInfos.Count == 0) return;

        int randomIndex;
        float maxRow = tileList.Max(t => t.row);
        int maxColumn = tileList.Max(t => t.column);
        int middleRow = (int)((maxRow - 1) / 2f + 1); // �߰� ���� ã��

        if (maxRow == 3)
        {
            int randomValue = Random.Range(0, 2);

            middleRow = middleRow - randomValue;
        }

        randomIndex = FindIndexByRowAndColumn(middleRow, maxColumn, tileList);

        // �θ� ������Ʈ�� ������ �� �÷��̾��� ����
        GameObject PlayerUnit = Instantiate(prefab, spawnParent);

        // �÷��̾��� �̸� ����
        PlayerUnit.name = "Player_Unit";

        // �θ� ������Ʈ�� ��ġ�� �����Ͽ� anchoredPosition�� ����
        RectTransform rectTransform = PlayerUnit.GetComponent<RectTransform>();

        // ������Ʈ�� BoxCollider2D�� ����
        BoxCollider2D boxCollider2D = PlayerUnit.GetComponent<BoxCollider2D>();

        if (rectTransform != null)
        {
            // �ε����� ����Ͽ� Ÿ�� ������ ��ġ ������ ������
            TileInfo tileInfo = tileList[randomIndex];
            rectTransform.anchoredPosition = new Vector2(tileInfo.posX, tileInfo.posY);
            // ������ ����
            rectTransform.sizeDelta = new Vector2(tileInfo.tileSize.x, tileInfo.tileSize.y);
        }
        else
        {
            Debug.LogWarning("�÷��̾� ������ RectTransform ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (boxCollider2D != null)
        {
            boxCollider2D.size = new Vector2(tileInfos[0].tileSize.x, tileInfos[0].tileSize.y);
        }
        else
        {
            Debug.LogWarning("�÷��̾� ������ BoxCollider2D ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // �÷��̾� ������ �̹��� ������Ʈ�� ������ ������ ����
        Image image = PlayerUnit.GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Debug.LogWarning("�÷��̾� ������ Image ������Ʈ�� ã�� �� �����ϴ�.");
        }

        Debug.Log("�÷��̾� ������ �����߽��ϴ�");
    }

    // ����Ʈ���� Ư�� ��� �� ���� ��ġ�ϴ� ����� �ε����� ã�� �Լ�
    public int FindIndexByRowAndColumn(int row, int column, List<TileInfo> tileList)
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            // ��� ���� �ݴ�� ���մϴ�.
            if (tileList[i].row == row && tileList[i].column == column)
            {
                return i; // ��ġ�ϴ� ����� �ε��� ��ȯ
            }
        }

        return -1; // ��ġ�ϴ� ��Ұ� ���� ��� -1 ��ȯ
    }
}