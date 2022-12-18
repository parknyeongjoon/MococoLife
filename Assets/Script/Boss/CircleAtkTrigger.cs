using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 겹쳐있을 때 다른 게 먼저 터지면 아직 안에 있어도 경고 표시 사라짐
/// </summary>
public class CircleAtkTrigger : MonoBehaviour
{
    PlayerInfo target;

    [SerializeField] float atkDelay;
    [SerializeField] float dmg;

    [SerializeField] CircleCollider2D trigger;
    [SerializeField] Transform circle, circleTimer;

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

    void OnEnable()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        Vector3 scaleV = new Vector3(trigger.radius * 2, trigger.radius * 2);
        float timer = 0;
        while (timer < atkDelay)
        {
            timer += Time.deltaTime;
            circleTimer.localScale = scaleV * timer / atkDelay;
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
