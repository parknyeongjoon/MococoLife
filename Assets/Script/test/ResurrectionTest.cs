using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionTest : MonoBehaviour
{
    public void Resurrection(int index)
    {
        GameManager.Instance.players[index].Resurrection(new Vector3(15,0,0));
    }
}
