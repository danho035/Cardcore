using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public SwipeInputManager swipeInputManager; // SwipeInputManager 스크립트 참조
    private TileInfoGenerator tileInfoGenerator; // TileInfoGenerator 스크립트 참조

    // 현재 플레이어가 위치한 타일 정보 확인 메서드
    void CheckCurrentTileInfo()
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
                            // 가져온 타일 정보를 사용하여 행과 열의 값을 불러오기
                            int row = tileInfo.row;
                            int column = tileInfo.column;
                            Debug.Log("플레이어의 위치 - 행: " + row + ", 열: " + column);
                            break;
                        }
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckCurrentTileInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
}