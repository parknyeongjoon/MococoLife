using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// ī�޶� ������ �̵� �ÿ��� ������ ���󰡰�(����)
/// Ÿ�̸� ���� �ð��� �޶� ���� �߻� - master client�� all via server�� �����ϱ�?(�ذ�)
/// ���������� �̵� �� �������� �� �� icon �� ��(�ذ�)
/// </summary>
public class AreaMove : MonoBehaviourPun//ī�޶� �̵����ִ� �Լ�
{
    GameManager gameManager;

    [SerializeField] Image timerFG;
    [SerializeField] Image stopWatch;

    [SerializeField] float cameraSpeed;
    bool canMove = true;//ī�޶� ������ �� �ִ°�
    bool isMove = false;//ī�޶� �����̴� ������

    bool[] leftAreaMove = new bool[4];
    bool[] rightAreaMove = new bool[4];

    int areaCount = 0;
    float[] areaTime;
    readonly float areaMaxTime = 30.0f;

    Coroutine timerCo;

    public bool IsMove { get => isMove; }

    IEnumerator Start()
    {
        gameManager = GameManager.Instance;

        areaTime = new float[TileManager.Instance.areaCount];
        for (int i = 0; i < TileManager.Instance.areaCount; i++)
        {
            areaTime[i] = areaMaxTime;
        }

        yield return new WaitUntil(() => gameManager.players[gameManager.MyPlayerNum] != null);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCStartAreaTimer", RpcTarget.AllViaServer, 0);
        }
    }

    [PunRPC]
    void SetCameraMove(int index, bool _isMove, bool isRight)
    {
        if (isRight)//���������� ���� �Ŷ��
        {
            rightAreaMove[index] = _isMove;

            if (_isMove)
            {
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (rightAreaMove[i])
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
            leftAreaMove[index] = _isMove;

            if (_isMove)
            {
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (leftAreaMove[i])
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

    [PunRPC]
    void SetMoveIcon(int index, bool _isMove, bool isRight)//����ȭ�Ϸ��� � �ൿ�� �� �����ؼ� rpc�� ������ �� �� ���� �� ����
    {
        PlayerInfo info = gameManager.players[index];
        if (_isMove)//�����̷��� �ϸ� ������ Ȱ��ȭ
        {
            if (isRight && areaCount < TileManager.Instance.areaCount - 1)
            {
                info.RAreaMoveIcon.SetActive(true);
            }
            else if (!isRight && areaCount > 0)
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
        isMove = true;

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

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCStartAreaTimer", RpcTarget.AllViaServer, areaCount);
        }
        yield return new WaitForSeconds(1.0f);//�̵��� �Ϸ�ǰ� 1�� ������ �ٽ� �� �����̰�
        isMove = false;
        canMove = true;
    }

    [PunRPC]
    void RPCStartAreaTimer(int index)
    {
        timerCo = StartCoroutine(StartAreaTimer(index));
    }

    IEnumerator StartAreaTimer(int areaIndex)
    {
        areaCount = areaIndex;
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
            gameManager.players[gameManager.MyPlayerNum].Damage(Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
