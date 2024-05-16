using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheckController : MonoBehaviour
{
    void CheckTileInfo(string objectName)
    {
        // 오브젝트 이름과 같은 게임 오브젝트를 찾습니다.
        GameObject tileObject = GameObject.Find(objectName);

        if (tileObject != null)
        {
            // 타일 오브젝트에 TileCheck 스크립트가 있는지 확인합니다.
            TileCheck tileCheck = tileObject.GetComponent<TileCheck>();

            if (tileCheck != null)
            {
                // TileCheck 스크립트가 있다면 해당 스크립트의 변수를 출력합니다.
                Debug.Log("타일 오브젝트 '" + objectName + "'에 대한 정보:");

                Debug.Log("MeleeAtk: " + TileCheck.MeleeAtk);
                Debug.Log("RangeAtk: " + TileCheck.RangeAtk);
                Debug.Log("Player: " + TileCheck.Player);
                Debug.Log("Enemy: " + TileCheck.Enemy);
            }
            else
            {
                Debug.LogWarning("타일 오브젝트 '" + objectName + "'에 TileCheck 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("이름이 '" + objectName + "'인 오브젝트를 찾을 수 없습니다.");
        }
    }

    // 타일 정보 가져오기
    void TileInfoCheck()
    {
        TileInfoGenerator tileInfoGenerator = FindObjectOfType<TileInfoGenerator>();

        if (tileInfoGenerator != null)
        {
            // 타일 정보 리스트를 가져옵니다.
            List<TileInfoGenerator.TileInfo> tilePositions = tileInfoGenerator.GetTilePositions();

            if (tilePositions != null)
            {
                int count = tilePositions.Count;

                Debug.Log("타일 정보를 성공적으로 가져왔습니다 - TileCheckController");
                Debug.Log("가져온 인덱스 개수: " + count);

                // 랜덤한 행과 열을 선택
                int randomIndex = Random.Range(0, count);
                int randomRow = tilePositions[randomIndex].row;
                int randomColumn = tilePositions[randomIndex].column;

                // 선택한 행과 열을 가진 타일의 이름 확인
                string objectName = GetObjectName(tilePositions, randomRow, randomColumn);
                Debug.Log("랜덤 선택된 행과 열: " + randomRow + "/" + randomColumn);
                Debug.Log("선택된 행과 열을 가진 타일의 이름: " + objectName);

                // 랜덤한 값에 따라 변수를 변경합니다.
                int randomValue = Random.Range(1, 5);
                if (randomValue == 1)
                {
                    TileCheck.MeleeAtk = true;
                    Debug.Log("MeleeAtk를 True로 설정했습니다.");
                }
                else if (randomValue == 2)
                {
                    TileCheck.RangeAtk = true;
                    Debug.Log("RangeAtk를 True로 설정했습니다.");
                }
                else if (randomValue == 3)
                {
                    TileCheck.Player = true;
                    Debug.Log("Player를 True로 설정했습니다.");
                }
                else if (randomValue == 4)
                {
                    TileCheck.Enemy = true;
                    Debug.Log("Enemy를 True로 설정했습니다.");
                }
            }
            else
            {
                Debug.LogError("타일 정보 리스트를 가져올 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("TileInfoGenerator 스크립트를 가진 게임 오브젝트를 찾을 수 없습니다.");
        }
    }

    // 주어진 행과 열을 가진 타일의 이름을 반환하는 함수
    string GetObjectName(List<TileInfoGenerator.TileInfo> tilePositions, int row, int column)
    {
        foreach (TileInfoGenerator.TileInfo tileInfo in tilePositions)
        {
            if (tileInfo.row == row && tileInfo.column == column)
            {
                return tileInfo.name;
            }
        }
        return "해당하는 타일이 없습니다.";
    }

    void Start()
    {
        Invoke("TileInfoCheck", 0.1f);
        CheckTileInfo("Tile_0_0"); // 예시로 타일 이름 전달
    }
}