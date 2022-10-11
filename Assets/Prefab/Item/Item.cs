using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    public void Effect(Vector2Int effectPos)
    {
        itemData.Effect(effectPos);
    }
}
