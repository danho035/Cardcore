using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStats : MonoBehaviour
{
    // ������ Ÿ���� ��Ÿ���� ������
    public enum UnitType
    {
        Normal,
        Middle,
        Boss,
        Player
    }

    // ������ �̸�
    public string unitName;

    // ������ Ÿ��
    public UnitType unitType;

    // ������ ���ݷ�
    public int attackPower;

    // ������ ����
    public int defensePower;

    // ������ ���ǵ�
    public float speed;

    // ������ ü��
    public int healthPoints;

    // ���� ������ �ʱ�ȭ�ϴ� �޼���
    public void InitializeStats(string name, UnitType type, int attack, int defense, float spd, int health)
    {
        unitName = name;
        unitType = type;
        attackPower = attack;
        defensePower = defense;
        speed = spd;
        healthPoints = health;
    }

    // ���� ������ ����ϴ� �޼���
    public void PrintStats()
    {
        Debug.Log("Unit Name: " + unitName);
        Debug.Log("Unit Type: " + unitType);
        Debug.Log("Attack Power: " + attackPower);
        Debug.Log("Defense Power: " + defensePower);
        Debug.Log("Speed: " + speed);
        Debug.Log("Health Points: " + healthPoints);
    }

    // Start �Լ����� ���� ���� �ʱ�ȭ �� ��� ����
    private void Start()
    {
        if (gameObject.name.EndsWith("_Prefab"))
        {
            healthPoints = 1;
        }
    }
    void Update()
    {
        if (healthPoints <= 0)
        {
            Debug.Log(gameObject.name + "�� ����߽��ϴ�!");
            Destroy(gameObject);
        }
    }
}

