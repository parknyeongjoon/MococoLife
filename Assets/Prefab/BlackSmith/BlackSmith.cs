using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class BlackSmith : MonoBehaviourPun
{
    PhotonView PV;
    GameManager gameManager;

    [SerializeField] GameObject InteracitveIcon;
    [Header("CreatePanel")]
    [SerializeField] GameObject createPanel;
    [Header("NeedPanel")]
    [SerializeField] GameObject needPanel;
    [SerializeField] TMP_Text needWoodText, needStoneText;
    [Header("CreatingPanel")]
    [SerializeField] GameObject creatingPanel;
    [SerializeField] Image creatingItemImg, creatingP;

    Create_State create_State = Create_State.Idle;
    BattleItemData creatingItem;
    int curWood, curStone;

    PlayerInfo targetInfo;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInfo info = collision.GetComponent<PlayerInfo>();
            if (info.photonView.IsMine)
            {
                InteracitveIcon.SetActive(true);
                targetInfo = info;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (targetInfo != null && collision.gameObject == targetInfo.gameObject)
        {
            InteracitveIcon.SetActive(false);
            targetInfo = null;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && targetInfo != null)
        {
            if(create_State == Create_State.Create && targetInfo.Hand.itemData.Item_Type == Item_Type.Ingredient)
            {
                int result = PlusIngredient(targetInfo.Hand.itemData.code, targetInfo.Hand.itemCount);
                Debug.Log(result);
                if (targetInfo.Hand.itemCount - result <= 0)
                {
                    targetInfo.photonView.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, "T_00", 0);
                }
                else
                {
                    targetInfo.photonView.RPC("SetHand", RpcTarget.AllViaServer, gameManager.MyPlayerNum, targetInfo.Hand.itemData.code, targetInfo.Hand.itemCount - result);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && targetInfo != null)
        {
            if (create_State == Create_State.Idle) { SetCreatePanel(); }//idle 상태면 createPanel 열기
            else if (create_State == Create_State.Finish)//제작이 완료됐다면 우클릭한 캐릭터에게 주기
            {
                GetItem(targetInfo);
            }
        }
    }

    void Start()
    {
        PV = photonView;
        gameManager = GameManager.Instance;

        createPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        needPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        creatingPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void SetCreatePanel()
    {
        createPanel.SetActive(true);
    }

    public void RPCSetNeedPanel(ItemData itemData)///지금은 인스펙터 창에서 하는데 itemData 넣어서 가능하게 하기
    {
        if (create_State == Create_State.Idle)
        {
            PV.RPC("SetNeedPanel", RpcTarget.AllViaServer, itemData.code);
        }
    }

    [PunRPC] void SetNeedPanel(string code)
    {
        create_State = Create_State.Create;
        CloseCreatePanel();
        creatingItem = (BattleItemData)gameManager.itemDic[code];
        curWood = 0; curStone = 0;
        needWoodText.text = curWood.ToString() + " / " + creatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + creatingItem.needStone.ToString();
        needPanel.SetActive(true);
    }

    public int PlusIngredient(string code, int count)
    {
        int result = 0;
        if (code == "I_00")//나무
        {
            if (curWood + count >= creatingItem.needWood)
            {
                result = creatingItem.needWood - curWood;
                curWood = creatingItem.needWood;
            }
            else { curWood += count; result = count; }
        }
        else if (code == "I_01")//돌
        {
            if (curStone + count >= creatingItem.needStone)
            {
                result = creatingItem.needStone - curStone;
                curStone = creatingItem.needStone;
            }
            else { curStone += count; result = count; }
        }
        photonView.RPC("RPCPlusIngredient", RpcTarget.AllViaServer, curWood, curStone);
        return result;
    }

    [PunRPC] void RPCPlusIngredient(int _wood, int _stone)
    {
        curWood = _wood; curStone = _stone;

        needWoodText.text = curWood.ToString() + " / " + creatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + creatingItem.needStone.ToString();

        if (curWood >= creatingItem.needWood && curStone >= creatingItem.needStone)
        {
            PV.RPC("SetCreatingPanel", RpcTarget.AllViaServer);
        }
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
    }

    [PunRPC] void SetCreatingPanel()
    {
        needPanel.SetActive(false);
        creatingItemImg.sprite = creatingItem.itemImg;
        creatingPanel.SetActive(true);
        StartCoroutine(CreatingTimer());
    }

    IEnumerator CreatingTimer()
    {
        float cur_Time = 0;
        creatingP.color = Color.red;

        while (cur_Time <= creatingItem.needTime)
        {
            cur_Time += Time.deltaTime;
            creatingP.fillAmount = cur_Time / creatingItem.needTime;
            yield return null;
        }

        creatingP.color = Color.green;
        create_State = Create_State.Finish;
    }

    public void GetItem(PlayerInfo targetInfo)//������� true �� ������� false
    {
        if(creatingItem.code == "BI_00")//음식이라면 바로 먹기
        {
            creatingItem.Effect(targetInfo);
            photonView.RPC("PickUp", RpcTarget.AllViaServer);//�������� ����ٸ� PickUp����
            return;
        }

        for (int i = 0; i < targetInfo.Inventory.Length; i++)
        {
            if (targetInfo.Inventory[i].itemData == creatingItem)//�κ��丮�� ��ġ�� �������� �ִٸ� üũ�ϱ�
            {
                if (targetInfo.Inventory[i].itemCount >= creatingItem.maxCount)
                {
                    Debug.Log("더 이상 가질 수 없음.");
                    return;
                }
                else
                {
                    targetInfo.Inventory[i].itemCount++;
                    photonView.RPC("PickUp", RpcTarget.AllViaServer);//�������� ����ٸ� PickUp����
                    return;
                }
            }
        }
        for (int i = 0; i < targetInfo.Inventory.Length; i++)//�κ��丮�� ��ġ�� �������� ���ٸ� �� ĭ üũ�ϱ�
        {
            if (targetInfo.Inventory[i].itemData == null)
            {
                targetInfo.Inventory[i].itemData = creatingItem;
                targetInfo.Inventory[i].itemCount = 1;
                photonView.RPC("PickUp", RpcTarget.AllViaServer);//�������� ����ٸ� PickUp����
                return;
            }
        }
        Debug.Log("자리가 없음");
        return;
    }

    [PunRPC] void PickUp()
    {
        create_State = Create_State.Idle;
        creatingItem = null;
        creatingPanel.SetActive(false);
    }
}