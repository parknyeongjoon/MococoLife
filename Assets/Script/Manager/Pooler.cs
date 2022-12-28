using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// ������ Photon.Instantiate�� ������ �׳� Instantiate�� �߰��ϱ�
/// </summary>
public class Pooler : MonoBehaviourPun
{
    static Pooler instance;
    public static Pooler Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetPhotonPool(List<GameObject> updateObjects)
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && updateObjects != null)
        {
            foreach (GameObject prefab in updateObjects)
            {
                pool.ResourceCache.Add(prefab.name, prefab);

                if (!pools.ContainsKey(prefab.name))
                {
                    pools.Add(prefab.name, new List<GameObject>());
                    pools[prefab.name].Add(prefab);
                }
            }
        }
    }

    public GameObject Get(string name, Vector3 spawnPos)
    {
        GameObject select = null;

        int pS = pools[name].Count;

        for (int i = 0; i < pS; i++)
        {
            if (pools[name][i].activeSelf == false)//������Ʈ�� Ȱ��ȭ ���°� �ƴ϶��
            {
                select = pools[name][i];
                select.GetComponent<PhotonView>().RPC("ActiveObject", RpcTarget.All, true, spawnPos);
                return select;
            }
        }

        if (select == null)//��� ������Ʈ�� Ȱ��ȭ�Ǿ��ִٸ� ���� ����
        {
            select = PhotonNetwork.Instantiate(pools[name][0].name, spawnPos, Quaternion.identity);
            pools[name].Add(select);
        }

        return select;
    }
}
