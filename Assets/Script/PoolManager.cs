using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    static PoolManager instance;
    public static PoolManager Instance
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

    [SerializeField] ItemData[] prefabDatas;
    Dictionary<string, List<GameObject>> pools;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pools = new Dictionary<string, List<GameObject>>();

        int pS = prefabDatas.Length;
        for(int i = 0; i < pS; i++)
        {
            pools.Add(prefabDatas[i].code, new List<GameObject>());
            //pools[prefabDatas[i].code].Add(prefabDatas[i].prefab);
        }
    }

    public GameObject Get(string code, Vector3 spawnPos)
    {
        GameObject select = null;

        int pS = pools[code].Count;

        for(int i = 0; i < pS; i++)
        {
            if (pools[code][i].activeSelf == false)//������Ʈ�� Ȱ��ȭ ���°� �ƴ϶��
            {
                select = pools[code][i];
                select.transform.position = spawnPos;
                select.SetActive(true);
                break;
            }
        }

        if(select == null)//��� ������Ʈ�� Ȱ��ȭ�Ǿ��ִٸ� ���� ����
        {
            select = Instantiate(pools[code][0]);
            select.transform.position = spawnPos;
            pools[code].Add(select);
        }

        return select;
    }
}
