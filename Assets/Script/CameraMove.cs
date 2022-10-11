using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
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
        Vector3 moveV = new Vector3(gameManager.cameraSpeed, 0, 0);
        WaitForFixedUpdate cameraPosUpdate = new();

        while (gameManager.isCameraMove)
        {
            if(transform.position.x >= 30 * (gameManager.tileCount - 1)) { break; }
            transform.position += moveV;
            yield return cameraPosUpdate;
        }
    }
}
