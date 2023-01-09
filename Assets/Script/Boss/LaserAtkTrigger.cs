using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAtkTrigger : MonoBehaviour
{
    PlayerInfo target;

    [SerializeField] float atkDelay;
    [SerializeField] float dmg;

    [SerializeField] BoxCollider2D trigger;
    [SerializeField] Transform SquareTimer;

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
        if (target != null && collision.gameObject == target.gameObject)
        {
            target.WarningIcon.SetActive(false);
            target = null;
        }
    }

    void OnEnable()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float timer = 0;
   
        while (timer < atkDelay)
        {
            timer += Time.deltaTime;
            SquareTimer.localScale = new Vector3(trigger.size.x, trigger.size.y*timer/atkDelay);
            yield return null;
        }
        if (target != null)
        {
            target.Damage(dmg);
        }
        gameObject.SetActive(false);
    }

    [PunRPC]
    void ActiveObject(bool isActive, Vector3 pos)
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(isActive);
    }
}
