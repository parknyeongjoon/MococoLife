using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// 카메라 앞으로 이동 시에만 보스가 따라가게
/// 타이머 시작 시간이 달라서 버그 발생 - master client가 all via server로 시작하기?
/// 오른쪽으로 이동 후 왼쪽으로 갈 때 icon 안 뜸
/// </summary>
public class CameraMove : MonoBehaviourPun//카메라를 이동해주는 함수
{
    GameManager gameManager;

    [SerializeField] Image timerFG;
    [SerializeField] Image stopWatch;

    [SerializeField] float cameraSpeed;
    bool canMove = true;//카메라가 움직일 수 있는가

    bool[] leftCameraMove = new bool[4];
    bool[] rightCameraMove = new bool[4];

    int areaCount = 0;
    float[] areaTime;
    readonly float areaMaxTime = 30.0f;

    Coroutine timerCo;

    IEnumerator Start()
    {
        gameManager = GameManager.Instance;

        areaTime = new float[TileManager.Instance.areaCount];
        for(int i = 0; i < TileManager.Instance.areaCount; i++)
        {
            areaTime[i] = areaMaxTime;
        }

        yield return new WaitUntil(() => gameManager.players[gameManager.MyPlayerNum] != null);

        timerCo = StartCoroutine(StartAreaTimer(0));//게임 시작 함수를 만들고 그 안에 포함시키기
    }

    [PunRPC] void SetCameraMove(int index, bool isMove, bool isRight)
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

    [PunRPC] void SetMoveIcon(int index, bool isMove, bool isRight)//최적화하려면 어떤 행동할 지 결정해서 rpc를 보내는 게 더 좋을 거 같음
    {
        PlayerInfo info = gameManager.players[index];
        if (isMove)//움직이려고 하면 아이콘 활성화
        {
            if (isRight && areaCount < TileManager.Instance.areaCount - 1)
            {
                info.RAreaMoveIcon.SetActive(true);
            }
            else if(!isRight && areaCount > 0)
            {
                info.LAreaMoveIcon.SetActive(true);
            }
        }
        else//안 움직이려고 하면 아이콘 비활성화
        {
            info.RAreaMoveIcon.SetActive(false);
            info.LAreaMoveIcon.SetActive(false);
        }
    }

    IEnumerator MoveCamera(bool isRight)//플레이어 모두가 맵의 가장 오른쪽에 있을 때 맵 이동
    {
        StopCoroutine(timerCo);
        canMove = false;//움직이는 동안에는 다시 움직이기 못하게

        Vector3 cameraSpeedV = new Vector3(cameraSpeed, 0, 0);

        if (isRight && areaCount < TileManager.Instance.areaCount - 1)//오른쪽으로 이동하려할 때
        {
            areaTime[areaCount] = 0;//현재 지역의 시간을 0으로 만들고
            areaCount++;//다음 지역으로 이동

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

        timerCo = StartCoroutine(StartAreaTimer(areaCount));

        yield return new WaitForSeconds(1.0f);//이동이 완료되고 1초 동안은 다시 못 움직이게
        canMove = true;
    }

    IEnumerator StartAreaTimer(int areaIndex)
    {
        timerFG.fillAmount = areaTime[areaIndex] / areaMaxTime;
        stopWatch.color = Color.white;

        while (areaTime[areaIndex] > 0)
        {
            timerFG.fillAmount = areaTime[areaIndex] / areaMaxTime;
            areaTime[areaIndex] -= Time.deltaTime;
            yield return null;
        }

        //시간이 초과되면 area 파괴하기
        stopWatch.color = Color.red;

        while (true)
        {
            gameManager.players[gameManager.MyPlayerNum].Damage(Dmg_Type.Damage, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
