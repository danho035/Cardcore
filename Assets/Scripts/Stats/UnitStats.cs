[System.Serializable]
public class UnitStats
{
    public string Name { get; set; }
    public int Type { get; set; }
    public int SpawnArea { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Shield { get; set; }
    public float CritChance { get; set; }
    public float AtkSpeed { get; set; }
    public float MoveSpeed { get; set; }

    // »ý¼ºÀÚ
    public UnitStats(string name, int type, int spawnArea, int level, int hp, int atk, int def, int shield, float critChance, float atkSpeed, float moveSpeed)
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
}