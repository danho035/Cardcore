using UnityEngine;

public class StatGenerator : MonoBehaviour
{
    public StatManager statManager;

    void Start()
    {
        statManager = GetComponent<StatManager>();

        // 몬스터 스탯 생성 예시
        UnitStats goblinStats = new UnitStats("Goblin", 1, 0, 1, 12, 4, 2, 0, 0f, 1f, 0f);
        UnitStats slimeStats = new UnitStats("Slime", 1, 0, 1, 10, 3, 2, 0, 0f, 1f, 0f);
        UnitStats robberStats = new UnitStats("Robber", 1, 1, 1, 18, 6, 2, 0, 0f, 1f, 0f);
        UnitStats boarStats = new UnitStats("Boar", 1, 1, 1, 16, 8, 4, 0, 0f, 1f, 0f);
        UnitStats kingSlimeStats = new UnitStats("King Slime", 2, 1, 1, 30, 10, 10, 0, 0f, 1f, 0f);
        UnitStats Dragon = new UnitStats("Dragon", 3, 0, 1, 150, 15, 15, 0, 0f, 1f, 0f);

        // 몬스터 스탯 추가
        statManager.AddMonsterStats("Goblin", goblinStats);
        statManager.AddMonsterStats("Slime", slimeStats);
        statManager.AddMonsterStats("Robber", robberStats);
        statManager.AddMonsterStats("Boar", boarStats);
        statManager.AddMonsterStats("King Slime", kingSlimeStats);
        statManager.AddMonsterStats("Dragon", Dragon);

        // 플레이어 스탯 생성 예시
        UnitStats knightStats = new UnitStats("Knight", 4, 0, 1, 18, 5, 2, 0, 0f, 1f, 0.5f);
        UnitStats wizardStats = new UnitStats("Wizard", 4, 0, 1, 12, 8, 2, 0, 0f, 1f, 0.42f);
        UnitStats thiefStats = new UnitStats("Thief", 4, 0, 1, 14, 6, 2, 0, 0f, 1f, 0.33f);

        // 플레이어 스탯 추가
        statManager.AddPlayerStats("Knight", knightStats);
        statManager.AddPlayerStats("Wizard", wizardStats);
        statManager.AddPlayerStats("Thief", thiefStats);
    }
}