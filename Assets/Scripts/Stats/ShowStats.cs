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
    
    // ���� �ӵ�
    public float MoveSpeed;

    // ������ ������ �����ϴ� �Լ�
    public void SetStats(string name, int type, int spawnArea, int level, int hp, int atk, int def, int shield, float critChance, float atkSpeed, float moveSpeed)
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
        MoveSpeed = moveSpeed;
    }

    void ApplyMoveSpeedToPlayerHandler()
    {
        if (gameObject.name.StartsWith("Player_"))
        {
            PlayerHandler playerHandler = gameObject.GetComponent<PlayerHandler>();
            if (playerHandler != null && MoveSpeed > 0)
            {
                playerHandler.duration = MoveSpeed;
            }
            else if (playerHandler == null)
            {
                Debug.LogError("Player ������Ʈ���� PlayerHandler�� ã�� �� �����ϴ�.");
            }
            else
            {
                Debug.LogError("������ MoveSpeed�� 0�Դϴ�.");
            }
        }
    }

    void Start()
    {
        Invoke("ApplyMoveSpeedToPlayerHandler", 0.15f);
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