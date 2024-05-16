using UnityEngine;
using UnityEngine.UI;

public class UnitGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // 생성할 타일 프리팹
    public int rows; // 행 수
    public int columns; // 열 수
    public GridLayoutGroup gridLayoutGroup; // 그리드 레이아웃 그룹 지정

    float screenWidth; // 화면 너비
    float screenHeight; // 화면 높이

    void GenerateTiles() // 타일 생성
    {
        GameObject firstTile = null; // 첫 번째 타일을 저장할 변수

        // 행과 열을 반복하여 타일을 생성합니다.
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // 타일 프리팹을 복제하여 새로운 타일을 생성합니다.
                GameObject newTile = Instantiate(tilePrefab, transform);

                // 첫 번째 타일일 경우 변수에 저장합니다.
                if (row == 0 && col == 0)
                {
                    firstTile = newTile;
                }
            }
        }
        // 첫 번째 타일을 제거합니다.
        if (firstTile != null)
        {
            Destroy(firstTile);
        }
    }

    void TilesSize() // 타일 사이즈 지정
    {
        if (gridLayoutGroup != null)
        {
            // 그리드 레이아웃 그룹의 열 수를 설정합니다.
            gridLayoutGroup.constraintCount = columns;

            // 그리드의 최대 너비와 높이를 가져옵니다.
            float maxGridWidth = gridLayoutGroup.GetComponent<RectTransform>().rect.width;
            float maxGridHeight = gridLayoutGroup.GetComponent<RectTransform>().rect.height;

            // 가로 방향의 타일 크기를 계산합니다.
            float cellSizeX = maxGridWidth / columns;
            // 해당 크기로 세로 방향의 타일 크기를 계산합니다.
            float cellSizeY = cellSizeX * (8f / 5f);

            // 타일이 그리드의 최대 높이를 초과하는지 확인하고 조정합니다.
            if (cellSizeY * rows > maxGridHeight)
            {
                // 타일이 최대 높이를 초과하는 경우, 세로 방향의 최대치로 조정합니다.
                cellSizeY = maxGridHeight / rows;
                // 이에 따라 가로 방향의 타일 크기도 조정합니다.
                cellSizeX = cellSizeY * (5f / 8f);
            }

            // 셀 크기를 설정합니다.
            gridLayoutGroup.cellSize = new Vector2(cellSizeX, cellSizeY);

            // 타일 간의 간격을 설정합니다.
            float spacing = Mathf.CeilToInt(cellSizeX) * 0.1f; // Cell Size를 10으로 나눈 몫을 올림
            gridLayoutGroup.spacing = new Vector2(spacing, spacing);
        }
        else
        {
            // 그리드 레이아웃 그룹이 할당되지 않은 경우 오류 메시지를 출력합니다.
            Debug.LogError("TileGenrator Error: Grid Layout Group이 할당되지 않았습니다.");
        }
    }

    void Start()
    {
        // 시작 시 화면 너비와 높이를 초기화합니다
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        GenerateTiles();
        TilesSize();
    }

    void Update()
    {
        // 프레임 당 화면 너비와 높이를 초기화합니다
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        TilesSize();

    }
}
