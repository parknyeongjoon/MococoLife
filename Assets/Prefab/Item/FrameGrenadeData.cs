using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameGrenadeData", menuName = "SO/Item/FrameGrenadeData")]
public class FrameGrenadeData : GrenadeData
{
    [SerializeField] int dmg_Count;
    [SerializeField] float dmg_Interval;

    public override void Effect(Vector2Int effectPos)
    {
        Collider2D[] targets = FindTarget(effectPos);
        int tS = targets.Length;

        for (int i = 0; i < tS; i++)
        {
            targets[i].GetComponent<MonoBehaviour>().StartCoroutine(FrameGrenadeEffect(targets[i]));
        }
    }

    IEnumerator FrameGrenadeEffect(Collider2D target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();

        if (damagable != null)
        {
            for (int i = 0; i < dmg_Count; i++)
            {
                damagable.Damage(dmg_Type, dmg);
                yield return new WaitForSeconds(dmg_Interval);
            }
        }
    }
}
