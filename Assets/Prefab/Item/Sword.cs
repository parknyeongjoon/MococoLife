using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Sword", menuName = "SO/Item/Sword")]
public class Sword : ItemData
{
    [SerializeField] float dmg;
    public override IEnumerator Effect(PlayerInfo info, Vector3 effectPos)
    {
        if(info.canAtkTime > 0)//���� ���� �ð��̶��
        {
            info.State = P_State.Action;
            info.photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "atk");

            yield return new WaitForSeconds(delay);
            detectTarget(effectPos);
            info.State = P_State.Idle;
        }
    }

    void detectTarget(Vector3 effectPos)//������ �ִٸ� �켱���� ������ �÷��̾�� ��ġ�Ⱑ �����ϰ�
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
            Debug.Log(target.name);
            target.GetComponent<IDamagable>().Damage(0);///�˹� ��� �߰�
            Debug.Log("���� ����");
            return;
        }
    }
}
