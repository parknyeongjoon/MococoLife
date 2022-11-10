using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleItemData", menuName = "SO/Item/BattleItemData")]
public class BattleItemData : ItemData
{
    public int needWood;
    public int needStone;
    public float needTime;

    public override void Effect(Vector2Int effectPos)
    {
        
    }
}
