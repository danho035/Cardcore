using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MonsterSpawner : MonoBehaviour
{
    protected List<TileInfoGenerator.TileInfo> tileInfos;

    public int MonsterCount { get; set; }


    // 몬스터 타입 상수
    public int MonsterType { get; private set; }

    public void SetTileInfos(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        this.tileInfos = tileInfos;
    }

    // SpawnMonster 메서드 시그니처 수정
    public virtual void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        Debug.LogWarning("기본 MonsterSpawner의 SpawnMonster()가 호출되었습니다. 파생된 클래스에서 이 메서드를 오버라이드하세요");
    }
}

public class BossMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("타일 정보가 없습니다. 보스 몬스터를 스폰할 수 없습니다.");
            return;
        }

        if (tileInfos[tileInfos.Count - 1].row != 4)
        {
            Debug.Log("최대 행이 5가 아니므로 보스 몬스터를 스폰할 수 없습니다.");
            return;
        }

        if (MonsterCount <= 0)
        {
            Debug.Log("보스 몬스터 카운터가 0 이하입니다. 스폰할 보스 몬스터가 없습니다.");
            return;
        }

        float posX = tileInfos[2].posX;
        float posY = tileInfos[2].posY;
        Vector3 spawnPosition = new Vector3(posX, posY, 0f);

        for (int i = 0; i < MonsterCount; i++)
        {
            GameObject bossMonster = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);
            bossMonster.name = $"Boss_Monster_{i + 1}";

            SpriteRenderer spriteRenderer = bossMonster.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }

            Debug.Log("보스 몬스터를 스폰했습니다.");

            if (MonsterCount > 1)
            {
                Debug.Log("보스 몬스터는 1회만 스폰 가능합니다.");
                return;
            }
        }
    }
}

public class MiddleMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        // 타일 정보가 없으면 종료
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("타일 정보가 없습니다. 미들 몬스터를 스폰할 수 없습니다.");
            return;
        }

        // 타일 인포의 행과 열을 가져옵니다.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // 몬스터 카운터가 있는지 확인
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("미들 몬스터 카운터가 0 이하입니다. 스폰할 미들 몬스터가 없습니다.");
            return;
        }

        // 몬스터 스폰
        for (int i = 0; i < MonsterCount; i++)
        {
            // 미들 몬스터를 스폰합니다.
            Debug.Log("미들 몬스터를 스폰했습니다.");
        }
    }
}

public class NormalMonsterSpawner : MonsterSpawner
{
    public override void SpawnMonster(GameObject prefab, Transform spawnParent)
    {
        // 타일 정보가 없으면 종료
        if (tileInfos == null || tileInfos.Count == 0)
        {
            Debug.LogWarning("타일 정보가 없습니다. 노말 몬스터를 스폰할 수 없습니다.");
            return;
        }

        // 타일 인포의 행과 열을 가져옵니다.
        int maxRow = tileInfos.Max(t => t.row);
        int maxColumn = tileInfos.Max(t => t.column);

        // 몬스터 카운터가 있는지 확인
        if (MonsterCount <= 0)
        {
            Debug.LogWarning("노말 몬스터 카운터가 0 이하입니다. 스폰할 노말 몬스터가 없습니다.");
            return;
        }

        // 몬스터 스폰
        for (int i = 0; i < MonsterCount; i++)
        {
            // 노말 몬스터를 스폰합니다.
            Debug.Log("노말 몬스터를 스폰했습니다.");
        }
    }
}