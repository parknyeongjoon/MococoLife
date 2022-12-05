using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "SO/Item/Food")]
public class Food : BattleItemData
{
    public override void Effect(Vector3 effectPos)
    {
        PlayerInfo info = GameManager.Instance.players[GameManager.Instance.MyPlayerNum];
    }
}
