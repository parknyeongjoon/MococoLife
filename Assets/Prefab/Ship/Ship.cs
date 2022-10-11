using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Ship : MonoBehaviourPun
{
    PhotonView PV;
    [SerializeField] GameObject createPanel;

    void Start()
    {
        PV = photonView;
        createPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,1.5f,0));
    }

    public void PanelCloseBtn()
    {
        PV.RPC("CloseCreatePanel", RpcTarget.AllViaServer);
    }

    void OnMouseDown()
    {
        PV.RPC("OpenCreatePanel", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void OpenCreatePanel()
    {
        createPanel.gameObject.SetActive(true);
    }
    
    [PunRPC]
    void CloseCreatePanel()
    {
        createPanel.gameObject.SetActive(false);
    }
}
