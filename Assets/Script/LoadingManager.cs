using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoadingManager : MonoBehaviourPun
{
    private float timeCount;
    private int playerCount;

    public Text playerText;
    public Text loadingText;
    public GameObject loadingObject;

    private void Start()
    {
        timeCount = 0f;
        playerCount = 1;
        playerText.text = "1 / 4";
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length < 4)
        {
            if (timeCount <= 1.5f) timeCount += Time.deltaTime;
            if (timeCount <= 0.5f)
            {
                loadingText.text = "�÷��̾ ��ٸ�����.";
            }
            else if (timeCount <= 1f)
            {
                loadingText.text = "�÷��̾ ��ٸ�����..";
            }
            else if (timeCount <= 1.5f)
            {
                loadingText.text = "�÷��̾ ��ٸ�����...";
            }
            else
            {
                timeCount = 0f;
            }
            loadingObject.transform.eulerAngles += new Vector3(0f, 0f, 0.1f);
        }
        else
        {
            PhotonNetwork.LoadLevel("Stage");
        }

        if (PhotonNetwork.PlayerList.Length != playerCount)
        {
            playerCount = PhotonNetwork.PlayerList.Length;
            playerText.text = playerCount.ToString() + " / 4";
        }
    }
}