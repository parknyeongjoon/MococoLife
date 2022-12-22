using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PUI : MonoBehaviour
{
    [SerializeField] GameObject HP_GO;
    [SerializeField] Image HP_FG;
    [SerializeField] GameObject grave;
    [SerializeField] Image[] inventoryImg;
    [SerializeField] TMP_Text[] inventoryTxt;
    [SerializeField] int index;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        StartCoroutine(UpdateHPBar(index));
    }

    IEnumerator UpdateHPBar(int index)
    {
        yield return new WaitUntil(() => gameManager.players[index] != null);
        yield return new WaitUntil(() => HP_FG != null);
        while (true)
        {
            HP_FG.fillAmount = gameManager.players[index].Hp / 100;
            yield return null;
        }
    }

    public void SetGraveIcon(bool isActive)
    {
        grave.gameObject.SetActive(isActive);
    }

    public void InventoryUpdate(int index, string code, int count)
    {
        if(count == 0)
        {
            inventoryImg[index].gameObject.SetActive(false);
            inventoryTxt[index].text = "";
        }
        else
        {
            inventoryImg[index].sprite = gameManager.itemDic[code].itemImg;
            inventoryTxt[index].text = count.ToString();
            inventoryImg[index].gameObject.SetActive(true);
        }
    }
}
