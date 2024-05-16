using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // 체크할 그리드 레이아웃 그룹
    private Vector2 lastCellSize; // 마지막에 저장된 셀 사이즈
    private List<TileInfo> tilePositions = new List<TileInfo>(); // 타일의 좌표와 이름 리스트

    void Start()
    {
        UpdateGridLayoutProperties(); // 시작할 때 그리드 레이아웃 그룹의 속성을 업데이트합니다.
        StartCoroutine(CheckChildObjectCoordinatesDelayed()); // 0.01초 후에 자식 오브젝트의 좌표를 체크합니다.
    }

    void Update()
    {
        if (CheckIfPropertiesChanged()) // 속성이 변경되었는지 확인합니다.
        {
            // 변경되었다면 그리드 레이아웃 그룹의 속성과 오브젝트 리스트 변경
            UpdateGridLayoutProperties();
            CheckChildObjectCoordinates();
        }
    }

    void UpdateGridLayoutProperties()
    {
        if (gridLayoutGroup != null)
        {
            // 이전에 저장된 값 업데이트
            lastCellSize = gridLayoutGroup.cellSize;

            Debug.Log("셀 사이즈: " + lastCellSize);
        }
        else
        {
            Debug.LogError("GridLayoutGroup이 설정되지 않았습니다.");
        }
    }

    bool CheckIfPropertiesChanged()
    {
        if (gridLayoutGroup != null)
        {
            // 현재 값과 이전 값 비교하여 변경 여부 확인
            return lastCellSize != gridLayoutGroup.cellSize;
        }
        else
        {
            return false;
        }
    }

    IEnumerator CheckChildObjectCoordinatesDelayed()
    {
        yield return new WaitForSeconds(0.01f); // 0.01초 대기 후에 실행됩니다.
        CheckChildObjectCoordinates();
    }

    void CheckChildObjectCoordinates()
    {
        // 그리드 열과 행 수 가져오기
        int columns = gridLayoutGroup.constraintCount;
        int rows = transform.childCount / columns;

        // 리스트 초기화
        tilePositions.Clear();

        // 각 자식 오브젝트의 좌표와 이름을 체크하고 리스트에 추가합니다.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTransform = transform.GetChild(i); // 캐싱

            // 오브젝트 이름 가져오기
            string name = childTransform.name;

            // 오브젝트 이름에서 행(row)과 열(column) 추출
            int row = i / columns;
            int column = i % columns;

            // 타일의 크기 가져오기
            RectTransform rectTransform = childTransform.GetComponent<RectTransform>();
            Vector2 tileSize = rectTransform.sizeDelta;

            // 좌표값 가져오기
            Vector3 position = childTransform.position;

            // 간격 계산
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

            // 리스트에 추가 (행, 열, x좌표, y좌표, 이름, 가로 간격, 세로 간격)
            tilePositions.Add(new TileInfo(row, column, position.x, position.y, name, tileSize, horizontalSpacing, verticalSpacing));
        }

        // 리스트를 체크할 수 있는 디버그 로그 추가
        Debug.Log("타일 좌표 정보: ");
        foreach (TileInfo info in tilePositions)
        {
            Debug.Log(info.ToString());
        }
    }

    // 타일 정보를 담는 클래스
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
            return "이름: " + name + ", 행: " + row + ", 열: " + column + ", X 좌표: " + x + ", Y 좌표: " + y
                + ", 타일 사이즈: " + tileSize + ", 가로 간격: " + horizontalSpacing + ", 세로 간격: " + verticalSpacing;
        }
    }

    // 외부에서 tilePositions 리스트에 접근할 수 있도록 하는 메서드
    public List<TileInfo> GetTilePositions()
    {
        return tilePositions;
    }
}