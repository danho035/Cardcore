using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // üũ�� �׸��� ���̾ƿ� �׷�
    private Vector2 lastCellSize; // �������� ����� �� ������
    private Vector2 lastSpacing; // �������� ����� ����
    private List<TileInfo> tilePositions = new List<TileInfo>(); // Ÿ���� ��ǥ�� �̸� ����Ʈ

    void Start()
    {
        UpdateGridLayoutProperties(); // ������ �� �׸��� ���̾ƿ� �׷��� �Ӽ��� ������Ʈ�մϴ�.
        StartCoroutine(CheckChildObjectCoordinatesDelayed()); // 0.01�� �Ŀ� �ڽ� ������Ʈ�� ��ǥ�� üũ�մϴ�.
    }

    void Update()
    {
        if (CheckIfPropertiesChanged()) // �Ӽ��� ����Ǿ����� Ȯ���մϴ�.
        {
            // ����Ǿ��ٸ� �׸��� ���̾ƿ� �׷��� �Ӽ��� ������Ʈ ����Ʈ ����
            UpdateGridLayoutProperties();
            CheckChildObjectCoordinates();
        }
    }

    void UpdateGridLayoutProperties()
    {
        if (gridLayoutGroup != null)
        {
            // ������ ����� �� ������Ʈ
            lastCellSize = gridLayoutGroup.cellSize;
            lastSpacing = gridLayoutGroup.spacing;

            Debug.Log("�� ������: " + lastCellSize);
            Debug.Log("����: " + lastSpacing);
        }
        else
        {
            Debug.LogError("GridLayoutGroup�� �������� �ʾҽ��ϴ�.");
        }
    }

    bool CheckIfPropertiesChanged()
    {
        if (gridLayoutGroup != null)
        {
            // ���� ���� ���� �� ���Ͽ� ���� ���� Ȯ��
            return lastCellSize != gridLayoutGroup.cellSize || lastSpacing != gridLayoutGroup.spacing;
        }
        else
        {
            return false;
        }
    }

    IEnumerator CheckChildObjectCoordinatesDelayed()
    {
        yield return new WaitForSeconds(0.01f); // 0.01�� ��� �Ŀ� ����˴ϴ�.
        CheckChildObjectCoordinates();
    }

    void CheckChildObjectCoordinates()
    {
        // �׸��� ���� �� �� ��������
        int columns = gridLayoutGroup.constraintCount;
        int rows = transform.childCount / columns;

        // ����Ʈ �ʱ�ȭ
        tilePositions.Clear();

        // �� �ڽ� ������Ʈ�� ��ǥ�� �̸��� üũ�ϰ� ����Ʈ�� �߰��մϴ�.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i); // ĳ��

            // ������Ʈ �̸� ��������
            string name = childTransform.name;

            // ������Ʈ �̸����� ��(row)�� ��(column) ����
            int row = i / columns;
            int column = i % columns;

            // ��ǥ�� ��������
            Vector3 position = childTransform.position;

            // ����Ʈ�� �߰� (��, ��, x��ǥ, y��ǥ, �̸�)
            tilePositions.Add(new TileInfo(row, column, position.x, position.y, name, lastCellSize, lastSpacing));
        }

        // ����Ʈ�� üũ�� �� �ִ� ����� �α� �߰�
        Debug.Log("Ÿ�� ��ǥ ����: ");
        foreach (TileInfo info in tilePositions)
        {
            Debug.Log(info.ToString());
        }
    }

    // Ÿ�� ������ ��� Ŭ����
    public class TileInfo
    {
        public int row;
        public int column;
        public float x;
        public float y;
        public string name;
        public Vector2 cellSize;
        public Vector2 spacing;

        public TileInfo(int row, int column, float x, float y, string name, Vector2 cellSize, Vector2 spacing)
        {
            this.row = row;
            this.column = column;
            this.x = x;
            this.y = y;
            this.name = name;
            this.cellSize = cellSize;
            this.spacing = spacing;
        }

        public override string ToString()
        {
            return "�̸�: " + name + ", ��: " + row + ", ��: " + column + ", X ��ǥ: " + x + ", Y ��ǥ: " + y
                + ", �� ������: " + cellSize + ", ����: " + spacing;
        }
    }

    // �ܺο��� tilePositions ����Ʈ�� ������ �� �ֵ��� �ϴ� �޼���
    public List<TileInfo> GetTilePositions()
    {
        return tilePositions;
    }
}