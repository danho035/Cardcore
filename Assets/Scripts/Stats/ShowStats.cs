using UnityEngine;

public class ShowStats : MonoBehaviour
{
    // ������ �̸�
    public string Name;

    // ������ ����
    public int Type;

    // ������ ��ȯ�Ǵ� ����
    public int SpawnArea;

    // ������ ����
    public int Level;

    // ������ ü��
    public int HP;

    // ������ ���ݷ�
    public int Atk;

    // ������ ����
    public int Def;

    // ������ ��ȣ��
    public int Shield;

    // ġ��Ÿ Ȯ��
    public float CritChance;

    // ���� �ӵ�
    public float AtkSpeed;

    // ������ ������ �����ϴ� �Լ�
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
            Debug.Log(gameObject.name + "�� ����߽��ϴ�!");
            Destroy(gameObject);
        }
    }
}