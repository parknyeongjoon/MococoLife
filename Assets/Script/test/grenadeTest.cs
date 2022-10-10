using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeTest : MonoBehaviour
{
    [SerializeField] ItemData testItem;
    Camera cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 클릭");
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int effectPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            testItem.Effect(effectPos);
        }
    }
}
