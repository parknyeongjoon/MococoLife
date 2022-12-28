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

    public void Damage(float dmg)
    {
        if(state != P_State.TimePause && state != P_State.Dead)
        {
            hp -= dmg;
            if (hp <= 0 && state != P_State.Dead)
            {
                StartCoroutine(Die());
            }
        }
    }

    public void Heal(float heal)
    {
        if(state != P_State.Dead)
        {
            hp += heal;
            if(hp > 100)
            {
                hp = 100;
            }
        }
    }

    public void KnockBack(Vector3 knockDir, float dis)
    {
        StartCoroutine(KnockBackTimer(knockDir, dis));
    }

    public void Stun(float stunTime)
    {
        StartCoroutine(StunTimer(stunTime));
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

    IEnumerator Die()
    {
        state = P_State.Dead;
        photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "death");
        photonView.RPC("SetGraveIcon", RpcTarget.AllViaServer, true);

        int randIndex = Random.Range(0, 5);
        Vector3 desPos = transform.position;
        grave = pooler.Get("Grave_" + randIndex, desPos + new Vector3(0, 10, 0));///������ Ŭ���̾�Ʈ�� ���� ������ ����ȭ�� �� ��
        while (grave.transform.position.y >= desPos.y)
        {
            grave.transform.position -= new Vector3(0, Time.deltaTime * 5);
            yield return null;
        }
    }

    public void Resurrection(Vector3 spawnPos)
    {
        hp = 20.0f;
        transform.position = spawnPos;
        state = P_State.Idle;
        photonView.RPC("AnimTrigger", RpcTarget.AllViaServer, "resurrection");
        photonView.RPC("SetGraveIcon", RpcTarget.AllViaServer, false);
    }

    [PunRPC]
    void SetGraveIcon(bool isActive)
    {
        myUI.SetGraveIcon(isActive);
    }

    [PunRPC]
    void SetInventoryIcon(int index, string code, int count)
    {
        myUI.InventoryUpdate(index, code, count);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//���� ������
        {
            stream.SendNext(hp);
        }
        else//���� �ޱ�
        {
            hp = (float)stream.ReceiveNext();
        }
    }
}
