using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    [Header("Camera")]
    public bool isCameraMove;
    public float cameraSpeed;

    [Header("Field")]
    public int tileCount;

    [Header("Boss")]
    private Boss boss;

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            boss = PhotonNetwork.Instantiate("Valtan", new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Valtan>();
            boss.Initialize(0);
        }
    }
}
