using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UnitSpawner : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator; // Ÿ�� ������ �����ϴ� TileInfoGenerator ��ũ��Ʈ�� ����

    // Ÿ�� ���� ����Ʈ
    private List<TileInfoGenerator.TileInfo> tileInfos; // Ÿ�� ���� ����Ʈ
    List<Vector2> selectedIndex = new List<Vector2>(); // ������ Ÿ�� ���� ����Ʈ �ε����� ���� Ÿ�� ���� ����Ʈ

    // ���� ī��Ʈ
    public int Monster11Count;
    public int Monster21Count;
    public int Monster31Count;

    // �Լ��� ����
    private int highestRow;
    private int highestColumn;

    // ���� ������
    public GameObject monster31Prefab; // ���� ���� 31�� ������
    public GameObject monster21Prefab; // ���� ���� 21�� ������
    public GameObject monster11Prefab; // ���� ���� 11�� ������

    void Start()
    {
        Debug.Log(highestRow);
        Debug.Log(highestColumn);

        Invoke("GetTileInfosDelayed", 0.1f);

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

    // ���� ���� ��/�� ����
    private int GetHighestRow(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        int highestRow = int.MinValue;
        foreach (var tileInfo in tileInfos)
        {
            if (tileInfo.row > highestRow)
            {
                highestRow = tileInfo.row;
            }
        }
        return highestRow;
    }
    private int GetHighestColumn(List<TileInfoGenerator.TileInfo> tileInfos) // ���� ���� ��
    {
        int highestColumn = int.MinValue;
        foreach (var tileInfo in tileInfos)
        {
            if (tileInfo.column > highestColumn)
            {
                highestColumn = tileInfo.column;
            }
        }
        return highestColumn;
    }

    // ���� �Լ�
    private void Type31Tile() // 31 Ÿ�� ���� ���� ��ġ ����
    {
        // 31Ÿ�� ������ ���� Ȯ��
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow + 1; // �� ���� ���

        if (rowCount != 5)
        {
            Debug.LogWarning("���� 5�� �ƴϾ 31Ÿ�� ���͸� ������ �� �����ϴ�.");
            return;
        }

        // 31 Ÿ�� ������ ���� ��ġ�� ����
        for (int i = 0; i < Monster31Count; i++)
        {
            if (tileInfos.Exists(tile => tile.row == 0 && (tile.column == 2 || tile.column == 3 || tile.column == 4)))
            {
                List<int> selectedColumns = new List<int> { 2, 3, 4 };
                string logMessage = "31Ÿ�� ���� ���� ��ġ: ";
                foreach (int column in selectedColumns)
                {
                    int tileIndex = tileInfos.FindIndex(tile => tile.row == 0 && tile.column == column);
                    selectedIndex.Add(new Vector2Int(tileIndex, 31));
                    logMessage += $"(0, {column}), ";
                }
                // ������ ��ǥ ����
                logMessage = logMessage.TrimEnd(',', ' ');
                Debug.Log(logMessage);
            }

            // �ߺ� ���� üũ
            if (Monster31Count > 1)
            {
                Debug.LogWarning("���� ����(31Ÿ��)�� ������ 2 �̻����� �����Ǿ� �ֽ��ϴ�.");
                return;
            }
        }
    }

    private void Type21Tile() // 21 Ÿ�� ���� ���� ��ġ ����
    {
        // 21Ÿ�� ������ ���� Ȯ��
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow + 1; // �� ���� ���

        if (rowCount != 4)
        {
            Debug.LogWarning("���� 4�� �ƴϾ 21Ÿ�� ���͸� ������ �� �����ϴ�.");
            return;
        }

        // 21 Ÿ�� ������ ���� ��ġ�� ����
        for (int i = 0; i < Monster21Count; i++)
        {
            if (tileInfos.Exists(tile => tile.row == 0 && tile.column == 2) && tileInfos.Exists(tile => tile.row == 0 && tile.column == 3))
            {
                List<(int, int)> selectedPositions = new List<(int, int)> { (0, 2), (0, 3) };
                string logMessage = "21Ÿ�� ���� ���� ��ġ: ";
                foreach (var position in selectedPositions)
                {
                    int tileIndex = tileInfos.FindIndex(tile => tile.row == position.Item1 && tile.column == position.Item2);
                    selectedIndex.Add(new Vector2Int(tileIndex, 21));
                    logMessage += $"({position.Item1}, {position.Item2}), ";
                }
                // ������ ��ǥ ����
                logMessage = logMessage.TrimEnd(',', ' ');
                Debug.Log(logMessage);

            }

            // �ߺ� ���� üũ
            if (Monster21Count > 1)
            {
                Debug.LogWarning("���� ����(21Ÿ��)�� ������ 2 �̻����� �����Ǿ� �ֽ��ϴ�.");
                return;
            }
        }
    }

    private void Type11Tile() // 11 Ÿ�� ���� ���� ��ġ ����
    {
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow;
        int columnCount = tileInfos.Max(tile => tile.column) + 1;

        System.Random random = new System.Random();

        for (int i = 0; i < Monster11Count; i++)
        {
            int randomRow, randomColumn;
            int tileIndex;

            if (Monster21Count > 0 && Monster31Count > 0)
            {
                do
                {
                    randomRow = random.Next(1, rowCount); // 1���� rowCount-1���� ����
                    randomColumn = random.Next(0, columnCount); // 0���� columnCount-1���� ����

                    tileIndex = tileInfos.FindIndex(tile => tile.row == randomRow && tile.column == randomColumn);

                } while (tileIndex == -1 || selectedIndex.Any(index => index.x == tileIndex)); // ��ȿ�� Ÿ������, �̹� ���õ� Ÿ������ Ȯ��
            }
            else
            {
                do
                {
                    randomRow = random.Next(1, rowCount); // 0���� rowCount-1���� ����
                    randomColumn = random.Next(0, columnCount); // 0���� columnCount-1���� ����

                    tileIndex = tileInfos.FindIndex(tile => tile.row == randomRow && tile.column == randomColumn);

                } while (tileIndex == -1 || selectedIndex.Any(index => index.x == tileIndex)); // ��ȿ�� Ÿ������, �̹� ���õ� Ÿ������ Ȯ��
            }

            selectedIndex.Add(new Vector2Int(tileIndex, 11)); // ���õ� �ε����� ���� Ÿ�� �߰�
            Debug.Log($"11Ÿ�� ���� ���� ��ġ: ({randomRow}, {randomColumn})");
        }
    }

    private void MonsterSpawning()
    {
        // 31 Ÿ�� ���� ����
        if (Monster31Count > 0)
        {
            SpawnMonster(monster31Prefab, 3, 0, "31_Monster_", "MonsterName");
            selectedIndex.RemoveAll(v => v.y == 31);
        }

        // 21 Ÿ�� ���� ����
        if (Monster21Count > 0)
        {
            SpawnMonster(monster21Prefab, 2, 1, "21_Monster_", "MonsterName");
            selectedIndex.RemoveAll(v => v.y == 21);
        }

        // 11 Ÿ�� ���� ����
        if (Monster11Count > 0)
        {
            for (int i = 0; i < Monster11Count; i++)
            {
                if (selectedIndex.Count > 0)
                {
                    Vector2 element = selectedIndex[0];
                    int x1 = (int)element.x;
                    SpawnMonster(monster11Prefab, 1, x1, "11_Monster_", "MonsterName" + i);
                    selectedIndex.RemoveAt(0);
                }
            }
        }
    }

    private void SpawnMonster(GameObject prefab, int tileCount, int tileIndex, string monsterType, string monsterName)
    {
        // ���� ������Ʈ ����
        GameObject newObject = Instantiate(prefab, transform);
        newObject.name = monsterType + monsterName;

        // ���� �ν����� ����
        float width = tileInfos[0].width * tileCount + tileInfos[0].spacing * (tileCount - 1);
        float height = tileInfos[0].height;
        float posX = 0f;
        float posY = 0f;

        for (int i = tileIndex; i < tileIndex + tileCount; i++)
        {
            posX += tileInfos[i].posX;
            posY += tileInfos[i].posY;
        }
        posX /= tileCount;
        posY /= tileCount;

        // ���� ũ�� ����
        RectTransform rectTransform = newObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(posX, posY); // ��ġ ����
        }
        else
        {
            Debug.LogError("�����տ� RectTransform�� �����ϴ�.");
            return;
        }

        // �̹��� ������Ʈ�� �ִ� ���� ����
        var image = newObject.GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f; // �ִ� ���� ����
            image.color = color;
        }
        else
        {
            Debug.LogError("�̹��� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }
    }


    // ���� üũ
    private bool TileChecking(List<Vector2Int> selectedIndex)
    {
        bool allTilesPassed = true;

        foreach (var indexInfo in selectedIndex)
        {
            int index = indexInfo.x;
            int monsterType = indexInfo.y;

            string tileName = tileInfos[index].tileName;

            GameObject[] foundObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == tileName).ToArray();

            // �ش� �̸��� ���� ������Ʈ�� �ִ��� Ȯ��
            if (foundObjects.Length == 0)
            {
                Debug.LogError($"�±� '{tileName}'�� ���� ������Ʈ�� ã�� �� �����ϴ�.");
                allTilesPassed = false;
                continue;
            }

            // �ش� �̸��� ���� ������Ʈ�� Ÿ�� Ÿ���� �ִ��� üũ
            bool tileTypeExists = false;
            foreach (var obj in foundObjects)
            {
                TileCheck tileCheck = obj.GetComponent<TileCheck>();

                if (tileCheck != null)
                {
                    tileTypeExists = true;
                    break;
                }
            }

            // Ÿ�� Ÿ���� ���� ���
            if (!tileTypeExists)
            {
                Debug.LogError($"������Ʈ�� Ÿ�� Ÿ���� �����ϴ�: '{tileName}'");
                allTilesPassed = false;
                continue;
            }

            // Ÿ�� Ÿ�Կ� �ִ� bool ������ ���� false���� Ȯ��
            bool allTileTypeBoolsFalse = true;
            foreach (var obj in foundObjects)
            {
                TileCheck tileCheck = obj.GetComponent<TileCheck>();

                if (tileCheck != null && (TileCheck.MeleeAtk || TileCheck.RangeAtk || TileCheck.Player || TileCheck.Enemy))
                {
                    allTileTypeBoolsFalse = false;
                    break;
                }
            }

            // ���� ������ ������ ���
            if (!allTileTypeBoolsFalse)
            {
                Debug.LogWarning($"Ÿ�� '{tileName}'�� ������ �������� �ʽ��ϴ�.");
                allTilesPassed = false;
            }
        }

        return allTilesPassed;
    }


    // ���� ���� �Լ�
    public void MonsterSpawn()
    {
        // ���� �ʱ�ȭ
        int type11Count = Monster11Count;
        int type21Count = Monster21Count;
        int type31Count = Monster31Count;

        if (tileInfos == null || tileInfos.Count == 0 || selectedIndex == null) // Ÿ�� ���� ����Ʈ �Ǵ� selectedIndex�� ���� ��� ����
        {
            Debug.LogError("Ÿ�� ������ selectedIndex�� �����ϴ�.");
            return;
        }
        else
        {
            selectedIndex.Clear(); // ����Ʈ �ʱ�ȭ
            int highestRow = GetHighestRow(tileInfos); // ���� ���� �� ���ϱ�
            int highestColumn = GetHighestColumn(tileInfos); // ���� ���� �� ���ϱ�

            Debug.Log("���� Ÿ�� �׸���" + (highestRow + 1) + (highestColumn + 1));

            //Ÿ�Ժ� ���� ��ġ ����
            Type31Tile();
            Type21Tile();
            Type11Tile();

            // ������ ��ġ Ÿ�� üũ
            bool allTilesPassed = TileChecking(selectedIndex.ConvertAll(v => new Vector2Int((int)v.x, (int)v.y)));

            Debug.Log("selectedIndex ����Ʈ:");
            foreach (var index in selectedIndex)
            {
                Debug.Log($"�ε���: ({index.x}, {index.y})");
            }

            // ��� Ÿ���� ����Ǿ����� Ȯ���ϰ� ó��
            if (allTilesPassed)
            {
                Debug.Log("��� Ÿ�� üũ �Ϸ�!");
                MonsterSpawning();
            }
            else
            {
                // ���⿡ ��� Ÿ���� ������� ���� ����� ������ �߰��մϴ�.
            }
        }
    }
}
