using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMoveTrigger : MonoBehaviour
{
    [SerializeField] CameraMove cameraMove;
    [SerializeField] bool isRight;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                cameraMove.photonView.RPC("SetCameraMove", RpcTarget.MasterClient, GameManager.Instance.MyPlayerNum, true, isRight);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                cameraMove.photonView.RPC("SetCameraMove", RpcTarget.MasterClient, GameManager.Instance.MyPlayerNum, false, isRight);
            }
        }
    }
}
