using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pooler : MonoBehaviourPun
{
    [SerializeField] GameObject[] prefabList;
    Dictionary<string, List<GameObject>> pools;

    void Start()
    {
        if(photonView == null)
        {
            gameObject.AddComponent<PhotonView>();
        }

        SetPhotonPool();

        pools = new Dictionary<string, List<GameObject>>();

        int pS = prefabList.Length;
        for (int i = 0; i < pS; i++)
        {
            pools.Add(prefabList[i].name, new List<GameObject>());
            pools[prefabList[i].name].Add(prefabList[i]);
        }
    }

    void SetPhotonPool()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && prefabList != null)
        {
            foreach (GameObject prefab in prefabList)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
    }

    public GameObject Get(string name, Vector3 spawnPos)
    {
        GameObject select = null;

        int pS = pools[name].Count;

        for (int i = 0; i < pS; i++)
        {
            if (pools[name][i].activeSelf == false)//오브젝트가 활성화 상태가 아니라면
            {
                select = pools[name][i];
                select.GetComponent<PhotonView>().RPC("ActiveObject", RpcTarget.All, true, spawnPos);
                return select;
            }
        }

        if (select == null)//모든 오브젝트가 활성화되어있다면 새로 생성
        {
            select = PhotonNetwork.Instantiate(pools[name][0].name, spawnPos, Quaternion.identity);
            pools[name].Add(select);
        }

        return select;
    }
}
