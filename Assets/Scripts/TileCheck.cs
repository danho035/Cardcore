using UnityEngine;

public class TileCheck : MonoBehaviour
{
    public static bool MeleeAtk;
    public static bool RangeAtk;
    public static bool Player;
    public static bool Enemy;

    void Start()
    {
        // √ ±‚»≠
        MeleeAtk = false;
        RangeAtk = false;
        Player = false;
        Enemy = false;
    }
}