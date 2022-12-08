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

    public virtual IEnumerator Effect(PlayerInfo info, Vector3 effectPos)//void로 바꾸고 startCoroutine으로 할 지?
    {
        yield return null;
    }

    public virtual void Effect(PlayerInfo info)
    {

    }
}
