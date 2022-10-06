using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "ScriptableObject/Item/FrameGrenadeData")]
public class FrameGrenadeData : GrenadeData
{
    public override void Effect(Vector2Int effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);
        int tS = targets.Length;

        for (int i = 0; i < tS; i++)
        {
            targets[i].GetComponent<IDamagable>().Damage(dmg_Type, dmg);
        }
    }
}
