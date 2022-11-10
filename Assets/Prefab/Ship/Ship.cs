using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Ship : MonoBehaviourPun
{
    PhotonView PV;

    //지우기
    [SerializeField] List<ItemData> itemList;//gameManager로 옮기기
    Dictionary<string, ItemData> itemDic = new();//gameManager로 옮기기

    public void RPCtestIngredientPlus()
    {
        if (PV.IsMine)
        {
            PV.RPC("testIngredientPlus", RpcTarget.AllViaServer);
        }
    }
    [PunRPC]
    void testIngredientPlus()
    {
        curWood++;
        curStone++;
        needWoodText.text = curWood.ToString() + " / " + ((BattleItemData)creatingItem).needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + ((BattleItemData)creatingItem).needStone.ToString();
        if (curWood >= ((BattleItemData)creatingItem).needWood && curStone >= ((BattleItemData)creatingItem).needStone)
        {
            PV.RPC("SetCreatingPanel", RpcTarget.AllViaServer);
        }
    }

    void OnMouseDown()
    {
        if (create_State == Create_State.idle)
        {
            createPanel.SetActive(true);
        }
        else if (create_State == Create_State.finish)
        {
            RPCPickUp();
        }
    }
    //지우기

    [Header("CreatePanel")]
    [SerializeField] GameObject createPanel;
    [Header("NeedPanel")]
    [SerializeField] GameObject needPanel;
    [SerializeField] TMP_Text needWoodText, needStoneText;
    [Header("CreatingPanel")]
    [SerializeField] GameObject creatingPanel;
    [SerializeField] Image creatingItemImg, creatingP;

    enum Create_State { idle, create, finish };
    Create_State create_State = Create_State.idle;
    ItemData creatingItem;
    int curWood, curStone;

    void Start()
    {
        PV = photonView;
        createPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        needPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        creatingPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));

        //지우기
        for (int i = 0; i < itemList.Count; i++)
        {
            itemDic.Add(itemList[i].code, itemList[i]);
        }
        //지우기
    }

    public void RPCSetNeedPanel(string code)
    {
        if (create_State == Create_State.idle)
        {
            PV.RPC("SetNeedPanel", RpcTarget.AllViaServer, code);
        }
    }

    public void RPCPickUp()
    {
        PV.RPC("PickUp", RpcTarget.AllViaServer);
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
    }

    #region PunRPC
    [PunRPC]
    void SetNeedPanel(string code)
    {
        create_State = Create_State.create;
        CloseCreatePanel();
        creatingItem = itemDic[code];
        curWood = 0; curStone = 0;
        needWoodText.text = curWood.ToString() + " / " + ((BattleItemData)creatingItem).needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + ((BattleItemData)creatingItem).needStone.ToString();
        needPanel.SetActive(true);
    }

    [PunRPC]
    void SetCreatingPanel()
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

        while (cur_Time <= ((BattleItemData)creatingItem).needTime)
        {
            cur_Time += Time.deltaTime;
            creatingP.fillAmount = cur_Time / ((BattleItemData)creatingItem).needTime;
            yield return null;
        }

        creatingP.color = Color.green;
        create_State = Create_State.finish;
    }

    [PunRPC]
    void PickUp()
    {
        create_State = Create_State.idle;
        creatingItem = null;
        creatingPanel.SetActive(false);
    }
    #endregion
}
