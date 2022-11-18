using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public Slot tileSlot = new Slot();
    public bool isTerrain = false;

    public void SetSprite(string code)
    {
        ///타일맵 써서 머 하기
    }
}
