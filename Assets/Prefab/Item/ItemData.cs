using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImg;
    public TileBase tileImg;
    public string code;
    public Item_Type Item_Type;
    public float delay;

    public virtual void Effect(PlayerInfo info, Vector3 effectPos)
    {

    }
}
