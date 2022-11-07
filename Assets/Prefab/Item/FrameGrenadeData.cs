using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameGrenadeData", menuName = "SO/Item/FrameGrenadeData")]
public class FrameGrenadeData : GrenadeData
{
    [SerializeField] int dmg_Count;

    public override void Effect(Vector2Int effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);
        int tS = targets.Length;

        for (int i = 0; i < tS; i++)
        {
            FrameGrenadeEffect(targets[i]);
        }
    }

    IEnumerator FrameGrenadeEffect(Collider2D target)//2중for문에 getcomponent 너무 낭비인듯..
    {
        for(int i = 0; i < dmg_Count; i++)
        {
            target.GetComponent<IDamagable>().Damage(dmg_Type, dmg);
        }
        yield return new WaitForSeconds(0.1f);
    }
}
