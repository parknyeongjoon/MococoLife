using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Sword", menuName = "SO/Item/Sword")]
public class Sword : ItemData
{
    public override IEnumerator Effect(PlayerInfo info, Vector3 effectPos)
    {
        if(info.canAtkTime > 0)//공격 가능 시간이라면
        {
            info.State = State.Action;
            info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "atk");

            yield return new WaitForSeconds(delay);
            info.State = State.Idle;
        }
    }
}
