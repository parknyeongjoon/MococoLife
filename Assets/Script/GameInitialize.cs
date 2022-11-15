using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialize : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] List<ItemData> itemList;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        for (int i = 0; i < itemList.Count; i++)
        {
            gameManager.itemDic.Add(itemList[i].code, itemList[i]);
        }

        SceneManager.LoadScene("MainTitle");
    }
}
