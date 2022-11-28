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

    [Header("CreatePanel")]
    [SerializeField] GameObject createPanel;
    [Header("NeedPanel")]
    [SerializeField] GameObject needPanel;
    [SerializeField] TMP_Text needWoodText, needStoneText;
    [Header("CreatingPanel")]
    [SerializeField] GameObject creatingPanel;
    [SerializeField] Image creatingItemImg, creatingP;

    Create_State create_State = Create_State.idle;
    BattleItemData creatingItem;
    int curWood, curStone;

    public Create_State Create_State { get => create_State; }
    public BattleItemData CreatingItem { get => creatingItem; }

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

    public void RPCSetNeedPanel(string code)///지금은 인스펙터 창에서 하는데 itemData 넣어서 가능하게 하기
    {
        if (Create_State == Create_State.idle)
        {
            PV.RPC("SetNeedPanel", RpcTarget.AllViaServer, code);
        }
    }

    [PunRPC]
    void SetNeedPanel(string code)
    {
        create_State = Create_State.create;
        CloseCreatePanel();
        creatingItem = (BattleItemData)gameManager.itemDic[code];
        curWood = 0; curStone = 0;
        needWoodText.text = curWood.ToString() + " / " + CreatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + CreatingItem.needStone.ToString();
        needPanel.SetActive(true);
    }

    public int PlusIngredient(string code, int count)
    {
        int result = 0;
        if (code == "I_00")//나무
        {
            if (curWood + count >= CreatingItem.needWood)
            {
                result = CreatingItem.needWood - curWood;
                curWood = CreatingItem.needWood;
            }
            else { curWood += count; result = count; }
        }
        else if (code == "I_01")//돌
        {
            if (curStone + count >= CreatingItem.needStone)
            {
                result = CreatingItem.needStone - curStone;
                curStone = CreatingItem.needStone;
            }
            else { curStone += count; result = count; }
        }
        photonView.RPC("RPCPlusIngredient", RpcTarget.AllViaServer, curWood, curStone);
        return result;
    }

    [PunRPC]
    void RPCPlusIngredient(int _wood, int _stone)
    {
        curWood = _wood; curStone = _stone;

        needWoodText.text = curWood.ToString() + " / " + CreatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + CreatingItem.needStone.ToString();

        if (curWood >= CreatingItem.needWood && curStone >= CreatingItem.needStone)
        {
            PV.RPC("SetCreatingPanel", RpcTarget.AllViaServer);
        }
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
    }

    [PunRPC]
    void SetCreatingPanel()
    {
        needPanel.SetActive(false);
        creatingItemImg.sprite = CreatingItem.itemImg;
        creatingPanel.SetActive(true);
        StartCoroutine(CreatingTimer());
    }

    IEnumerator CreatingTimer()
    {
        float cur_Time = 0;
        creatingP.color = Color.red;

        while (cur_Time <= CreatingItem.needTime)
        {
            cur_Time += Time.deltaTime;
            creatingP.fillAmount = cur_Time / CreatingItem.needTime;
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
}