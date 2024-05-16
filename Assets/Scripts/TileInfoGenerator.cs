using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // üũ�� �׸��� ���̾ƿ� �׷�
    private Vector2 lastCellSize; // �������� ����� �� ������
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

            Debug.Log("�� ������: " + lastCellSize);
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
            return lastCellSize != gridLayoutGroup.cellSize;
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

            // Ÿ���� ũ�� ��������
            RectTransform rectTransform = childTransform.GetComponent<RectTransform>();
            Vector2 tileSize = rectTransform.sizeDelta;

            // ��ǥ�� ��������
            Vector3 position = childTransform.position;

            // ���� ���
            float horizontalSpacing = 0f;
            float verticalSpacing = 0f;
            if (column > 0)
            {
                horizontalSpacing = Mathf.Abs(transform.GetChild(i - 1).position.x - position.x) - tileSize.x;
            }
            if (row > 0)
            {
                verticalSpacing = Mathf.Abs(transform.GetChild(i - columns).position.y - position.y) - tileSize.y;
            }

            // ����Ʈ�� �߰� (��, ��, x��ǥ, y��ǥ, �̸�, ���� ����, ���� ����)
            tilePositions.Add(new TileInfo(row, column, position.x, position.y, name, tileSize, horizontalSpacing, verticalSpacing));
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
        public Vector2 tileSize;
        public float horizontalSpacing;
        public float verticalSpacing;

        public TileInfo(int row, int column, float x, float y, string name, Vector2 tileSize, float horizontalSpacing, float verticalSpacing)
        {
            this.row = row;
            this.column = column;
            this.x = x;
            this.y = y;
            this.name = name;
            this.tileSize = tileSize;
            this.horizontalSpacing = horizontalSpacing;
            this.verticalSpacing = verticalSpacing;
        }

        public override string ToString()
        {
            return "�̸�: " + name + ", ��: " + row + ", ��: " + column + ", X ��ǥ: " + x + ", Y ��ǥ: " + y
                + ", Ÿ�� ������: " + tileSize + ", ���� ����: " + horizontalSpacing + ", ���� ����: " + verticalSpacing;
        }
    }

    // �ܺο��� tilePositions ����Ʈ�� ������ �� �ֵ��� �ϴ� �޼���
    public List<TileInfo> GetTilePositions()
    {
        return tilePositions;
    }
}