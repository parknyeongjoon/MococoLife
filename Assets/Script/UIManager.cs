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
    [SerializeField] GameObject BHP_GO;
    [SerializeField] Image BHP_FG;

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
                StartCoroutine(UpdatePHPBar(i));
            }
        }

        gameManager.boss = GameObject.Find("Boss").GetComponent<BossInfo>();//바꾸기 - GameManager에서 photonnetwork.instantiate할 때 할당하기
        StartCoroutine(UpdateBHPBar());
    }

    IEnumerator UpdatePHPBar(int index)
    {
        while (true)
        {
            PHP_FG[index].fillAmount = gameManager.players[index].Hp / 100;
            yield return null;
        }
    }

    IEnumerator UpdateBHPBar()
    {
        while (true)
        {
            BHP_FG.fillAmount = gameManager.boss.Hp / 200;//수치 변경 및 스크립트에 저장;
            yield return null;
        }
    }
}
