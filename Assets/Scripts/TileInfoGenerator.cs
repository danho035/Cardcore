using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // ����ڰ� �׸��� ���̾ƿ� �׷��� ���� ������ �� �ֵ��� ������ public���� ����

    private List<TileInfo> tileInfos = new List<TileInfo>(); // Ÿ�� ���� ����Ʈ

    void Start()
    {
        Invoke("GenerateTileInfo", 0.001f);
    }

    void GenerateTileInfo()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogError("Grid Layout Group�� �������� �ʾҽ��ϴ�.");
            return;
        }

        int columns = gridLayoutGroup.constraintCount; // �� ��

        // ���� ���� ��������
        float spacing = gridLayoutGroup.spacing.x;

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

        public override string ToString()
        {
            return "�̸�: " + tileName + ", ��: " + row + ", ��: " + column + ", Pos X: " + posX + ", Pos Y: " + posY;
        }
    }

    // �ٸ� Ŭ�������� Ÿ�� ������ ������ �� �ֵ��� public �޼��� �߰�
    public List<TileInfo> GetTileInfos()
    {
        return tileInfos;
    }
}