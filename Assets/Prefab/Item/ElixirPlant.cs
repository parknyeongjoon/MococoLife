using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElixirPlant", menuName = "SO/Item/ElixirPlant")]
public class ElixirPlant : BattleItemData
{
    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        info.Resurrection(effectPos);
    }
}
