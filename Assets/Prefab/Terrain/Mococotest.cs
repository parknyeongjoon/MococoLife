using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mococotest : MonoBehaviourPun
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GetMococo();
    }

    [PunRPC]
    void GetMococo()
    {
        Debug.Log("Get Mococo");
        Destroy(gameObject);
    }
}
