using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public InputManager InputManager; // SwipeInputManager ��ũ��Ʈ ����
    public TileInfoGenerator tileInfoGenerator; // TileInfoGenerator ��ũ��Ʈ ����
    public GameObject objectToDestroy;

    public float duration; // �ִϸ��̼� ��� �ð�

    private int playerRow;
    private int playerColumn;
    private bool isMoving = false; // �÷��̾ �̵� ������ üũ

    // �÷��̾� ��ġ �ʱ�ȭ
    void PostionReset()
    {
        // �÷��̾ ���� Ÿ���� ������ �������� ���� �ڽ� �ݶ��̴�2D ������Ʈ ã��
        BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();

        tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator�� ã�� �� �����ϴ�!");
            return;
        }

        if (playerCollider != null)
        {
            // �ڽ� �ݶ��̴� 2D�� ������ ������Ʈ �̸� ��������
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
            foreach (Collider2D collider in colliders)
            {
                string objectName = collider.gameObject.name;
                Debug.Log("������ ������Ʈ �̸�: " + objectName);

                // Ÿ�� ���� ��������
                List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
                if (tileInfos != null)
                {
                    foreach (var tileInfo in tileInfos)
                    {
                        if (tileInfo.tileName == objectName)
                        {
                            // ������ Ÿ�� ������ ����Ͽ� ��� ���� ���� �ҷ�����
                            playerRow = tileInfo.row;
                            playerColumn = tileInfo.column;
                            Debug.Log("�÷��̾��� ��ġ - ��: " + playerRow + ", ��: " + playerColumn);
                            break;
                        }
                    }
                }
            }
        }
    }

    // �÷��̾� ��ġ ������Ʈ
    void UpdatePosition()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator�� ã�� �� �����ϴ�!");
            return;
        }

        // �ڽ� �ݶ��̴� 2D�� ������ ������Ʈ �̸� ��������
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            string objectName = collider.gameObject.name;
            Debug.Log("������ ������Ʈ �̸�: " + objectName);

            // Ÿ�� ���� ��������
            List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
            if (tileInfos != null)
            {
                foreach (var tileInfo in tileInfos)
                {
                    if (tileInfo.tileName == objectName)
                    {
                        // ������ Ÿ�� ������ ����Ͽ� ��� ���� ���� �ҷ�����
                        playerRow = tileInfo.row;
                        playerColumn = tileInfo.column;

                        Debug.Log("�÷��̾��� ��ġ - ��: " + playerRow + ", ��: " + playerColumn);
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
            Debug.LogError("TileInfoGenerator�� ã�� �� �����ϴ�!");
            return;
        }

        // �̵��� Ÿ�� ���� ��������
        List<TileInfoGenerator.TileInfo> tileInfos = tileInfoGenerator.GetTileInfos();
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("Ÿ�� ������ ������ �� �����ϴ�.");
            return;
        }

        // �̵��� Ÿ�� ã��
        TileInfoGenerator.TileInfo targetTile = tileInfos.Find(tile => tile.row == newRow && tile.column == newColumn);
        if (targetTile == null)
        {
            Debug.LogWarning("��ǥ Ÿ���� ã�� �� �����ϴ�: �� " + newRow + ", �� " + newColumn);
            StartCoroutine(ShakePlayer());
            return;
        }

        // Ÿ���� TileCheck ��ũ��Ʈ ��������
        TileCheck targetTileCheck = GameObject.Find(targetTile.tileName).GetComponent<TileCheck>();
        if (targetTileCheck == null)
        {
            Debug.LogError("TileCheck ��ũ��Ʈ�� ã�� �� �����ϴ�: " + targetTile.tileName);
            return;
        }

        // �÷��̾� �Ǵ� ���� �ִ��� Ȯ��
        if (targetTileCheck.Player || targetTileCheck.Enemy)
        {
            Debug.LogWarning("�̵��� �� �����ϴ�. Ÿ�Ͽ� �ٸ� ������Ʈ�� �ֽ��ϴ�: " + targetTile.tileName);
            StartCoroutine(ShakePlayer());
        }
        else
        {
            // �̵� ����, �÷��̾� �̵�
            StartCoroutine(MovePlayer(GameObject.Find(targetTile.tileName).transform.position));
            playerRow = newRow;
            playerColumn = newColumn;
        }
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        float preparationDistance = 15f; // �غ� �ܰ迡�� �ݴ� �������� �̵��ϴ� �Ÿ� ����
        isMoving = true;

        Vector3 startingPosition = transform.position;
        float elapsedTime = 0f;

        // �غ� ��ġ�� ��� (Ÿ�� ��ġ�� �ݴ� �������� ���� �Ÿ���ŭ �̵�)
        Vector3 directionToTarget = (targetPosition - startingPosition).normalized;
        Vector3 preparationPosition = startingPosition - directionToTarget * preparationDistance; // �ݴ� �������� preparationDistance ��ŭ �̵�

        // �غ� �ܰ� (��ü duration�� 75%)
        while (elapsedTime < duration * 0.75f)
        {
            transform.position = Vector3.Lerp(startingPosition, preparationPosition, elapsedTime / (duration * 0.75f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� �̵� �ܰ� (��ü duration�� 25%)
        float fastMoveStartTime = elapsedTime;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(preparationPosition, targetPosition, (elapsedTime - fastMoveStartTime) / (duration * 0.25f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ�� ��Ȯ�� Ÿ�� ��ġ�� ����
        transform.position = targetPosition;
        isMoving = false;

        // �̵� �� ���� Ÿ�� ���� ����
        UpdatePosition();
    }

    IEnumerator ShakePlayer() // ����Ÿ���
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.15f; // ����Ÿ��� �ִϸ��̼��� ���� �ð�
        float shakeMagnitude = 15f; // ����Ÿ��� ����
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

            transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ����Ÿ��� �ִϸ��̼� �� ���� ��ġ�� ����
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
                Debug.LogError("PlayerHandler�� InputManager�� �������� �ʾҽ��ϴ�");
            }
            else if (tileInfoGenerator == null)
            {
                Debug.LogError("PlayerHandle�� TileInfoGenerator�� �������� �ʾҽ��ϴ�");
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
            Debug.LogError("SwipeInputManager�� ã�� �� �����ϴ� - PlayerHandler");
            return;
        }

        if (!isMoving && InputManager.Gesture_Result != Result.none)
        {
            Debug.Log("���� �������� ���: " + InputManager.Gesture_Result);
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
                Debug.Log("Ŭ�� �ν�");
            }

            // �������� ����� �ʱ�ȭ
            InputManager.ResetGestureResult();
        }
    }
}