using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheckController : MonoBehaviour
{
    void CheckTileInfo(string objectName)
    {
        // ������Ʈ �̸��� ���� ���� ������Ʈ�� ã���ϴ�.
        GameObject tileObject = GameObject.Find(objectName);

        if (tileObject != null)
        {
            // Ÿ�� ������Ʈ�� TileCheck ��ũ��Ʈ�� �ִ��� Ȯ���մϴ�.
            TileCheck tileCheck = tileObject.GetComponent<TileCheck>();

            if (tileCheck != null)
            {
                // TileCheck ��ũ��Ʈ�� �ִٸ� �ش� ��ũ��Ʈ�� ������ ����մϴ�.
                Debug.Log("Ÿ�� ������Ʈ '" + objectName + "'�� ���� ����:");

                Debug.Log("MeleeAtk: " + TileCheck.MeleeAtk);
                Debug.Log("RangeAtk: " + TileCheck.RangeAtk);
                Debug.Log("Player: " + TileCheck.Player);
                Debug.Log("Enemy: " + TileCheck.Enemy);
            }
            else
            {
                Debug.LogWarning("Ÿ�� ������Ʈ '" + objectName + "'�� TileCheck ��ũ��Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�̸��� '" + objectName + "'�� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // Ÿ�� ���� ��������
    void TileInfoCheck()
    {
        TileInfoGenerator tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();

        if (tileInfoGenerator != null)
        {
            // Ÿ�� ���� ����Ʈ�� �����ɴϴ�.
            List<TileInfoGenerator.TileInfo> tilePositions = tileInfoGenerator.GetTilePositions();

            if (tilePositions != null)
            {
                int count = tilePositions.Count;

                Debug.Log("Ÿ�� ������ ���������� �����Խ��ϴ� - TileCheckController");
                Debug.Log("������ �ε��� ����: " + count);

                // ������ ��� ���� ����
                int randomIndex = Random.Range(0, count);
                int randomRow = tilePositions[randomIndex].row;
                int randomColumn = tilePositions[randomIndex].column;

                // ������ ��� ���� ���� Ÿ���� �̸� Ȯ��
                string objectName = GetObjectName(tilePositions, randomRow, randomColumn);
                Debug.Log("���� ���õ� ��� ��: " + randomRow + "/" + randomColumn);
                Debug.Log("���õ� ��� ���� ���� Ÿ���� �̸�: " + objectName);

                // ������ ���� ���� ������ �����մϴ�.
                int randomValue = Random.Range(1, 5);
                if (randomValue == 1)
                {
                    TileCheck.MeleeAtk = true;
                    Debug.Log("MeleeAtk�� True�� �����߽��ϴ�.");
                }
                else if (randomValue == 2)
                {
                    TileCheck.RangeAtk = true;
                    Debug.Log("RangeAtk�� True�� �����߽��ϴ�.");
                }
                else if (randomValue == 3)
                {
                    TileCheck.Player = true;
                    Debug.Log("Player�� True�� �����߽��ϴ�.");
                }
                else if (randomValue == 4)
                {
                    TileCheck.Enemy = true;
                    Debug.Log("Enemy�� True�� �����߽��ϴ�.");
                }
            }
            else
            {
                Debug.LogError("Ÿ�� ���� ����Ʈ�� ������ �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("TileInfoGenerator ��ũ��Ʈ�� ���� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // �־��� ��� ���� ���� Ÿ���� �̸��� ��ȯ�ϴ� �Լ�
    string GetObjectName(List<TileInfoGenerator.TileInfo> tilePositions, int row, int column)
    {
        foreach (TileInfoGenerator.TileInfo tileInfo in tilePositions)
        {
            if (tileInfo.row == row && tileInfo.column == column)
            {
                return tileInfo.name;
            }
        }
        return "�ش��ϴ� Ÿ���� �����ϴ�.";
    }

    void Start()
    {
        Invoke("TileInfoCheck", 0.1f);
        CheckTileInfo("Tile_0_0"); // ���÷� Ÿ�� �̸� ����
    }
}