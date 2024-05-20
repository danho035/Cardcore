using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup; // 사용자가 그리드 레이아웃 그룹을 직접 설정할 수 있도록 변수를 public으로 변경

    private List<TileInfo> tileInfos = new List<TileInfo>(); // 타일 정보 리스트

    void Start()
    {
        Invoke("GenerateTileInfo", 0.001f);
    }

    void GenerateTileInfo()
    {
        if (gridLayoutGroup == null)
        {
            Debug.LogError("Grid Layout Group이 설정되지 않았습니다.");
            return;
        }

        int columns = gridLayoutGroup.constraintCount; // 열 수
        int rows = Mathf.CeilToInt((float)gridLayoutGroup.transform.childCount / columns); // 행 수

        // 간격 정보 가져오기
        float spacing = gridLayoutGroup.spacing.x;

        // 타일 크기 가져오기
        Vector2 tileSize = gridLayoutGroup.cellSize;

        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            RectTransform tileRectTransform = gridLayoutGroup.transform.GetChild(i) as RectTransform;

            // 타일 정보 생성
            TileInfo tileInfo = new TileInfo();
            tileInfo.tileName = tileRectTransform.name;
            tileInfo.row = i / columns;
            tileInfo.column = i % columns;

            // 타일의 anchoredPosition을 사용하여 실제 위치 가져오기
            tileInfo.posX = tileRectTransform.anchoredPosition.x;
            tileInfo.posY = tileRectTransform.anchoredPosition.y;

            // 타일 크기 및 간격 정보 추가
            tileInfo.tileSize = tileSize;
            tileInfo.spacing = spacing;

            tileInfos.Add(tileInfo); // 리스트에 타일 정보 추가
        }

        // 타일 정보 출력
        Debug.Log("타일 정보:");
        foreach (TileInfo info in tileInfos)
        {
            Debug.Log(info.ToString());
        }
    }

    // 타일 정보를 담는 클래스
    public class TileInfo
    {
        public string tileName;
        public int row;
        public int column;
        public float posX;
        public float posY;
        public Vector2 tileSize; // 타일 크기
        public float spacing; // 간격

        public override string ToString()
        {
            return "이름: " + tileName + ", 행: " + row + ", 열: " + column + ", Pos X: " + posX + ", Pos Y: " + posY
                + ", 타일 크기: " + tileSize + ", 간격: " + spacing;
        }
    }

    // 다른 클래스에서 타일 정보에 접근할 수 있도록 public 메서드 추가
    public List<TileInfo> GetTileInfos()
    {
        return tileInfos;
    }
}