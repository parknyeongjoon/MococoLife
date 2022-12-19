using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialize : MonoBehaviour
{
    GameManager gameManager;
    Pooler pooler;

    [SerializeField] List<ItemData> itemList;
    [SerializeField] List<GameObject> initPrefabs;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        for (int i = 0; i < itemList.Count; i++)
        {
            gameManager.itemDic.Add(itemList[i].code, itemList[i]);
        }

        yield return new WaitUntil(() => Pooler.Instance != null);
        pooler = Pooler.Instance;
        pooler.SetPhotonPool(initPrefabs);

        SceneManager.LoadScene("MainTitle");
    }
}
