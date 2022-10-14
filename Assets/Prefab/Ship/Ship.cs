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
        needWoodText.text = curWood.ToString() + " / " + creatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + creatingItem.needStone.ToString();
        if (curWood >= creatingItem.needWood && curStone >= creatingItem.needStone)
        {
            PV.RPC("SetCreatingPanel", RpcTarget.AllViaServer);
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

    bool isCreate = false;
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

    void OnMouseDown()
    {
        if (isCreate == false)
        {
            createPanel.SetActive(true);
        }
    }

    public void RPCSetNeedPanel(string code)
    {
        if (isCreate == false)
        {
            PV.RPC("SetNeedPanel", RpcTarget.AllViaServer, code);
        }
    }

    public void CloseCreatePanel()
    {
        createPanel.SetActive(false);
    }

    [PunRPC]
    void SetNeedPanel(string code)
    {
        isCreate = true;
        CloseCreatePanel();
        creatingItem = itemDic[code];
        curWood = 0; curStone = 0;
        needWoodText.text = curWood.ToString() + " / " + creatingItem.needWood.ToString();
        needStoneText.text = curStone.ToString() + " / " + creatingItem.needStone.ToString();
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
        creatingP.gameObject.SetActive(true);

        while (cur_Time <= creatingItem.needTime)
        {
            cur_Time += Time.deltaTime;
            creatingP.fillAmount = cur_Time / creatingItem.needTime;
            yield return null;
        }

        creatingP.gameObject.SetActive(false);
    }
}
