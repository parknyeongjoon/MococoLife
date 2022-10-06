using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "ScriptableObject/Item/GrenadeData")]
public class GrenadeData : ItemData
{
    [SerializeField] protected Dmg_Type dmg_Type;
    [SerializeField] protected int dmg;

    public override void Effect(Vector2Int effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);
        int tS = targets.Length;

        for (int i = 0; i < tS; i++)
        {
            targets[i].GetComponent<IDamagable>().Damage(dmg_Type, dmg);
        }
    }

    protected Collider2D[] FindTarget(Vector2Int effectPos)
    {
        return Physics2D.OverlapBoxAll(effectPos + new Vector2(0.5f, 0.5f), new Vector2(1, 1), 0, LayerMask.NameToLayer("Terrain"));
    }
}
