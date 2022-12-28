using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Sword", menuName = "SO/Item/Sword")]
public class Sword : ItemData
{
    [SerializeField] float dmg;

    public override void Effect(PlayerInfo info, Vector3 effectPos)
    {
        info.StartCoroutine(IEEffect(info, effectPos));
    }

    public IEnumerator IEEffect(PlayerInfo info, Vector3 effectPos)
    {
        if(info.canAtkTime > 0)//공격 가능 시간이라면
        {
            info.State = P_State.Action;
            info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "atk");

            yield return new WaitForSeconds(delay);
            detectTarget(info ,effectPos);
            info.State = P_State.Idle;
        }
    }

    void detectTarget(PlayerInfo info, Vector3 effectPos)//보스가 있다면 우선으로 때리고 플레이어는 밀치기가 가능하게
    {
        Collider2D target = Physics2D.OverlapPoint(effectPos, 1 << LayerMask.NameToLayer("Boss"));
        if(target != null)//보스가 있다면 때리고 리턴
        {
            target.GetComponent<IDamagable>().Damage(dmg);
            Debug.Log("보스 때림");
            return;
        }
        target = Physics2D.OverlapPoint(effectPos, 1 << LayerMask.NameToLayer("Player"));//보스를 못 때렸다면 유저탐색 유저끼리 팀킬 가능
        if(target != null)
        {
            target.GetComponent<ICC>().KnockBack(Vector3.Normalize(target.transform.position - info.transform.position), 0.05f);//넉백
            Debug.Log("유저 때림");
            return;
        }
    }
}
