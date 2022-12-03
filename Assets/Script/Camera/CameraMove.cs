using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CameraMove : MonoBehaviourPun//카메라를 이동해주는 함수
{
    GameManager gameManager;

    [SerializeField] Image timerFG;

    [SerializeField] float cameraSpeed;
    bool canMove = true;//카메라가 움직일 수 있는가

    bool[] leftCameraMove = new bool[4];
    bool[] rightCameraMove = new bool[4];

    int areaCount = 0;
    float[] areaTime;
    readonly float areaMaxTime = 10.0f;

    IDamagable PDamage;

    void Start()
    {
        gameManager = GameManager.Instance;

        areaTime = new float[TileManager.Instance.areaCount];

        TileManager.Instance.isAreaVisited[0] = true;
        StartCoroutine(StartAreaTimer(0));
    }

    void Update()
    {
        timerFG.fillAmount = areaTime[areaCount] / areaMaxTime;
        if(areaTime[areaCount] <= 0)//현재 area의 타이머가 다 됐다면 피 닳게 하기
        {
            gameManager.players[gameManager.MyPlayerNum].Damage(Dmg_Type.Damage, 0.01f);
        }
    }

    [PunRPC]
    void SetCameraMove(int index, bool isMove, bool isRight)
    {
        if (isRight)//오른쪽으로 가는 거라면
        {
            rightCameraMove[index] = isMove;

            if (isMove)
            {
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (rightCameraMove[i])
                    {
                        count++;
                        if (canMove && count >= PhotonNetwork.CurrentRoom.PlayerCount)
                        {
                            StartCoroutine(MoveCamera(isRight));
                        }
                    }
                }
            }
        }
        else//왼쪽으로 가는 거라면
        {
            leftCameraMove[index] = isMove;

            if (isMove)
            {
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (leftCameraMove[i])
                    {
                        count++;
                        if (canMove && count >= PhotonNetwork.CurrentRoom.PlayerCount)
                        {
                            StartCoroutine(MoveCamera(isRight));
                        }
                    }
                }
            }
        }
    }

    IEnumerator MoveCamera(bool isRight)//플레이어 모두가 맵의 가장 오른쪽에 있을 때 맵 이동
    {
        canMove = false;//움직이는 동안에는 다시 움직이기 못하게

        Vector3 cameraSpeedV = new Vector3(cameraSpeed, 0, 0);

        if (isRight && areaCount < TileManager.Instance.areaCount - 1)//오른쪽으로 이동하려할 때
        {
            areaCount++;

            if (!TileManager.Instance.isAreaVisited[areaCount])//아직 방문한 적이 없다면 타이머 시작하기
            {
                TileManager.Instance.isAreaVisited[areaCount] = true;
                StartCoroutine(StartAreaTimer(areaCount));
            }
            float destiny = transform.position.x + 30;

            while (transform.position.x < destiny)
            {
                transform.position += cameraSpeedV * Time.deltaTime;
                yield return null;
            }
        }
        else if (!isRight && areaCount > 0)//왼쪽으로 이동하려고 할 때
        {
            areaCount--;
            float destiny = transform.position.x - 30;

            while (transform.position.x > destiny)
            {
                transform.position -= cameraSpeedV * Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(1.0f);//이동이 완료되고 1초 동안은 다시 못 움직이게
        canMove = true;
    }

    IEnumerator StartAreaTimer(int areaIndex)
    {
        areaTime[areaIndex] = areaMaxTime;

        while (areaTime[areaIndex] > 0)
        {
            areaTime[areaIndex] -= Time.deltaTime;
            yield return null;
        }

        TileManager.Instance.DestroyArea(areaIndex);//시간이 초과되면 area 파괴하기
    }
}
