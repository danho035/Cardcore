using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTileSelect : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator;

    List<Vector2Int> attackableTiles = new List<Vector2Int>();

    List<int> melee_1 = new List<int> { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 2, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
    List<int> melee_2 = new List<int> { 0, 1, 0, 1, 2, 1, 0, 1, 0 };
    List<int> range_1 = new List<int> { 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 0, 2, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0 };
    List<int> range_2 = new List<int> { 1, 0, 1, 1, 2, 1, 1, 0, 1 };

    List<List<int>> atkRange = new List<List<int>>();

    public int unitRow;
    public int unitColumn;

    public int rowCheck = 100;
    public int columnCheck = 100;

    public int meleeTargetTileCount;
    public int rangeTargetTileCount;

    public float duration = 3f;

    public int meleeChance;
    public int rangeChance;

    public int atkCount = 1;

    void ConvertToAtkRange(List<int> originalList, List<List<int>> atkRange)
    {
        atkRange.Clear();
        int count = originalList.Count;
        int sublistSize = 0;

        if (count == 25)
        {
            sublistSize = 5;
        }
        else if (count == 9)
        {
            sublistSize = 3;
        }
        else
        {
            Debug.LogError("지정된 공격 스킬이 어떤 타일 타입에도 부합하지 않습니다. 딕셔너리를 확인해주세요");
            return;
        }

        List<int> sublist = new List<int>();
        for (int i = 0; i < count; i++)
        {
            sublist.Add(originalList[i]);

            if (sublist.Count == sublistSize)
            {
                atkRange.Add(new List<int>(sublist));
                sublist.Clear();
            }
        }
    }

    void CalculateAttackableTiles(Vector2Int startPosition)
    {
        attackableTiles.Clear();
        int rows = atkRange.Count;
        int columns = atkRange[0].Count;
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (atkRange[i][j] == 1)
                {
                    int relativeRow = i - centerRow;
                    int relativeColumn = j - centerColumn;
                    int targetRow = startPosition.x + relativeRow;
                    int targetColumn = startPosition.y + relativeColumn;
                    Vector2Int targetPosition = new Vector2Int(targetRow, targetColumn);
                    attackableTiles.Add(targetPosition);
                }
            }
        }
    }

    bool IsTileValid(Vector2Int tile)
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("tileInfoGenerator가 설정되지 않았습니다.");
            return false;
        }

        if (!tileInfoGenerator.IsTilePresent(tile)) return false;

        GameObject tileObject = GameObject.Find("tile_" + tile.x + tile.y);
        if (tileObject == null) return false;

        TileCheck tileCheck = tileObject.GetComponent<TileCheck>();
        if (tileCheck == null) return false;

        if (tileCheck.Enemy || tileCheck.MeleeAtk || tileCheck.RangeAtk) return false;

        return true;
    }

    List<Vector2Int> SelectMeleeTiles(List<Vector2Int> tiles, int count)
    {
        List<Vector2Int> selectedTiles = new List<Vector2Int>();

        foreach (Vector2Int tile in tiles)
        {
            if (selectedTiles.Count >= count) break;

            bool isAdjacent = selectedTiles.Count == 0 || selectedTiles.Exists(t =>
                (t.x == tile.x && Mathf.Abs(t.y - tile.y) == 1) ||
                (t.y == tile.y && Mathf.Abs(t.x - tile.x) == 1));

            if (isAdjacent && IsTileValid(tile))
            {
                selectedTiles.Add(tile);
            }
        }

        return selectedTiles;
    }

    List<Vector2Int> SelectRangeTiles(List<Vector2Int> tiles, int count)
    {
        List<Vector2Int> selectedTiles = new List<Vector2Int>();

        foreach (Vector2Int tile in tiles)
        {
            if (selectedTiles.Count >= count) break;

            if (IsTileValid(tile))
            {
                selectedTiles.Add(tile);
            }
        }

        return selectedTiles;
    }

    void ApplyAttackToTiles(List<Vector2Int> tiles, bool isMelee)
    {
        foreach (Vector2Int tile in tiles)
        {
            GameObject tileObject = GameObject.Find("tile_" + tile.x + tile.y);
            if (tileObject != null)
            {
                TileCheck tileCheck = tileObject.GetComponent<TileCheck>();
                if (tileCheck != null)
                {
                    if (isMelee)
                    {
                        tileCheck.MeleeAtk = true;
                    }
                    else
                    {
                        tileCheck.RangeAtk = true;
                    }

                    tileObject.GetComponent<Image>().color = Color.red; // 타일 이미지 색상을 빨간색으로 변경
                }
            }
        }

        StartCoroutine(ResetTileAfterDuration(tiles));
    }

    IEnumerator ResetTileAfterDuration(List<Vector2Int> tiles)
    {
        yield return new WaitForSeconds(duration);

        foreach (Vector2Int tile in tiles)
        {
            GameObject tileObject = GameObject.Find("tile_" + tile.x + tile.y);
            if (tileObject != null)
            {
                TileCheck tileCheck = tileObject.GetComponent<TileCheck>();
                if (tileCheck != null)
                {
                    tileCheck.MeleeAtk = false;
                    tileCheck.RangeAtk = false;

                    tileObject.GetComponent<Image>().color = Color.white; // 타일 이미지 색상을 하얀색으로 변경
                }
            }
        }
    }

    void TileTargeting()
    {
        rangeChance = rangeChance + meleeChance;

        int atkTypeSelect = Random.Range(1, (meleeChance + rangeChance + 1));
        if (atkTypeSelect <= meleeChance)
        {
            int meleeRan = Random.Range(1, 3);

            if (meleeRan == 1)
            {
                ConvertToAtkRange(melee_1, atkRange);
            }
            else if (meleeRan == 2)
            {
                ConvertToAtkRange(melee_2, atkRange);
            }

            Vector2Int attackStartPosition = new Vector2Int(unitColumn, unitRow);
            CalculateAttackableTiles(attackStartPosition);
            List<Vector2Int> selectedTiles = SelectMeleeTiles(attackableTiles, atkCount);

            if (selectedTiles.Count > 0)
            {
                ApplyAttackToTiles(selectedTiles, true);
            }
        }
        else
        {
            int rangeRan = Random.Range(1, 3);

            if (rangeRan == 1)
            {
                ConvertToAtkRange(range_1, atkRange);
            }
            else if (rangeRan == 2)
            {
                ConvertToAtkRange(range_2, atkRange);
            }

            Vector2Int attackStartPosition = new Vector2Int(unitColumn, unitRow);
            CalculateAttackableTiles(attackStartPosition);
            List<Vector2Int> selectedTiles = SelectRangeTiles(attackableTiles, atkCount);

            if (selectedTiles.Count > 0)
            {
                ApplyAttackToTiles(selectedTiles, false);
            }
        }
    }

    void Start()
    {
        StartCoroutine(UpdateTileTargeting());
    }

    IEnumerator UpdateTileTargeting()
    {
        while (true)
        {
            TileTargeting();
            yield return new WaitForSeconds(duration);
        }
    }

    void Update()
    {

    }
}