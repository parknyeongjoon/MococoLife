using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    GameManager gameManager;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        gameManager = GameManager.Instance;

        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        gameManager.isCameraMove = true;
        Vector3 moveV = new Vector3(moveSpeed, 0, 0);

        while (gameManager.isCameraMove)
        {
            transform.position += moveV;
            yield return null;
        }
    }
}
