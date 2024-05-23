using System.Collections.Generic;
using UnityEngine;

public class AttackTileSelect : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator;

    List<Vector2Int> attackableTiles = new List<Vector2Int>(); // attackableTiles 리스트 선언

    List<int> melee_1 = new List<int> { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 2, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
    List<int> melee_2 = new List<int> { 0, 1, 0, 1, 2, 1, 0, 1, 0 };
    List<int> range_1 = new List<int> { 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 0, 2, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0 };
    List<int> range_2 = new List<int> { 1, 0, 1, 1, 2, 1, 1, 0, 1 };

    List<List<int>> atkRange = new List<List<int>>();

    public int unitRow;
    public int unitColumn;

    public int meleeTargetTileCount;
    public int rangeTargetTileCount;

    public float duration;

    public int meleeChance;
    public int rangeChance;

    public int atkCount;

    void Start()
    {
        ConvertToAtkRange(range_2, atkRange);

        // Debug.Log를 이용하여 atkRange 리스트 출력
        foreach (var sublist in atkRange)
        {
            string sublistString = string.Join(", ", sublist);
            Debug.Log("Sublist: " + sublistString);
        }

        // 예시로 (0, 0) 위치에 공격 시전 위치가 있다고 가정
        Vector2Int attackStartPosition = new Vector2Int(unitColumn, unitRow);
        // 공격 시전 위치를 기준으로 공격 가능한 타일의 좌표 계산
        CalculateAttackableTiles(attackStartPosition);
    }

    void ConvertToAtkRange(List<int> originalList, List<List<int>> atkRange)
    {
        int count = originalList.Count;

        List<int> sublist = new List<int>();
        int sublistSize = 0;

        if (count == 25)
        {
            sublistSize = 5;
        }
        else if (count == 16)
        {
            sublistSize = 4;
        }
        else if (count == 9)
        {
            sublistSize = 3;
        }
        else
        {
            Debug.LogError("지정된 공격 스킬이 어떤 타일 타입에도 부합하지 않습니다. 딕셔너리를 확인해주세요");
            return; // 유효하지 않은 경우 이후 실행 중지
        }

        for (int i = 0; i < count; i++)
        {
            sublist.Add(originalList[i]);

            if (sublist.Count == sublistSize)
            {
                atkRange.Add(new List<int>(sublist)); // sublist를 초기화한 후 추가
                sublist.Clear(); // sublist 초기화
            }
        }
    }

    void CalculateAttackableTiles(Vector2Int startPosition)
    {
        int rows = atkRange.Count;
        int columns = atkRange[0].Count;
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (atkRange[i][j] == 1) // 1은 공격 가능한 타일을 나타냄
                {
                    // 공격 시전 위치에서의 상대적인 좌표 계산
                    int relativeRow = i - centerRow;
                    int relativeColumn = j - centerColumn;
                    // 실제 좌표 계산
                    int targetRow = startPosition.x + relativeRow;
                    int targetColumn = startPosition.y + relativeColumn;
                    Vector2Int targetPosition = new Vector2Int(targetRow, targetColumn);
                    // 결과 출력
                    Debug.Log("공격 가능한 타일 위치: " + targetPosition);
                    // 공격 가능한 타일을 리스트에 추가
                    attackableTiles.Add(targetPosition);
                }
            }
        }
    }

    void TileTargeting()
    {
        rangeChance = rangeChance + meleeChance;

        int atkTypeSelect = Random.Range(1, (meleeChance + rangeChance + 1));
        if (atkTypeSelect >= meleeChance)
        {
            // 근접 공격 실행 매커니즘
        }
        else
        {
            // 원거리 공격 실행 매커니즘
        }
    }

    void Update()
    {
        // 매 프레임마다 실행되는 로직
    }
}