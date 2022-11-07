using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "SO/Item/GrenadeData")]
public class GrenadeData : ItemData
{
    [SerializeField] protected Dmg_Type dmg_Type;
    [SerializeField] protected float dmg;

    public override void Effect(Vector2Int effectPos)
    {
        Debug.Log("ÆøÅº »ç¿ë");
        Collider2D[] targets = FindTarget(effectPos);

        int tS = targets.Length;
        for (int i = 0; i < tS; i++)
        {
            targets[i].GetComponent<IDamagable>().Damage(dmg_Type, dmg);
        }
    }

    protected Collider2D[] FindTarget(Vector2Int effectPos)
    {
        Debug.Log(effectPos);
        return Physics2D.OverlapBoxAll(effectPos, new Vector2(0.5f, 0.5f), 0, 1 << LayerMask.NameToLayer("Terrain"));
    }
}
