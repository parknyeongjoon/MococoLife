using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
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

    GameManager gameManager;

    [SerializeField] GameObject[] PHP_GO;
    [SerializeField] Image[] PHP_FG;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager = GameManager.Instance;

        for (int i = 0; i < 4; i++)
        {
            if (gameManager.players[i] != null)
            {
                PHP_GO[i].SetActive(true);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (gameManager.players[i] != null)
            {
                StartCoroutine(UpdateHPBar(i));
            }
        }
    }

    IEnumerator UpdateHPBar(int index)
    {
        while (true)
        {
            PHP_FG[index].fillAmount = gameManager.players[index].Hp / 100;
            yield return null;
        }
    }
}
