using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Pickaxe", menuName = "SO/Item/Pickaxe")]
public class Pickaxe : ItemData
{
    public override IEnumerator Effect(PlayerInfo info, Vector3 effectPos)
    {
        Collider2D[] targets = Physics2D.OverlapPointAll(effectPos, 1 << LayerMask.NameToLayer("Terrain"));
        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Rock"))
            {
                info.State = State.Action;
                info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "pickaxe");
                Debug.Log("Ã¤±¤ Áß..");

                yield return new WaitForSeconds(delay);
                target.GetComponent<IDamagable>().Damage(Dmg_Type.Pickaxe, 1);
                info.State = State.Idle;
                break;
            }
        }
    }
}
