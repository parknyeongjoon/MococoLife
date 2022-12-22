using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ToolCollider : MonoBehaviour
{
    [SerializeField] PlayerInfo info;
    GameObject curInteractiveIcon;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (info.Hand.itemData.code == "T_01" && collision.CompareTag("Tree"))//µµ³¢¶ó¸é ³ª¹« Å½»ö
        {
            curInteractiveIcon = collision.GetComponent<Terrain>().InteractiveIcon;
            curInteractiveIcon.SetActive(true);
        }
        else if (info.Hand.itemData.code == "T_02" && collision.CompareTag("Rock"))
        {
            curInteractiveIcon = collision.GetComponent<Terrain>().InteractiveIcon;
            curInteractiveIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(curInteractiveIcon != null && collision.gameObject == curInteractiveIcon.transform.parent.gameObject)
        {
            curInteractiveIcon.SetActive(false);
            curInteractiveIcon = null;
        }
    }
}
