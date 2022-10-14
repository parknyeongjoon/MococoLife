using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour//카메라로 오토 스크롤을 해주는 함수
{
    GameManager gameManager;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        gameManager = GameManager.Instance;

        StartCoroutine(MoveCamera(gameManager.cameraSpeed));
    }

    IEnumerator MoveCamera(float cameraSpeed)
    {
        gameManager.isCameraMove = true;
        Vector3 moveV = new Vector3(cameraSpeed, 0, 0);
        WaitForFixedUpdate cameraPosUpdate = new();

        while (gameManager.isCameraMove)
        {
            if(transform.position.x >= 30 * (gameManager.tileCount - 1)) { break; }
            transform.position += moveV;
            yield return cameraPosUpdate;
        }
    }
}
