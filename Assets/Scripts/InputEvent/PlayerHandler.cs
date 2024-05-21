using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public SwipeInputManager swipeInputManager; // SwipeInputManager ��ũ��Ʈ ����
    private TileInfoGenerator tileInfoGenerator; // TileInfoGenerator ��ũ��Ʈ ����

    // ���� �÷��̾ ��ġ�� Ÿ�� ���� Ȯ�� �޼���
    void CheckCurrentTileInfo()
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
                            int row = tileInfo.row;
                            int column = tileInfo.column;
                            Debug.Log("�÷��̾��� ��ġ - ��: " + row + ", ��: " + column);
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