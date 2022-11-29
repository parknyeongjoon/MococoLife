using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMove : MonoBehaviourPun//ī�޶� �̵����ִ� �Լ�
{
    [SerializeField] float cameraSpeed;
    bool[] isCameraMove = new bool[4];

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                photonView.RPC("SetCameraMove", RpcTarget.MasterClient, GameManager.Instance.MyPlayerNum, true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                photonView.RPC("SetCameraMove", RpcTarget.MasterClient, GameManager.Instance.MyPlayerNum, false);
            }
        }
    }

    [PunRPC]
    void SetCameraMove(int index, bool isMove)
    {
        isCameraMove[index] = isMove;

        if (isMove)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (isCameraMove[i])
                {
                    count++;
                    if (count >= PhotonNetwork.CurrentRoom.PlayerCount)
                    {
                        StartCoroutine(MoveCamera());
                    }
                }
            }
        }
    }

    IEnumerator MoveCamera()//�÷��̾� ��ΰ� ���� ���� �����ʿ� ���� �� �� �̵�
    {
        Vector3 cameraSpeedV = new Vector3(cameraSpeed, 0, 0);
        float destiny = transform.position.x + 30;

        while (transform.position.x <= destiny)
        {
            transform.position += cameraSpeedV * Time.deltaTime;
            yield return null;
        }
    }
}
