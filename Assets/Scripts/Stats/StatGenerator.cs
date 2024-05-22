using UnityEngine;

public class StatGenerator : MonoBehaviour
{
    public StatManager statManager;

    void Start()
    {
        statManager = GetComponent<StatManager>();

        // 몬스터 스탯 생성 예시
        UnitStats goblinStats = new UnitStats("Goblin", 1, 0, 1, 12, 4, 2, 0, 0f, 1f);
        UnitStats slimeStats = new UnitStats("Slime", 1, 0, 1, 10, 3, 2, 0, 0f, 1f);
        UnitStats robberStats = new UnitStats("Robber", 1, 1, 1, 18, 6, 2, 0, 0f, 1f);
        UnitStats boarStats = new UnitStats("Boar", 1, 1, 1, 16, 8, 4, 0, 0f, 1f);
        UnitStats kingSlimeStats = new UnitStats("King Slime", 2, 1, 1, 30, 10, 10, 0, 0f, 1f);
        UnitStats Dragon = new UnitStats("Dragon", 3, 0, 1, 150, 15, 15, 0, 0f, 1f);

        // 몬스터 스탯 추가
        statManager.AddMonsterStats("Goblin", goblinStats);
        statManager.AddMonsterStats("Slime", slimeStats);
        statManager.AddMonsterStats("Robber", robberStats);
        statManager.AddMonsterStats("Boar", boarStats);
        statManager.AddMonsterStats("King Slime", kingSlimeStats);
        statManager.AddMonsterStats("Dragon", Dragon);

        // 특정 몬스터의 스탯 가져오기 예시
        UnitStats goblinStatsRetrieved = statManager.GetMonsterStats("Goblin");
        Debug.Log("Goblin HP: " + goblinStatsRetrieved.HP);
    }
}