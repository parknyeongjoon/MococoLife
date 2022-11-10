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

        yield return new WaitUntil(() => gameManager.isGameReady && TileManager.Instance != null);
        StartCoroutine(MoveCamera(gameManager.gameSpeed));
    }

    IEnumerator MoveCamera(float cameraSpeed)
    {
        gameManager.isPause = false;
        Vector3 moveV = new Vector3(cameraSpeed, 0, 0);
        WaitWhile isCameraMove = new WaitWhile(() => gameManager.isPause);
        float destiny = (TileManager.Instance.tileCount - 1) * 30;

        while (transform.position.x <= destiny)
        {
            transform.position += moveV * Time.deltaTime;
            yield return isCameraMove;
        }
    }
}
