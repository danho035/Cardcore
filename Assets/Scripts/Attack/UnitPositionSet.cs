using System.Collections.Generic;
using UnityEngine;

public class UnitPositionSet : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator;

    public List<(string objectName, int row, int column)> unitPositions = new List<(string, int, int)>();

    void PositionReset()
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
                            // ������ Ÿ�� ������ ����Ͽ� ��� ���� ���� ����Ʈ�� �߰�
                            unitPositions.Add((objectName, tileInfo.row, tileInfo.column));
                            Debug.Log(gameObject.name + "��ġ - ��: " + tileInfo.row + ", ��: " + tileInfo.column);
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
            Debug.LogError("TileInfoGenerator�� ã�� �� �����ϴ�!");
            return;
        }

        // ����Ʈ �ʱ�ȭ 
        unitPositions.Clear();

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
                        // ������ Ÿ�� ������ ����Ͽ� ��� ���� ���� ����Ʈ�� �߰�
                        unitPositions.Add((objectName, tileInfo.row, tileInfo.column));
                        Debug.Log(gameObject.name + "�� ����� ��ġ - ��: " + tileInfo.row + ", ��: " + tileInfo.column);
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
