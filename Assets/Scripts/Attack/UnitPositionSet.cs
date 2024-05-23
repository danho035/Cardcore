using System.Collections.Generic;
using UnityEngine;

public class UnitPositionSet : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator;

    public List<(string objectName, int row, int column)> unitPositions = new List<(string, int, int)>();

    void PositionReset()
    {
        // 플레이어가 속한 타일의 정보를 가져오기 위해 박스 콜라이더2D 컴포넌트 찾기
        BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();

        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();

        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator를 찾을 수 없습니다!");
            return;
        }

        if (playerCollider != null)
        {
            // 박스 콜라이더 2D와 접촉한 오브젝트 이름 가져오기
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
            foreach (Collider2D collider in colliders)
            {
                string objectName = collider.gameObject.name;
                Debug.Log("접촉한 오브젝트 이름: " + objectName);

                // 타일 정보 가져오기
                List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
                if (tileInfos != null)
                {
                    foreach (var tileInfo in tileInfos)
                    {
                        if (tileInfo.tileName == objectName)
                        {
                            // 가져온 타일 정보를 사용하여 행과 열의 값을 리스트에 추가
                            unitPositions.Add((objectName, tileInfo.row, tileInfo.column));
                            Debug.Log(gameObject.name + "위치 - 행: " + tileInfo.row + ", 열: " + tileInfo.column);
                            break;
                        }
                    }
                }
            }
        }
    }

    void UpdatePosition()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator를 찾을 수 없습니다!");
            return;
        }

        // 리스트 초기화 
        unitPositions.Clear();

        // 박스 콜라이더 2D와 접촉한 오브젝트 이름 가져오기
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            string objectName = collider.gameObject.name;
            Debug.Log("접촉한 오브젝트 이름: " + objectName);

            // 타일 정보 가져오기
            List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
            if (tileInfos != null)
            {
                foreach (var tileInfo in tileInfos)
                {
                    if (tileInfo.tileName == objectName)
                    {
                        // 가져온 타일 정보를 사용하여 행과 열의 값을 리스트에 추가
                        unitPositions.Add((objectName, tileInfo.row, tileInfo.column));
                        Debug.Log(gameObject.name + "의 변경된 위치 - 행: " + tileInfo.row + ", 열: " + tileInfo.column);
                        break;
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PositionReset();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
}
