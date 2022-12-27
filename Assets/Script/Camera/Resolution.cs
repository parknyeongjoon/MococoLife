using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float height = ((float)Screen.width / Screen.height) / (16f / 9);
        float width = 1f / height;
        if (height < 1)
        {
            rect.height = height;
            rect.y = (1f - height) / 2f;
        }
        else
        {
            rect.width = width;
            rect.x = (1f - width) / 2f;
        }
        camera.rect = rect;
    }
}
