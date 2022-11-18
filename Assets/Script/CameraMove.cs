using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour//ī�޶�� ���� ��ũ���� ���ִ� �Լ�
{
    GameManager gameManager;
    public bool isCameraMove;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        yield return new WaitUntil(() => gameManager.isGameReady[0] && TileManager.Instance != null);//�ٲٱ�
        StartCoroutine(MoveCamera(gameManager.gameSpeed));
    }

    IEnumerator MoveCamera(float cameraSpeed)//������ �Ͻ������� Ǯ�ų� ī�޶� ���������� �� �������ֱ�?
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
