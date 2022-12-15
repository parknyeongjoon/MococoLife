using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// ī�޶� ������ �̵� �ÿ��� ������ ���󰡰�
/// Ÿ�̸� ���� �ð��� �޶� ���� �߻� - master client�� all via server�� �����ϱ�?
/// ���������� �̵� �� �������� �� �� icon �� ��
/// </summary>
public class CameraMove : MonoBehaviourPun//ī�޶� �̵����ִ� �Լ�
{
    GameManager gameManager;

    [SerializeField] Image timerFG;
    [SerializeField] Image stopWatch;

    [SerializeField] float cameraSpeed;
    bool canMove = true;//ī�޶� ������ �� �ִ°�

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

        timerCo = StartCoroutine(StartAreaTimer(0));//���� ���� �Լ��� ����� �� �ȿ� ���Խ�Ű��
    }

    [PunRPC] void SetCameraMove(int index, bool isMove, bool isRight)
    {
        if (isRight)//���������� ���� �Ŷ��
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
        else//�������� ���� �Ŷ��
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

    [PunRPC] void SetMoveIcon(int index, bool isMove, bool isRight)//����ȭ�Ϸ��� � �ൿ�� �� �����ؼ� rpc�� ������ �� �� ���� �� ����
    {
        PlayerInfo info = gameManager.players[index];
        if (isMove)//�����̷��� �ϸ� ������ Ȱ��ȭ
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
        else//�� �����̷��� �ϸ� ������ ��Ȱ��ȭ
        {
            info.RAreaMoveIcon.SetActive(false);
            info.LAreaMoveIcon.SetActive(false);
        }
    }

    IEnumerator MoveCamera(bool isRight)//�÷��̾� ��ΰ� ���� ���� �����ʿ� ���� �� �� �̵�
    {
        StopCoroutine(timerCo);
        canMove = false;//�����̴� ���ȿ��� �ٽ� �����̱� ���ϰ�

        Vector3 cameraSpeedV = new Vector3(cameraSpeed, 0, 0);

        if (isRight && areaCount < TileManager.Instance.areaCount - 1)//���������� �̵��Ϸ��� ��
        {
            areaTime[areaCount] = 0;//���� ������ �ð��� 0���� �����
            areaCount++;//���� �������� �̵�

            float destiny = transform.position.x + 30;

            while (transform.position.x < destiny)
            {
                transform.position += cameraSpeedV * Time.deltaTime;
                yield return null;
            }
        }
        else if (!isRight && areaCount > 0)//�������� �̵��Ϸ��� �� ��
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

        yield return new WaitForSeconds(1.0f);//�̵��� �Ϸ�ǰ� 1�� ������ �ٽ� �� �����̰�
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

        //�ð��� �ʰ��Ǹ� area �ı��ϱ�
        stopWatch.color = Color.red;

        while (true)
        {
            gameManager.players[gameManager.MyPlayerNum].Damage(Dmg_Type.Damage, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
