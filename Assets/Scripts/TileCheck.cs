using UnityEngine;
using UnityEngine.UI;

public class TileCheck : MonoBehaviour
{
    private RectTransform rectTransform;
    private BoxCollider2D boxCollider2D;

    public bool MeleeAtk;
    public bool RangeAtk;
    public bool Player;
    public bool Enemy;

    void UpdateColliderSize()
    {
        // UI 요소의 크기를 가져와서 BoxCollider의 크기로 설정
        Vector2 sizeDelta = rectTransform.sizeDelta;
        boxCollider2D.size = new Vector2(sizeDelta.x, sizeDelta.y);
    }

    void Start()
    {
        // 초기화
        MeleeAtk = false;
        RangeAtk = false;
        Player = false;
        Enemy = false;

        // RectTransform 컴포넌트와 BoxCollider 컴포넌트를 가져옴
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        // UI 요소의 크기가 변경될 때마다 UpdateColliderSize 함수를 호출하여 BoxCollider 크기를 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    void Update()
    {
        // UI 요소의 크기가 변경되면 BoxCollider 크기를 업데이트
        if (rectTransform.hasChanged)
        {
            UpdateColliderSize();
            rectTransform.hasChanged = false; // hasChanged 상태를 초기화
        }

        // 박스 콜라이더 2D의 위치와 크기를 기준으로 충돌하는 모든 콜라이더를 가져옴
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxCollider2D.size, 0f);

        // Player 및 Enemy 상태를 초기화
        Player = false;
        Enemy = false;

        foreach (Collider2D collider in colliders)
        {
            string objectName = collider.gameObject.name;

            if (objectName.StartsWith("Player_"))
            {
                Player = true;
                // Debug.Log("타일 이름: " + tileObjectName + ", 충돌 오브젝트 이름: " + objectName + ", Player true 상태");
            }
            else if (objectName.StartsWith("Normal_Monster") || objectName.StartsWith("Middle_Monster") || objectName.StartsWith("Boss_Monster"))
            {
                Enemy = true;
                // Debug.Log("타일 이름: " + tileObjectName + ", 충돌 오브젝트 이름: " + objectName + ", Enemy true 상태");
            }
        }
    }
}