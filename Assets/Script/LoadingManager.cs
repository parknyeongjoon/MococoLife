using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoadingManager : MonoBehaviourPun
{
    GameManager gameManager;

    private float timeCount = 0f;

    public Text playerText;
    public Text loadingText;
    public GameObject loadingObject;

    bool[] isReady = new bool[4];
    [SerializeField] Image[] readyImg = new Image[4];

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.myPlayerNum = PhotonNetwork.PlayerList.Length - 1;//겹치지 않게 체크하는 거 추가해주기

        photonView.RPC("UpdatePlayerText", RpcTarget.AllBufferedViaServer, gameManager.myPlayerNum);
    }

    private void Update()
    {
        if (timeCount <= 1.5f) timeCount += Time.deltaTime;
        if (timeCount <= 0.5f)
        {
            loadingText.text = "플레이어를 기다리는중.";
        }
        else if (timeCount <= 1f)
        {
            loadingText.text = "플레이어를 기다리는중..";
        }
        else if (timeCount <= 1.5f)
        {
            loadingText.text = "플레이어를 기다리는중...";
        }
        else
        {
            timeCount = 0f;
        }
        loadingObject.transform.eulerAngles += new Vector3(0f, 0f, 0.1f);
    }

    public void ReadyBtn()
    {
        photonView.RPC("Ready", RpcTarget.AllBufferedViaServer, gameManager.myPlayerNum);
    }

    [PunRPC]
    public void Ready(int index)
    {
        if (isReady[index] == true)//레디를 풀 때
        {
            isReady[index] = false;
            readyImg[index].color = Color.red;
        }
        else//레디를 할 때
        {
            isReady[index] = true;
            readyImg[index].color = Color.green;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (isReady[i] == false) break;
                if (i == PhotonNetwork.PlayerList.Length - 1)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.LoadLevel("Stage");
                }
            }
        }
    }

    [PunRPC]
    void UpdatePlayerText(int index)
    {
        playerText.text = PhotonNetwork.PlayerList.Length.ToString() + " / 4";
        readyImg[index].color = Color.red;
    }
}
