using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameGrenadeData", menuName = "SO/Item/FrameGrenadeData")]
public class FrameGrenadeData : GrenadeData
{
    /*
    [SerializeField] int dmg_Count;
    [SerializeField] float dmg_Interval;

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
                targets[i].GetComponent<MonoBehaviour>().StartCoroutine(FrameGrenadeEffect(targets[i]));
            }
            info.State = State.Idle;
        }
    }

    IEnumerator FrameGrenadeEffect(Collider2D target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();

        if (damagable != null)
        {
            for (int i = 0; i < dmg_Count; i++)
            {
                damagable.Damage(dmg);
                yield return new WaitForSeconds(dmg_Interval);
            }
        }
    }
    */
}