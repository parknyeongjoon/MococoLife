using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public int maxCount;
    public int needWood;
    public int needStone;

    public virtual void Effect(Vector2Int effectPos)
    {

    }
}
