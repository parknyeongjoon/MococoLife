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
    private bool isLoading;

    public Text playerText;
    public Text loadingText;
    public GameObject loadingObject;

    private void Start()
    {
        isLoading = false;
        timeCount = 0f;
        playerCount = 1;
        playerText.text = "1 / 4";
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length < 1)
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
        else
        {
            LoadScene();
        }

        if (PhotonNetwork.PlayerList.Length != playerCount)
        {
            playerCount = PhotonNetwork.PlayerList.Length;
            playerText.text = playerCount.ToString() + " / 4";
        }
    }

    private void LoadScene()
    {
        if (!isLoading)
        {
            isLoading = true;
            PhotonNetwork.LoadLevel("BossTest");
        }
    }
}
