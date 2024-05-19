using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UnitSpawner : MonoBehaviour
{
    public TileInfoGenerator tileInfoGenerator; // 타일 정보를 제공하는 TileInfoGenerator 스크립트를 참조

    // 타일 정보 리스트
    private List<TileInfoGenerator.TileInfo> tileInfos; // 타일 정보 리스트
    List<Vector2> selectedIndex = new List<Vector2>(); // 지정한 타일 정보 리스트 인덱스와 몬스터 타입 저장 리스트

    // 몬스터 카운트
    public int Monster11Count;
    public int Monster21Count;
    public int Monster31Count;

    // 함수용 변수
    private int highestRow;
    private int highestColumn;

    // 몬스터 프리팹
    public GameObject monster31Prefab; // 몬스터 유형 31의 프리팹
    public GameObject monster21Prefab; // 몬스터 유형 21의 프리팹
    public GameObject monster11Prefab; // 몬스터 유형 11의 프리팹

    void Start()
    {
        Debug.Log(highestRow);
        Debug.Log(highestColumn);

        Invoke("GetTileInfosDelayed", 0.1f);

    }

    void GetTileInfosDelayed()
    {
        if (tileInfoGenerator == null)
        {
            Debug.LogError("TileInfoGenerator가 설정되지 않았습니다.");
            return;
        }

        // 타일 정보 리스트 받아오기
        tileInfos = tileInfoGenerator.GetTileInfos();

        // 몬스터 스폰
        MonsterSpawn();
    }

    // 가장 높은 행/열 지정
    private int GetHighestRow(List<TileInfoGenerator.TileInfo> tileInfos)
    {
        int highestRow = int.MinValue;
        foreach (var tileInfo in tileInfos)
        {
            if (tileInfo.row > highestRow)
            {
                highestRow = tileInfo.row;
            }
        }
        return highestRow;
    }
    private int GetHighestColumn(List<TileInfoGenerator.TileInfo> tileInfos) // 가장 높은 열
    {
        int highestColumn = int.MinValue;
        foreach (var tileInfo in tileInfos)
        {
            if (tileInfo.column > highestColumn)
            {
                highestColumn = tileInfo.column;
            }
        }
        return highestColumn;
    }

    // 몬스터 함수
    private void Type31Tile() // 31 타입 몬스터 스폰 위치 지정
    {
        // 31타입 몬스터의 조건 확인
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow + 1; // 행 개수 계산

        if (rowCount != 5)
        {
            Debug.LogWarning("열이 5가 아니어서 31타입 몬스터를 생성할 수 없습니다.");
            return;
        }

        // 31 타입 몬스터의 스폰 위치를 지정
        for (int i = 0; i < Monster31Count; i++)
        {
            if (tileInfos.Exists(tile => tile.row == 0 && (tile.column == 2 || tile.column == 3 || tile.column == 4)))
            {
                List<int> selectedColumns = new List<int> { 2, 3, 4 };
                string logMessage = "31타입 몬스터 스폰 위치: ";
                foreach (int column in selectedColumns)
                {
                    int tileIndex = tileInfos.FindIndex(tile => tile.row == 0 && tile.column == column);
                    selectedIndex.Add(new Vector2Int(tileIndex, 31));
                    logMessage += $"(0, {column}), ";
                }
                // 마지막 쉼표 제거
                logMessage = logMessage.TrimEnd(',', ' ');
                Debug.Log(logMessage);
            }

            // 중복 스폰 체크
            if (Monster31Count > 1)
            {
                Debug.LogWarning("보스 몬스터(31타입)의 개수가 2 이상으로 지정되어 있습니다.");
                return;
            }
        }
    }

    private void Type21Tile() // 21 타입 몬스터 스폰 위치 지정
    {
        // 21타입 몬스터의 조건 확인
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow + 1; // 행 개수 계산

        if (rowCount != 4)
        {
            Debug.LogWarning("열이 4가 아니어서 21타입 몬스터를 생성할 수 없습니다.");
            return;
        }

        // 21 타입 몬스터의 스폰 위치를 지정
        for (int i = 0; i < Monster21Count; i++)
        {
            if (tileInfos.Exists(tile => tile.row == 0 && tile.column == 2) && tileInfos.Exists(tile => tile.row == 0 && tile.column == 3))
            {
                List<(int, int)> selectedPositions = new List<(int, int)> { (0, 2), (0, 3) };
                string logMessage = "21타입 몬스터 스폰 위치: ";
                foreach (var position in selectedPositions)
                {
                    int tileIndex = tileInfos.FindIndex(tile => tile.row == position.Item1 && tile.column == position.Item2);
                    selectedIndex.Add(new Vector2Int(tileIndex, 21));
                    logMessage += $"({position.Item1}, {position.Item2}), ";
                }
                // 마지막 쉼표 제거
                logMessage = logMessage.TrimEnd(',', ' ');
                Debug.Log(logMessage);

            }

            // 중복 스폰 체크
            if (Monster21Count > 1)
            {
                Debug.LogWarning("보스 몬스터(21타입)의 개수가 2 이상으로 지정되어 있습니다.");
                return;
            }
        }
    }

    private void Type11Tile() // 11 타입 몬스터 스폰 위치 지정
    {
        int highestRow = GetHighestRow(tileInfos);
        int rowCount = highestRow;
        int columnCount = tileInfos.Max(tile => tile.column) + 1;

        System.Random random = new System.Random();

        for (int i = 0; i < Monster11Count; i++)
        {
            int randomRow, randomColumn;
            int tileIndex;

            if (Monster21Count > 0 && Monster31Count > 0)
            {
                do
                {
                    randomRow = random.Next(1, rowCount); // 1부터 rowCount-1까지 랜덤
                    randomColumn = random.Next(0, columnCount); // 0부터 columnCount-1까지 랜덤

                    tileIndex = tileInfos.FindIndex(tile => tile.row == randomRow && tile.column == randomColumn);

                } while (tileIndex == -1 || selectedIndex.Any(index => index.x == tileIndex)); // 유효한 타일인지, 이미 선택된 타일인지 확인
            }
            else
            {
                do
                {
                    randomRow = random.Next(1, rowCount); // 0부터 rowCount-1까지 랜덤
                    randomColumn = random.Next(0, columnCount); // 0부터 columnCount-1까지 랜덤

                    tileIndex = tileInfos.FindIndex(tile => tile.row == randomRow && tile.column == randomColumn);

                } while (tileIndex == -1 || selectedIndex.Any(index => index.x == tileIndex)); // 유효한 타일인지, 이미 선택된 타일인지 확인
            }

            selectedIndex.Add(new Vector2Int(tileIndex, 11)); // 선택된 인덱스와 몬스터 타입 추가
            Debug.Log($"11타입 몬스터 스폰 위치: ({randomRow}, {randomColumn})");
        }
    }

    private void MonsterSpawning()
    {
        // 31 타입 몬스터 스폰
        if (Monster31Count > 0)
        {
            SpawnMonster(monster31Prefab, 3, 0, "31_Monster_", "MonsterName");
            selectedIndex.RemoveAll(v => v.y == 31);
        }

        // 21 타입 몬스터 스폰
        if (Monster21Count > 0)
        {
            SpawnMonster(monster21Prefab, 2, 1, "21_Monster_", "MonsterName");
            selectedIndex.RemoveAll(v => v.y == 21);
        }

        // 11 타입 몬스터 스폰
        if (Monster11Count > 0)
        {
            for (int i = 0; i < Monster11Count; i++)
            {
                if (selectedIndex.Count > 0)
                {
                    Vector2 element = selectedIndex[0];
                    int x1 = (int)element.x;
                    SpawnMonster(monster11Prefab, 1, x1, "11_Monster_", "MonsterName" + i);
                    selectedIndex.RemoveAt(0);
                }
            }
        }
    }

    private void SpawnMonster(GameObject prefab, int tileCount, int tileIndex, string monsterType, string monsterName)
    {
        // 몬스터 오브젝트 생성
        GameObject newObject = Instantiate(prefab, transform);
        newObject.name = monsterType + monsterName;

        // 몬스터 인스펙터 변경
        float width = tileInfos[0].width * tileCount + tileInfos[0].spacing * (tileCount - 1);
        float height = tileInfos[0].height;
        float posX = 0f;
        float posY = 0f;

        for (int i = tileIndex; i < tileIndex + tileCount; i++)
        {
            posX += tileInfos[i].posX;
            posY += tileInfos[i].posY;
        }
        posX /= tileCount;
        posY /= tileCount;

        // 몬스터 크기 설정
        RectTransform rectTransform = newObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(posX, posY); // 위치 설정
        }
        else
        {
            Debug.LogError("프리팹에 RectTransform이 없습니다.");
            return;
        }

        // 이미지 컴포넌트의 최대 투명도 설정
        var image = newObject.GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f; // 최대 투명도 설정
            image.color = color;
        }
        else
        {
            Debug.LogError("이미지 컴포넌트를 찾을 수 없습니다.");
            return;
        }
    }


    // 스폰 체크
    private bool TileChecking(List<Vector2Int> selectedIndex)
    {
        bool allTilesPassed = true;

        foreach (var indexInfo in selectedIndex)
        {
            int index = indexInfo.x;
            int monsterType = indexInfo.y;

            string tileName = tileInfos[index].tileName;

            GameObject[] foundObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == tileName).ToArray();

            // 해당 이름을 가진 오브젝트가 있는지 확인
            if (foundObjects.Length == 0)
            {
                Debug.LogError($"태그 '{tileName}'을 가진 오브젝트를 찾을 수 없습니다.");
                allTilesPassed = false;
                continue;
            }

            // 해당 이름을 가진 오브젝트에 타일 타입이 있는지 체크
            bool tileTypeExists = false;
            foreach (var obj in foundObjects)
            {
                TileCheck tileCheck = obj.GetComponent<TileCheck>();

                if (tileCheck != null)
                {
                    tileTypeExists = true;
                    break;
                }
            }

            // 타일 타입이 없는 경우
            if (!tileTypeExists)
            {
                Debug.LogError($"오브젝트에 타일 타입이 없습니다: '{tileName}'");
                allTilesPassed = false;
                continue;
            }

            // 타일 타입에 있는 bool 변수가 전부 false인지 확인
            bool allTileTypeBoolsFalse = true;
            foreach (var obj in foundObjects)
            {
                TileCheck tileCheck = obj.GetComponent<TileCheck>();

                if (tileCheck != null && (TileCheck.MeleeAtk || TileCheck.RangeAtk || TileCheck.Player || TileCheck.Enemy))
                {
                    allTileTypeBoolsFalse = false;
                    break;
                }
            }

            // 조건 충족에 실패한 경우
            if (!allTileTypeBoolsFalse)
            {
                Debug.LogWarning($"타일 '{tileName}'은 조건을 충족하지 않습니다.");
                allTilesPassed = false;
            }
        }

        return allTilesPassed;
    }


    // 몬스터 스폰 함수
    public void MonsterSpawn()
    {
        // 변수 초기화
        int type11Count = Monster11Count;
        int type21Count = Monster21Count;
        int type31Count = Monster31Count;

        if (tileInfos == null || tileInfos.Count == 0 || selectedIndex == null) // 타일 정보 리스트 또는 selectedIndex가 없을 경우 종료
        {
            Debug.LogError("타일 정보나 selectedIndex가 없습니다.");
            return;
        }
        else
        {
            selectedIndex.Clear(); // 리스트 초기화
            int highestRow = GetHighestRow(tileInfos); // 가장 높은 행 구하기
            int highestColumn = GetHighestColumn(tileInfos); // 가장 높은 행 구하기

            Debug.Log("현재 타일 그리드" + (highestRow + 1) + (highestColumn + 1));

            //타입별 스폰 위치 지정
            Type31Tile();
            Type21Tile();
            Type11Tile();

            // 지정한 위치 타일 체크
            bool allTilesPassed = TileChecking(selectedIndex.ConvertAll(v => new Vector2Int((int)v.x, (int)v.y)));

            Debug.Log("selectedIndex 리스트:");
            foreach (var index in selectedIndex)
            {
                Debug.Log($"인덱스: ({index.x}, {index.y})");
            }

            // 모든 타일이 통과되었는지 확인하고 처리
            if (allTilesPassed)
            {
                Debug.Log("모든 타일 체크 완료!");
                MonsterSpawning();
            }
            else
            {
                // 여기에 모든 타일이 통과되지 않은 경우의 로직을 추가합니다.
            }
        }
    }
}
