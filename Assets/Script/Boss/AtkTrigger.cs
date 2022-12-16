using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AtkTrigger : MonoBehaviour
{
    PlayerInfo target;

    [SerializeField] float atkDelay;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInfo info = collision.GetComponent<PlayerInfo>();
            if (info.photonView.IsMine)
            {
                target = info;
                target.WarningIcon.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(target != null && collision.gameObject == target.gameObject)
        {
            target.WarningIcon.SetActive(false);
            target = null;
        }
    }
}
