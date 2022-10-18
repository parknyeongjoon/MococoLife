using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Boss: MonoBehaviourPun
{
    public GameManager gameManager;
    public int curState;
    public float curHealth;
    public float incapacitation;
    public int target;

    public abstract void Initialize(int difficulty);
    public abstract void GetDamaged(float damage);
    public abstract void GetIncpacitated(float incapacitation);
    public abstract void ActiveIncapacitationState();
    public abstract void MoveAndBehave();
    public void UpdateTarget()
    {
        if (target >= 0 && target < 4)
        {
            if (Random.Range(0,100) > 30)
            {
                int num;
                while (true)
                {
                    num = Random.Range(0, 4);
                    if (num == target) continue;
                    else
                    {
                        target = num;
                        break;
                    }
                }
            }
        }
        else
        {
            target = Random.Range(0, 4);
        }
    }
    public abstract void DetermineNextPattern();
    public abstract void BehavePattern(int type);
}
