using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class testPlayerMove : MonoBehaviourPun
{
    PhotonView PV;

    void Start()
    {
        PV = photonView;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 0.03f, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-0.03f, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -0.03f, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(0.03f, 0, 0);
            }
        }
    }
}
