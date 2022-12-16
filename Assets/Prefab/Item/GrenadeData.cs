using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "SO/Item/GrenadeData")]
public class GrenadeData : BattleItemData
{
    /*
    [SerializeField] protected float dmg;

    public override IEnumerator Effect(PlayerInfo info, Vector3 effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);

        int tS = targets.Length;
        if(tS > 0)
        {
            info.State = State.Action;

            yield return new WaitForSeconds(delay);
            for (int i = 0; i < tS; i++)
            {
                IDamagable damagable = targets[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.Damage(dmg);
                }
            }
            info.State = State.Idle;
        }
    }

    protected Collider2D[] FindTarget(Vector3 effectPos)
    {
        return Physics2D.OverlapBoxAll(effectPos, new Vector2(0.5f, 0.5f), 0, 1 << LayerMask.NameToLayer("Terrain"));
    }
*/
}