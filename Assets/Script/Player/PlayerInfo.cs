using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun, IDamagable, IPunObservable, ICC
{
    GameManager gameManager;
    public PUI myUI;
    Pooler pooler;

    [SerializeField] float hp = 100;
    [SerializeField] Collider2D bodyCollider, triggerCollider;

    public float canAtkTime;

    P_State state;
    [SerializeField] Slot hand = new Slot();
    [SerializeField] Slot[] inventory = new Slot[4];

    public P_State State { get => state; set => state = value; }
    public Slot Hand { get => hand; }
    public float Hp { get => hp; }
    public Slot[] Inventory { get => inventory; set => inventory = value; }

    #region rpcVar

    public GameObject LAreaMoveIcon, RAreaMoveIcon, WarningIcon;
    GameObject grave;
    public SpriteRenderer handImg;

    #endregion

    void Awake()
    {
        gameManager = GameManager.Instance;
        pooler = Pooler.Instance;

        if (photonView.IsMine)
        {
            photonView.RPC("SetPlayers", RpcTarget.AllBufferedViaServer, gameManager.MyPlayerNum);
        }

        hand.itemData = gameManager.itemDic["T_00"];

        for (int i = 0; i < 4; i++)
        {
            inventory[i] = new Slot();
        }
    }

    [PunRPC]
    void SetPlayers(int index)
    {
        gameManager.players[index] = this;
    }

    [PunRPC]
    public void SetHand(int index, string code, int count)
    {
        Hand.itemData = gameManager.itemDic[code];
        Hand.itemCount = count;
        handImg.sprite = gameManager.itemDic[code].itemImg;
    }
    #region interface

    public void Damage(float dmg)
    {
        if (state != P_State.TimePause && state != P_State.Dead)
        {
            hp -= dmg;
            if (hp <= 0 && state != P_State.Dead)
            {
                photonView.RPC("DeadEvent", RpcTarget.AllViaServer);
                photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "death");
                StartCoroutine(GetGrave());
            }
            else
            {
                photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "hit");
            }
        }
    }

    public void Heal(float heal)
    {
        if (state != P_State.Dead)
        {
            hp += heal;
            if (hp > 100)
            {
                hp = 100;
            }
        }
    }

    public void KnockBack(Vector3 knockDir, float dis)
    {
        photonView.RPC("RPCKnockBack", RpcTarget.AllViaServer, knockDir, dis);
    }

    [PunRPC]
    void RPCKnockBack(Vector3 knockDir, float dis)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(KnockBackTimer(knockDir, dis));
        }
    }

    IEnumerator KnockBackTimer(Vector3 knockDir, float dis)
    {
        float time = 0;
        state = P_State.Stun;

        while (time < 0.1f)
        {
            time += Time.deltaTime;
            transform.position += knockDir * dis;
            yield return null;
        }

        state = P_State.Idle;
    }

    public void Stun(float stunTime)
    {
        photonView.RPC("RPCStun", RpcTarget.AllViaServer, stunTime);
    }

    [PunRPC]
    void RPCStun(float stunTime)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(StunTimer(stunTime));
        }
    }

    IEnumerator StunTimer(float stunTime)
    {
        float time = 0;
        state = P_State.Stun;

        while (time < stunTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        state = P_State.Idle;
    }

    #endregion

    [PunRPC]
    void DeadEvent()///UnityEvent?
    {
        state = P_State.Dead;

        myUI.SetGraveIcon(true);

        bodyCollider.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                if (gameManager.players[i] != null && gameManager.players[i].state != P_State.Dead) { return; }
            }

            gameManager.photonView.RPC("GameOver", RpcTarget.AllViaServer);
        }
    }

    IEnumerator GetGrave()
    {
        int randIndex = Random.Range(0, 5);
        Vector3 desPos = transform.position;
        grave = pooler.Get("Grave_" + randIndex, desPos + new Vector3(0, 10, 0));
        while (grave.transform.position.y >= desPos.y)
        {
            grave.transform.position -= new Vector3(0, Time.deltaTime * 5);
            yield return null;
        }
    }

    public void Resurrection(Vector3 spawnPos)
    {
        photonView.RPC("ResurrectionEvent", RpcTarget.AllViaServer, spawnPos);
        photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "resurrection");
    }

    [PunRPC]
    void ResurrectionEvent(Vector3 spawnPos)///UnityEvent?
    {
        hp = 20.0f;

        myUI.SetGraveIcon(false);

        bodyCollider.gameObject.SetActive(true);
        triggerCollider.gameObject.SetActive(true);

        transform.position = spawnPos;

        state = P_State.Idle;
    }

    [PunRPC]
    void SetInventoryIcon(int index, string code, int count)
    {
        myUI.InventoryUpdate(index, code, count);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//정보 보내기
        {
            stream.SendNext(hp);
            stream.SendNext(state);
        }
        else//정보 받기
        {
            hp = (float)stream.ReceiveNext();
            state = (P_State)stream.ReceiveNext();
        }
    }
}
