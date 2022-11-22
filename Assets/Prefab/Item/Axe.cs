using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Axe", menuName = "SO/Item/Axe")]
public class Axe : ItemData
{
    public override void Effect(Vector2 effectPos)
    {
        Collider2D[] targets = Physics2D.OverlapPointAll(effectPos, 1 << LayerMask.NameToLayer("Terrain"));
        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Tree"))
            {
                Debug.Log("¹ú¸ñ Áß..");
                target.GetComponent<IDamagable>().Damage(Dmg_Type.Axe, 1);
                break;
            }
        }
    }
}
