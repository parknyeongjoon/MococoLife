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

    void Start()
    {
        gameManager = GameManager.Instance;

        /* �ε� ������ �غ� ������ Ÿ�̸Ӷ� ���� �����ϱ�
        for(int i = 0; i < 4; i++)
        {
            if(gameManager.players[i] != null)
            {
                PHP_GO[i].SetActive(true);
            }
        }
        */
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameManager.players[i] != null)
            {
                PHP_FG[i].fillAmount = gameManager.players[i].Hp / 100;
            }
        }
    }
}
