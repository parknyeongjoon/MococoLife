using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class BossBehaviour : MonoBehaviourPun
{
    [SerializeField] BossInfo info;
    [SerializeField] List<GameObject> patternList;
    Pooler pooler;
    AreaMove areaMove;

    void Start()
    {
        pooler = Pooler.Instance;
        pooler.SetPhotonPool(patternList);

        if (PhotonNetwork.IsMasterClient)//마스터 클라이언트에서만 실행을 하고 다른 곳에 rpc로 넘기기
        {
            areaMove = Camera.main.GetComponent<AreaMove>();

            StartCoroutine(StartBoss());
        }
    }

    #region Pattern

    IEnumerator StartBoss()
    {
        while (true)
        {
            Think();
            yield return new WaitForSeconds(5f);
        }
    }

    [PunRPC]
    void Think()
    {
        if(info.State == Boss_State.Idle && !areaMove.IsMove)//보스가 공격을 할 수 있고 카메라가 이동 중이 아니라면
        {
            int randIndex = Random.Range(0, patternList.Count);
            StartCoroutine(patternList[randIndex].name);
        }
    }

    IEnumerator AttackAround()
    {
        yield return null;
        pooler.Get("AttackAround", transform.position);
    }

    IEnumerator StarfishBomb()
    {
        float randX, randY;
        for(int i = 0; i < 20; i++)
        {
            randX = Random.Range(3.5f, 30f);
            randY = Random.Range(-4.0f, 4.0f);
            pooler.Get("StarfishBomb", new Vector3(transform.position.x + randX, transform.position.y + randY));
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }

    #endregion
}
