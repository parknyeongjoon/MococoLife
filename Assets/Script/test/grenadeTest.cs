using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeTest : MonoBehaviour
{
    [SerializeField] ItemData testItem;

    PoolManager poolManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 클릭");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            /*Vector2Int effectPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            testItem.Effect(effectPos);*/
            poolManager = PoolManager.Instance;

            poolManager.Get("I_00", mousePos);
        }
    }
}
