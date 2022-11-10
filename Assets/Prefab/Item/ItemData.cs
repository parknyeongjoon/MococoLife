using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImg;
    public GameObject prefab;
    public string code;
    public int maxCount;

    public virtual void Effect(Vector2Int effectPos)
    {

    }
}
