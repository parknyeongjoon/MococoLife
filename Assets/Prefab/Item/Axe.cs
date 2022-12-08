using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Axe", menuName = "SO/Item/Axe")]
public class Axe : ItemData
{
    public override IEnumerator Effect(PlayerInfo info, Vector3 effectPos)
    {
        Collider2D[] targets = Physics2D.OverlapPointAll(effectPos, 1 << LayerMask.NameToLayer("Terrain"));
        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Tree"))
            {
                info.State = State.Action;
                info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "axe");
                Debug.Log("¹ú¸ñ Áß..");

                yield return new WaitForSeconds(delay);
                target.GetComponent<IDamagable>().Damage(Dmg_Type.Axe, 1);
                info.State = State.Idle;
                break;
            }
        }
    }
}
