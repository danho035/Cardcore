using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public InputManager InputManager; // SwipeInputManager 스크립트 참조
    public TileInfoGenerator tileInfoGenerator; // TileInfoGenerator 스크립트 참조
    public GameObject objectToDestroy;

    public float duration; // 애니메이션 출력 시간

    private int playerRow;
    private int playerColumn;
    private bool isMoving = false; // 플레이어가 이동 중인지 체크

    // 플레이어 위치 초기화
    void PostionReset()
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
                            playerRow = tileInfo.row;
                            playerColumn = tileInfo.column;
                            Debug.Log("플레이어의 위치 - 행: " + playerRow + ", 열: " + playerColumn);
                            break;
                        }
                    }
                }
            }
        }
    }

    // 플레이어 위치 업데이트
    void UpdatePosition()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator를 찾을 수 없습니다!");
            return;
        }

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
                        playerRow = tileInfo.row;
                        playerColumn = tileInfo.column;

                        Debug.Log("플레이어의 위치 - 행: " + playerRow + ", 열: " + playerColumn);
                        break;
                    }
                }
            }
        }
    }

    void PlayerMove(int newRow, int newColumn)
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator를 찾을 수 없습니다!");
            return;
        }

        // 이동할 타일 정보 가져오기
        List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("타일 정보를 가져올 수 없습니다.");
            return;
        }

        // 이동할 타일 찾기
        TileInfoGenerator.TileInfo targetTile = tileInfos.Find(tile => tile.row == newRow && tile.column == newColumn);
        if (targetTile == null)
        {
            Debug.LogWarning("목표 타일을 찾을 수 없습니다: 행 " + newRow + ", 열 " + newColumn);
            StartCoroutine(ShakePlayer());
            return;
        }

        // 타일의 TileCheck 스크립트 가져오기
        TileCheck targetTileCheck = GameObject.Find(targetTile.tileName).GetComponent<TileCheck>();
        if (targetTileCheck == null)
        {
            Debug.LogError("TileCheck 스크립트를 찾을 수 없습니다: " + targetTile.tileName);
            return;
        }

        // 플레이어 또는 적이 있는지 확인
        if (targetTileCheck.Player || targetTileCheck.Enemy)
        {
            Debug.LogWarning("이동할 수 없습니다. 타일에 다른 오브젝트가 있습니다: " + targetTile.tileName);
            StartCoroutine(ShakePlayer());
        }
        else
        {
            // 이동 가능, 플레이어 이동
            StartCoroutine(MovePlayer(GameObject.Find(targetTile.tileName).transform.position));
            playerRow = newRow;
            playerColumn = newColumn;
        }
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        float preparationDistance = 15f; // 준비 단계에서 반대 방향으로 이동하는 거리 조정
        isMoving = true;

        Vector3 startingPosition = transform.position;
        float elapsedTime = 0f;

        // 준비 위치를 계산 (타겟 위치의 반대 방향으로 일정 거리만큼 이동)
        Vector3 directionToTarget = (targetPosition - startingPosition).normalized;
        Vector3 preparationPosition = startingPosition - directionToTarget * preparationDistance; // 반대 방향으로 preparationDistance 만큼 이동

        // 준비 단계 (전체 duration의 75%)
        while (elapsedTime < duration * 0.75f)
        {
            transform.position = Vector3.Lerp(startingPosition, preparationPosition, elapsedTime / (duration * 0.75f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 빠른 이동 단계 (전체 duration의 25%)
        float fastMoveStartTime = elapsedTime;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(preparationPosition, targetPosition, (elapsedTime - fastMoveStartTime) / (duration * 0.25f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치를 정확히 타겟 위치로 설정
        transform.position = targetPosition;
        isMoving = false;

        // 이동 후 현재 타일 정보 갱신
        UpdatePosition();
    }

    IEnumerator ShakePlayer() // 움찍거리기
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.15f; // 움찔거리는 애니메이션의 지속 시간
        float shakeMagnitude = 15f; // 움찔거리는 정도
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 움찔거리는 애니메이션 후 원래 위치로 복원
        transform.position = originalPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        string objectName = gameObject.name;

        if (objectName == "Player_Unit")
        {
            if (InputManager == null)
            {
                Debug.LogError("PlayerHandler에 InputManager가 설정되지 않았습니다");
            }
            else if (tileInfoGenerator == null)
            {
                Debug.LogError("PlayerHandle에 TileInfoGenerator가 설정되지 않았습니다");
            }
            else
            {
                PostionReset();
            }
        }
        else
        {
            Destroy(objectToDestroy, 0.2f);
        }
    }

    void Update()
    {
        if (InputManager == null)
        {
            Debug.LogError("SwipeInputManager를 찾을 수 없습니다 - PlayerHandler");
            return;
        }

        if (!isMoving && InputManager.Gesture_Result != Result.none)
        {
            Debug.Log("현재 스와이프 결과: " + InputManager.Gesture_Result);
            if (InputManager.Gesture_Result == Result.up) // UP
            {
                PlayerMove(playerRow, playerColumn - 1);
            }
            else if (InputManager.Gesture_Result == Result.down) // Down
            {
                PlayerMove(playerRow, playerColumn + 1);
            }
            else if (InputManager.Gesture_Result == Result.left) // Left
            {
                PlayerMove(playerRow - 1, playerColumn);
            }
            else if (InputManager.Gesture_Result == Result.right) // Right
            {
                PlayerMove(playerRow + 1, playerColumn);
            }
            else if (InputManager.Gesture_Result == Result.click)
            {
                Debug.Log("클릭 인식");
            }

            // 스와이프 결과를 초기화
            InputManager.ResetGestureResult();
        }
    }
}