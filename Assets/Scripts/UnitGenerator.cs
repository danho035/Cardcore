using UnityEngine;
using UnityEngine.UI;

public class UnitGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // ������ Ÿ�� ������
    public int rows; // �� ��
    public int columns; // �� ��
    public GridLayoutGroup gridLayoutGroup; // �׸��� ���̾ƿ� �׷� ����

    float screenWidth; // ȭ�� �ʺ�
    float screenHeight; // ȭ�� ����

    void GenerateTiles() // Ÿ�� ����
    {
        GameObject firstTile = null; // ù ��° Ÿ���� ������ ����

        // ��� ���� �ݺ��Ͽ� Ÿ���� �����մϴ�.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Ÿ�� �������� �����Ͽ� ���ο� Ÿ���� �����մϴ�.
                GameObject newTile = Instantiate(tilePrefab, transform);

                // ù ��° Ÿ���� ��� ������ �����մϴ�.
                if (row == 0 && col == 0)
                {
                    firstTile = newTile;
                }
            }
        }
        // ù ��° Ÿ���� �����մϴ�.
        if (firstTile != null)
        {
            Destroy(firstTile);
        }
    }

    void TilesSize() // Ÿ�� ������ ����
    {
        if (gridLayoutGroup != null)
        {
            // �׸��� ���̾ƿ� �׷��� �� ���� �����մϴ�.
            gridLayoutGroup.constraintCount = columns;

            // �׸����� �ִ� �ʺ�� ���̸� �����ɴϴ�.
            float maxGridWidth = gridLayoutGroup.GetComponent<RectTransform>().rect.width;
            float maxGridHeight = gridLayoutGroup.GetComponent<RectTransform>().rect.height;

            // ���� ������ Ÿ�� ũ�⸦ ����մϴ�.
            float cellSizeX = maxGridWidth / columns;
            // �ش� ũ��� ���� ������ Ÿ�� ũ�⸦ ����մϴ�.
            float cellSizeY = cellSizeX * (8f / 5f);

            // Ÿ���� �׸����� �ִ� ���̸� �ʰ��ϴ��� Ȯ���ϰ� �����մϴ�.
            if (cellSizeY * rows > maxGridHeight)
            {
                // Ÿ���� �ִ� ���̸� �ʰ��ϴ� ���, ���� ������ �ִ�ġ�� �����մϴ�.
                cellSizeY = maxGridHeight / rows;
                // �̿� ���� ���� ������ Ÿ�� ũ�⵵ �����մϴ�.
                cellSizeX = cellSizeY * (5f / 8f);
            }

            // �� ũ�⸦ �����մϴ�.
            gridLayoutGroup.cellSize = new Vector2(cellSizeX, cellSizeY);

            // Ÿ�� ���� ������ �����մϴ�.
            float spacing = Mathf.CeilToInt(cellSizeX) * 0.1f; // Cell Size�� 10���� ���� ���� �ø�
            gridLayoutGroup.spacing = new Vector2(spacing, spacing);
        }
        else
        {
            // �׸��� ���̾ƿ� �׷��� �Ҵ���� ���� ��� ���� �޽����� ����մϴ�.
            Debug.LogError("TileGenrator Error: Grid Layout Group�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Start()
    {
        // ���� �� ȭ�� �ʺ�� ���̸� �ʱ�ȭ�մϴ�
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        GenerateTiles();
        TilesSize();
    }

    void Update()
    {
        // ������ �� ȭ�� �ʺ�� ���̸� �ʱ�ȭ�մϴ�
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        TilesSize();

    }
}
