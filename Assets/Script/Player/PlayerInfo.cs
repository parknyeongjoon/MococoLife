using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPun
{
    public PlayerMove playerMove;

    public float hp = 100;
    public State state;
    public Slot hand = new Slot();
    public Slot[] inventory = new Slot[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
