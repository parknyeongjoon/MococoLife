using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class BossBehaviour : MonoBehaviourPun
{
    [SerializeField] BossInfo info;

    List<string> randPattern;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)//마스터 클라이언트에서만 실행을 하고 다른 곳에 rpc로 넘기기
        {
            StartCoroutine(StartBoss());
        }
    }

    #region Pattern

    IEnumerator StartBoss()
    {
        while (true)
        {
            //photonView.RPC("AttackAround", RpcTarget.AllViaServer);
            yield return new WaitForSeconds(2.0f);
        }
    }

    [PunRPC]
    void AttackAround()
    {
        Debug.Log("AttackAround");
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));
        foreach (Collider2D target in targets)
        {
            if (target.GetComponent<PhotonView>().IsMine)
            {
                target.GetComponent<IDamagable>().Damage(10);
            }
        }
    }

    #endregion
}
