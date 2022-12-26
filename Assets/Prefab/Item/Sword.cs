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
        if(info.canAtkTime > 0)//���� ���� �ð��̶��
        {
            info.State = P_State.Action;
            info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "atk");

            yield return new WaitForSeconds(delay);
            detectTarget(info ,effectPos);
            info.State = P_State.Idle;
        }
    }

    void detectTarget(PlayerInfo info, Vector3 effectPos)//������ �ִٸ� �켱���� ������ �÷��̾�� ��ġ�Ⱑ �����ϰ�
    {
        Collider2D target = Physics2D.OverlapPoint(effectPos, 1 << LayerMask.NameToLayer("Boss"));
        if(target != null)//������ �ִٸ� ������ ����
        {
            target.GetComponent<IDamagable>().Damage(dmg);
            Debug.Log("���� ����");
            return;
        }
        target = Physics2D.OverlapPoint(effectPos, 1 << LayerMask.NameToLayer("Player"));//������ �� ���ȴٸ� ����Ž�� �������� ��ų ����
        if(target != null)
        {
            target.GetComponent<ICC>().KnockBack(Vector3.Normalize(target.transform.position - info.transform.position), 0.05f);//�˹�
            Debug.Log("���� ����");
            return;
        }
    }
}
