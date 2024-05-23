using System.Collections.Generic;
using UnityEngine;

public class AttackTileSelect : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator;

    List<Vector2Int> attackableTiles = new List<Vector2Int>(); // attackableTiles ����Ʈ ����

    List<int> melee_1 = new List<int> { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 2, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
    List<int> melee_2 = new List<int> { 0, 1, 0, 1, 2, 1, 0, 1, 0 };
    List<int> range_1 = new List<int> { 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 0, 2, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0 };
    List<int> range_2 = new List<int> { 1, 0, 1, 1, 2, 1, 1, 0, 1 };

    List<List<int>> atkRange = new List<List<int>>();

    public int unitRow;
    public int unitColumn;

    public int meleeTargetTileCount;
    public int rangeTargetTileCount;

    public float duration;

    public int meleeChance;
    public int rangeChance;

    public int atkCount;

    void Start()
    {
        ConvertToAtkRange(range_2, atkRange);

        // Debug.Log�� �̿��Ͽ� atkRange ����Ʈ ���
        foreach (var sublist in atkRange)
        {
            string sublistString = string.Join(", ", sublist);
            Debug.Log("Sublist: " + sublistString);
        }

        // ���÷� (0, 0) ��ġ�� ���� ���� ��ġ�� �ִٰ� ����
        Vector2Int attackStartPosition = new Vector2Int(unitColumn, unitRow);
        // ���� ���� ��ġ�� �������� ���� ������ Ÿ���� ��ǥ ���
        CalculateAttackableTiles(attackStartPosition);
    }

    void ConvertToAtkRange(List<int> originalList, List<List<int>> atkRange)
    {
        int count = originalList.Count;

        List<int> sublist = new List<int>();
        int sublistSize = 0;

        if (count == 25)
        {
            sublistSize = 5;
        }
        else if (count == 16)
        {
            sublistSize = 4;
        }
        else if (count == 9)
        {
            sublistSize = 3;
        }
        else
        {
            Debug.LogError("������ ���� ��ų�� � Ÿ�� Ÿ�Կ��� �������� �ʽ��ϴ�. ��ųʸ��� Ȯ�����ּ���");
            return; // ��ȿ���� ���� ��� ���� ���� ����
        }

        for (int i = 0; i < count; i++)
        {
            sublist.Add(originalList[i]);

            if (sublist.Count == sublistSize)
            {
                atkRange.Add(new List<int>(sublist)); // sublist�� �ʱ�ȭ�� �� �߰�
                sublist.Clear(); // sublist �ʱ�ȭ
            }
        }
    }

    void CalculateAttackableTiles(Vector2Int startPosition)
    {
        int rows = atkRange.Count;
        int columns = atkRange[0].Count;
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (atkRange[i][j] == 1) // 1�� ���� ������ Ÿ���� ��Ÿ��
                {
                    // ���� ���� ��ġ������ ������� ��ǥ ���
                    int relativeRow = i - centerRow;
                    int relativeColumn = j - centerColumn;
                    // ���� ��ǥ ���
                    int targetRow = startPosition.x + relativeRow;
                    int targetColumn = startPosition.y + relativeColumn;
                    Vector2Int targetPosition = new Vector2Int(targetRow, targetColumn);
                    // ��� ���
                    Debug.Log("���� ������ Ÿ�� ��ġ: " + targetPosition);
                    // ���� ������ Ÿ���� ����Ʈ�� �߰�
                    attackableTiles.Add(targetPosition);
                }
            }
        }
    }

    void TileTargeting()
    {
        rangeChance = rangeChance + meleeChance;

        int atkTypeSelect = Random.Range(1, (meleeChance + rangeChance + 1));
        if (atkTypeSelect >= meleeChance)
        {
            // ���� ���� ���� ��Ŀ����
        }
        else
        {
            // ���Ÿ� ���� ���� ��Ŀ����
        }
    }

    void Update()
    {
        // �� �����Ӹ��� ����Ǵ� ����
    }
}