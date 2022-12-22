using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "SO/Item/Potion")]
public class Potion : BattleItemData
{
    [SerializeField] float heal;

    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        info.Heal(heal);
    }
}
