using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
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

    [SerializeField] PUI[] PUIs;

    [SerializeField] GameObject BHP_GO;
    [SerializeField] Image BHP_FG;

    [SerializeField] GameObject GameClearPanel, GameOverPanel;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);//���� ���� �Լ� ������ֱ�
        gameManager = GameManager.Instance;

        for (int i = 0; i < 4; i++)
        {
            if (gameManager.players[i] != null)
            {
                PUIs[i].gameObject.SetActive(true);
                gameManager.players[i].myUI = PUIs[i];
            }
        }

        gameManager.boss = GameObject.Find("Boss").GetComponent<BossInfo>();///�ӽ� - GameManager���� photonnetwork.instantiate�� �� �Ҵ��ϱ�
        StartCoroutine(UpdateBHPBar());
    }

    IEnumerator UpdateBHPBar()
    {
        while (true)
        {
            BHP_FG.fillAmount = gameManager.boss.Hp / 200;//��ġ ���� �� ��ũ��Ʈ�� ����;
            yield return null;
        }
    }

    public void SetClearPanel(bool isOpen)
    {
        GameClearPanel.SetActive(isOpen);
    }

    public void SetOverPanel(bool isOpen)
    {
        GameOverPanel.SetActive(isOpen);
    }

    public void RetryBtn()
    {
        PhotonNetwork.LoadLevel("Loading");
    }

    public void QuitBtn()
    {
        PhotonNetwork.LeaveRoom();
        
        Debug.Log("�泪����");
    }

    #region network

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom");
        PhotonNetwork.LoadLevel("MainTitle");
    }

    #endregion
}
