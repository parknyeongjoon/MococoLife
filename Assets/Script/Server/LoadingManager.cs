using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoadingManager : MonoBehaviourPun
{
    GameManager gameManager;

    private float timeCount = 0f;

    [SerializeField] TMP_Text roomName, playerText, loadingText;
    [SerializeField] GameObject loadingObject;

    bool[] isReady = new bool[4];
    [SerializeField] Image[] readyImg = new Image[4];

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.SetMyPlayerNum(PhotonNetwork.PlayerList.Length - 1);//겹치지 않게 체크하는 거 추가해주기

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        photonView.RPC("UpdatePlayerText", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum, true);
    }

    private void Update()
    {
        if (timeCount <= 1.5f) timeCount += Time.deltaTime;
        if (timeCount <= 0.5f)
        {
            loadingText.text = "Waiting for players.";
        }
        else if (timeCount <= 1f)
        {
            loadingText.text = "Waiting for players..";
        }
        else if (timeCount <= 1.5f)
        {
            loadingText.text = "Waiting for players...";
        }
        else
        {
            timeCount = 0f;
        }
        loadingObject.transform.eulerAngles += new Vector3(0f, 0f, 0.1f);
    }

    public void ReadyBtn()
    {
        photonView.RPC("Ready", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
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
    void UpdatePlayerText(int index, bool isOnline)
    {
        playerText.text = PhotonNetwork.PlayerList.Length.ToString() + " / 4";
        if (isOnline) { readyImg[index].color = Color.red; }
        else { readyImg[index].color = Color.white; }
    }

    public void QuitRoom()
    {
        photonView.RPC("UpdatePlayerText", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum, false);
        isReady[gameManager.MyPlayerNum] = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainTitle");
    }
}
