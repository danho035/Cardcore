using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // ����ڰ� �׸��� ���̾ƿ� �׷��� ���� ������ �� �ֵ��� ������ public���� ����

    private List<TileInfo> tileInfos = new List<TileInfo>(); // Ÿ�� ���� ����Ʈ

    void Start()
    {
        // Start() �Լ��� ȣ��ǰ� ���� 0.0001�� �Ŀ� GenerateTileInfo() �Լ��� ȣ���ϵ��� ����ϴ�.
        Invoke("GenerateTileInfo", 0.0001f);
    }

    void GenerateTileInfo()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogError("Grid Layout Group�� �������� �ʾҽ��ϴ�.");
            return;
        }

        int columns = gridLayoutGroup.constraintCount; // �� ��
        int rows = gridLayoutGroup.transform.childCount / columns; // �� ��

        // ���� ���� ��������
        float spacing = gridLayoutGroup.spacing.x;

        // �� ������ ��������
        Vector2 cellSize = gridLayoutGroup.cellSize;
        float cellWidth = cellSize.x;
        float cellHeight = cellSize.y;

        RectTransform gridRectTransform = gridLayoutGroup.GetComponent<RectTransform>();

        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            RectTransform tileRectTransform = gridLayoutGroup.transform.GetChild(i) as RectTransform;

            // Ÿ�� ���� ����
            TileInfo tileInfo = new TileInfo();
            tileInfo.tileName = tileRectTransform.name;
            tileInfo.row = i / columns;
            tileInfo.column = i % columns;

            // Ÿ���� anchoredPosition�� ����Ͽ� ���� ��ġ ��������
            tileInfo.posX = tileRectTransform.anchoredPosition.x;
            tileInfo.posY = tileRectTransform.anchoredPosition.y;

            tileInfo.width = cellWidth;
            tileInfo.height = cellHeight;
            tileInfo.spacing = spacing;

            tileInfos.Add(tileInfo); // ����Ʈ�� Ÿ�� ���� �߰�
        }

        // Ÿ�� ���� ���
        Debug.Log("Ÿ�� ����:");
        foreach (TileInfo info in tileInfos)
        {
            Debug.Log(info.ToString());
        }
    }

    // Ÿ�� ������ ��� Ŭ����
    public class TileInfo
    {
        public string tileName;
        public int row;
        public int column;
        public float posX;
        public float posY;
        public float width;
        public float height;
        public float spacing;

        public override string ToString()
        {
            return "�̸�: " + tileName + ", ��: " + row + ", ��: " + column + ", Pos X: " + posX + ", Pos Y: " + posY
                + ", Width: " + width + ", Height: " + height + ", Spacing: " + spacing;
        }
    }

    // Ÿ�� ���� ����Ʈ�� ��ȯ�ϴ� �޼���
    public List<TileInfo> GetTileInfos()
    {
        return tileInfos;
    }
}