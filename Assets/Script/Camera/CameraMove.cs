using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CameraMove : MonoBehaviourPun//ī�޶� �̵����ִ� �Լ�
{
    GameManager gameManager;

    [SerializeField] Image timerFG;

    [SerializeField] float cameraSpeed;
    bool canMove = true;//ī�޶� ������ �� �ִ°�

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
        if(areaTime[areaCount] <= 0)//���� area�� Ÿ�̸Ӱ� �� �ƴٸ� �� ��� �ϱ�
        {
            gameManager.players[gameManager.MyPlayerNum].Damage(Dmg_Type.Damage, 0.01f);
        }
    }

    [PunRPC]
    void SetCameraMove(int index, bool isMove, bool isRight)
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

    IEnumerator MoveCamera(bool isRight)//�÷��̾� ��ΰ� ���� ���� �����ʿ� ���� �� �� �̵�
    {
        canMove = false;//�����̴� ���ȿ��� �ٽ� �����̱� ���ϰ�

        Vector3 cameraSpeedV = new Vector3(cameraSpeed, 0, 0);

        if (isRight && areaCount < TileManager.Instance.areaCount - 1)//���������� �̵��Ϸ��� ��
        {
            areaCount++;

            if (!TileManager.Instance.isAreaVisited[areaCount])//���� �湮�� ���� ���ٸ� Ÿ�̸� �����ϱ�
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

        yield return new WaitForSeconds(1.0f);//�̵��� �Ϸ�ǰ� 1�� ������ �ٽ� �� �����̰�
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

        TileManager.Instance.DestroyArea(areaIndex);//�ð��� �ʰ��Ǹ� area �ı��ϱ�
    }
}
