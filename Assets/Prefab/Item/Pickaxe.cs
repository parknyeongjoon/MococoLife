using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickaxe", menuName = "SO/Item/Pickaxe")]
public class Pickaxe : ItemData
{
    public override void Effect(Vector2 effectPos)
    {
        Collider2D[] targets = Physics2D.OverlapPointAll(effectPos, 1 << LayerMask.NameToLayer("Terrain"));
        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Rock"))
            {
                Debug.Log("Ã¤±¤ Áß..");
                target.GetComponent<IDamagable>().Damage(Dmg_Type.Pickaxe, 1);
                break;
            }
        }
    }
}
