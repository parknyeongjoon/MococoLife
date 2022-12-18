using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class BossBehaviour : MonoBehaviourPun
{
    [SerializeField] BossInfo info;
    [SerializeField] Pooler paternPooler;

    List<string> randPattern;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)//������ Ŭ���̾�Ʈ������ ������ �ϰ� �ٸ� ���� rpc�� �ѱ��
        {
            StartCoroutine(StartBoss());
        }
    }

    #region Pattern

    IEnumerator StartBoss()
    {
        while (true)
        {
            StartCoroutine(StarfishBomb());
            yield return new WaitForSeconds(7f);
        }
    }

    [PunRPC]
    void Think()
    {

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

    IEnumerator StarfishBomb()
    {
        float randX, randY;
        for(int i = 0; i < 5; i++)
        {
            randX = Random.Range(5.0f, 10.0f);
            randY = Random.Range(3.0f, 11.0f);
            paternPooler.Get("StarfishBomb", new Vector3(randX, randY));
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }

    #endregion
}
