using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

enum State { Normal, Behave, Incapacitated}
public class Valtan : Boss
{
    private int armor; // curState, curHealth, incapacitation, target

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (curState == (int)State.Normal)
        {

        }
        else if (curState == (int)State.Behave)
        {

        }
        else if (curState == (int)State.Incapacitated)
        {

        }
    }

    public void GetDestroyed(int destroy)
    {
        armor -= destroy;
    }

    public override void Initialize(int difficulty)
    {
        armor = 2;
        curState = (int)State.Normal;
        curHealth = 100f;
    }

    public override void GetDamaged(float damage)
    {
        curHealth -= damage;
    }

    public override void GetIncpacitated(float incapacitation)
    {
        this.incapacitation -= incapacitation;
        if (incapacitation <= 0)
        {
            ActiveIncapacitationState();
        }
    }

    public override void ActiveIncapacitationState()
    {
        curState = (int)State.Incapacitated;
    }

    public override void MoveAndBehave()
    {
        int nextPattern = DetermineNextPattern();
        BehavePattern(nextPattern);
    }

    public override int DetermineNextPattern()
    {
        return 0;
    }

    public override void BehavePattern(int pattern)
    {

    }
}
