using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour//카메라로 오토 스크롤을 해주는 함수
{
    GameManager gameManager;
    public bool isCameraMove;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        yield return new WaitUntil(() => gameManager.isGameReady[0] && TileManager.Instance != null);//바꾸기
        StartCoroutine(MoveCamera(gameManager.gameSpeed));
    }

    IEnumerator MoveCamera(float cameraSpeed)//실행을 일시정지를 풀거나 카메라가 움직여야할 떄 실행해주기?
    {
        gameManager.isPause = false;
        WaitWhile cameraWaitWhile = new WaitWhile(() => gameManager.isPause || !isCameraMove);
        float destiny = (TileManager.Instance.areaCount - 1) * 30;

        while (transform.position.x <= destiny)
        {
            transform.position += new Vector3(cameraSpeed, 0, 0) * Time.deltaTime;
            yield return cameraWaitWhile;
        }
    }
}
