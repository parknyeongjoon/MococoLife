using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviourPun
{
    [SerializeField] ItemData itemData;

    public void Effect(Vector2Int effectPos)
    {
        itemData.Effect(effectPos);
    }
}
