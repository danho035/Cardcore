using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStats : MonoBehaviour
{
    // 유닛의 타입을 나타내는 열거형
    public enum UnitType
    {
        Normal,
        Middle,
        Boss,
        Player
    }

    // 유닛의 이름
    public string unitName;

    // 유닛의 타입
    public UnitType unitType;

    // 유닛의 공격력
    public int attackPower;

    // 유닛의 방어력
    public int defensePower;

    // 유닛의 스피드
    public float speed;

    // 유닛의 체력
    public int healthPoints;

    // 유닛 스탯을 초기화하는 메서드
    public void InitializeStats(string name, UnitType type, int attack, int defense, float spd, int health)
    {
        unitName = name;
        unitType = type;
        attackPower = attack;
        defensePower = defense;
        speed = spd;
        healthPoints = health;
    }

    // 유닛 스탯을 출력하는 메서드
    public void PrintStats()
    {
        Debug.Log("Unit Name: " + unitName);
        Debug.Log("Unit Type: " + unitType);
        Debug.Log("Attack Power: " + attackPower);
        Debug.Log("Defense Power: " + defensePower);
        Debug.Log("Speed: " + speed);
        Debug.Log("Health Points: " + healthPoints);
    }

    // Start 함수에서 유닛 스탯 초기화 및 출력 예시
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
            Debug.Log(gameObject.name + "가 사망했습니다!");
            Destroy(gameObject);
        }
    }
}

