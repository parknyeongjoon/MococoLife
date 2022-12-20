using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    UnityEvent InventoryEvent;

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

    [SerializeField] PUI[] PUIs;

    [SerializeField] GameObject BHP_GO;
    [SerializeField] Image BHP_FG;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);//게임 시작 함수 만들어주기
        gameManager = GameManager.Instance;

        for (int i = 0; i < 4; i++)
        {
            if (gameManager.players[i] != null)
            {
                PUIs[i].gameObject.SetActive(true);
                gameManager.players[i].myUI = PUIs[i];
            }
        }

        gameManager.boss = GameObject.Find("Boss").GetComponent<BossInfo>();//바꾸기 - GameManager에서 photonnetwork.instantiate할 때 할당하기
        StartCoroutine(UpdateBHPBar());
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
