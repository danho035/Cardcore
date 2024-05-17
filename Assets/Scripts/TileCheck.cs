using UnityEngine;

public class TileCheck : MonoBehaviour
{
    public static bool MeleeAtk;
    public static bool RangeAtk;
    public static bool Player;
    public static bool Enemy;

    void Start()
    {
        // 초기화
        MeleeAtk = false;
        RangeAtk = false;
        Player = false;
        Enemy = false;
    }

    void Update()
    {
        // 버튼 위에 있는 모든 UI 오브젝트 가져오기
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        // hits 배열의 모든 요소를 반복
        foreach (RaycastHit2D hit in hits)
        {
            // 디버그 로그로 충돌하는 오브젝트 출력
            Debug.Log("충돌한 오브젝트 이름: " + hit.collider.gameObject.name);

            // 오브젝트의 이름이 Player_로 시작하는지 확인
            if (hit.collider.gameObject.name.StartsWith("Player_"))
            {
                // Player를 true로 설정
                Player = true;
                // 디버그 로그 출력
                Debug.Log("Player가 True로 되었습니다 - " + transform.gameObject.name);
            }

            // 오브젝트의 이름이 Monster_로 시작하는지 확인
            if (hit.collider.gameObject.name.StartsWith("Monster_"))
            {
                // Enemy를 true로 설정
                Enemy = true;
                // 디버그 로그 출력
                Debug.Log("Enemy가 True로 되었습니다 - " + transform.gameObject.name);
            }
        }
    }
}