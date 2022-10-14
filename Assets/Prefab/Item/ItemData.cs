using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImg;
    public string code;
    public int maxCount;
    public int needWood;
    public int needStone;
    public float needTime;

    public virtual void Effect(Vector2Int effectPos)
    {

    }
}
