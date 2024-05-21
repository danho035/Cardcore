using UnityEngine;
using UnityEngine.UI;

public class TileCheck : MonoBehaviour
{
    private RectTransform rectTransform;
    private BoxCollider2D boxCollider2D;

    public static bool MeleeAtk;
    public static bool RangeAtk;
    public static bool Player;
    public static bool Enemy;

    void UpdateColliderSize()
    {
        // UI ����� ũ�⸦ �����ͼ� BoxCollider�� ũ��� ����
        Vector2 sizeDelta = rectTransform.sizeDelta;
        boxCollider2D.size = new Vector2(sizeDelta.x, sizeDelta.y);
    }

    void Start()
    {
        // �ʱ�ȭ
        MeleeAtk = false;
        RangeAtk = false;
        Player = false;
        Enemy = false;
        string objectName = gameObject.name;

        // RectTransform ������Ʈ�� BoxCollider ������Ʈ�� ������
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        // UI ����� ũ�Ⱑ ����� ������ UpdateColliderSize �Լ��� ȣ���Ͽ� BoxCollider ũ�⸦ ������Ʈ
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

    }

    void Update()
    {
        // UI ����� ũ�Ⱑ ����Ǹ� BoxCollider ũ�⸦ ������Ʈ
        if (rectTransform.hasChanged)
        {
            UpdateColliderSize();
        }

        // �ڽ� �ݶ��̴� 2D�� ��ġ�� ũ�⸦ �������� �浹�ϴ� ��� �ݶ��̴��� ������
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f);
        foreach (Collider2D collider in colliders)
        {
            string objectName = collider.gameObject.name;
            string tileObjectName = gameObject.name;

            if (objectName.StartsWith("Player_"))
            {
                Player = true;
                Debug.Log("Ÿ�� �̸�: " + tileObjectName + ", �浹 ������Ʈ �̸�: " + objectName + ", Player true ����");
            }
            else if (objectName.StartsWith("Normal_Monster") || objectName.StartsWith("Middle_Monster") || objectName.StartsWith("Boss_Monster"))
            {
                Enemy = true;
                Debug.Log("Ÿ�� �̸�: " + tileObjectName + ", �浹 ������Ʈ �̸�: " + objectName + ", Enemy true ����");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // �浹�� ���� ������Ʈ�� �̸��� ������
        string objectName = other.gameObject.name;

        // ��ġ�� ������Ʈ�� �̸��� ���� ���� ����
        if (objectName.StartsWith("Player_"))
        {
            Player = false;
            Debug.Log("�ش� ������Ʈ�� ������ �ִ� ������Ʈ �̸�: " + objectName + ", �÷��̾� false ����");
        }
        else if (objectName.StartsWith("Normal_Monster") || objectName.StartsWith("Middle_Monster") || objectName.StartsWith("Boss_Monster"))
        {
            Enemy = false;
            Debug.Log("�ش� ������Ʈ�� ������ �ִ� ������Ʈ �̸�: " + objectName + ", �� false ����");
        }
    }
}