using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "SO/Item/GrenadeData")]
public class GrenadeData : BattleItemData
{
    [SerializeField] protected Dmg_Type dmg_Type;
    [SerializeField] protected float dmg;

    public override void Effect(Vector2 effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);

        int tS = targets.Length;
        for (int i = 0; i < tS; i++)
        {
            IDamagable damagable = targets[i].GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.Damage(dmg_Type, dmg);
            }
        }
    }

    protected Collider2D[] FindTarget(Vector2 effectPos)
    {
        return Physics2D.OverlapBoxAll(effectPos, new Vector2(0.5f, 0.5f), 0, 1 << LayerMask.NameToLayer("Terrain"));
    }
}
