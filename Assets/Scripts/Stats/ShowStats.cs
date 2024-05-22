using UnityEngine;

public class ShowStats : MonoBehaviour
{
    // 유닛의 이름
    public string Name;

    // 유닛의 종류
    public int Type;

    // 유닛이 소환되는 지역
    public int SpawnArea;

    // 유닛의 레벨
    public int Level;

    // 유닛의 체력
    public int HP;

    // 유닛의 공격력
    public int Atk;

    // 유닛의 방어력
    public int Def;

    // 유닛의 보호막
    public int Shield;

    // 치명타 확률
    public float CritChance;

    // 공격 속도
    public float AtkSpeed;

    // 유닛의 스탯을 설정하는 함수
    public void SetStats(string name, int type, int spawnArea, int level, int hp, int atk, int def, int shield, float critChance, float atkSpeed)
    {
        Name = name;
        Type = type;
        SpawnArea = spawnArea;
        Level = level;
        HP = hp;
        Atk = atk;
        Def = def;
        Shield = shield;
        CritChance = critChance;
        AtkSpeed = atkSpeed;
    }

    void Update()
    {
        if (HP <= 0)
        {
            Debug.Log(gameObject.name + "가 사망했습니다!");
            Destroy(gameObject);
        }
    }
}