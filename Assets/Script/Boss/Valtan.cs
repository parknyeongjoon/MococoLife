using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Valtan : Boss
{
    private PhotonView pv;
    private int armor;

    private void Start()
    {
        gameManager = GameManager.Instance;
        pv = GetComponent<PhotonView>();
    }

    public void GetDestroyed(int destroy)
    {

    }

    public override void Initialize(int difficulty)
    {
        armor = 2;
        curState = 0;
        curHealth = 100f;
        Debug.Log("Difficulty is " + difficulty.ToString());
    }

    public override void GetDamaged(float damage)
    {

    }

    public override void GetIncpacitated(float incapacitation)
    {

    }

    public override void ActiveIncapacitationState()
    {

    }

    public override void MoveAndBehave()
    {

    }

    public override void DetermineNextPattern()
    {

    }

    public override void BehavePattern(int type)
    {

    }
}
