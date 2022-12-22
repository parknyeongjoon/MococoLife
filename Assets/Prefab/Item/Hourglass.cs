using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hourglass", menuName = "SO/Item/Hourglass")]
public class Hourglass : BattleItemData
{
    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        info.StartCoroutine(IEEffect(info));
    }

    public IEnumerator IEEffect (PlayerInfo info)
    {
        info.State = P_State.TimePause;
        yield return new WaitForSeconds(delay);
        info.State = P_State.Idle;
    }
}
