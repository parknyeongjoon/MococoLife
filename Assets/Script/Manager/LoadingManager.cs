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

        gameManager.SetMyPlayerNum(PhotonNetwork.PlayerList.Length - 1);//��ġ�� �ʰ� üũ�ϴ� �� �߰����ֱ�

        photonView.RPC("UpdatePlayerText", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
    }

    private void Update()
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

    public void ReadyBtn()
    {
        photonView.RPC("Ready", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
    }

    [PunRPC]
    public void Ready(int index)
    {
        if (isReady[index] == true)//���� Ǯ ��
        {
            isReady[index] = false;
            readyImg[index].color = Color.red;
        }
        else//���� �� ��
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
