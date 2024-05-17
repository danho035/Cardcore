using UnityEngine;

public class TileCheck : MonoBehaviour
{
    public static bool MeleeAtk;
    public static bool RangeAtk;
    public static bool Player;
    public static bool Enemy;

    void Start()
    {
        // �ʱ�ȭ
        MeleeAtk = false;
        RangeAtk = false;
        Player = false;
        Enemy = false;
    }

    void Update()
    {
        // ��ư ���� �ִ� ��� UI ������Ʈ ��������
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        // hits �迭�� ��� ��Ҹ� �ݺ�
        foreach (RaycastHit2D hit in hits)
        {
            // ����� �α׷� �浹�ϴ� ������Ʈ ���
            Debug.Log("�浹�� ������Ʈ �̸�: " + hit.collider.gameObject.name);

            // ������Ʈ�� �̸��� Player_�� �����ϴ��� Ȯ��
            if (hit.collider.gameObject.name.StartsWith("Player_"))
            {
                // Player�� true�� ����
                Player = true;
                // ����� �α� ���
                Debug.Log("Player�� True�� �Ǿ����ϴ� - " + transform.gameObject.name);
            }

            // ������Ʈ�� �̸��� Monster_�� �����ϴ��� Ȯ��
            if (hit.collider.gameObject.name.StartsWith("Monster_"))
            {
                // Enemy�� true�� ����
                Enemy = true;
                // ����� �α� ���
                Debug.Log("Enemy�� True�� �Ǿ����ϴ� - " + transform.gameObject.name);
            }
        }
    }
}