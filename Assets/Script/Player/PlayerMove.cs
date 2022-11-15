using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    GameObject io;

    private enum State { Idle, Dash, Attack, Dead };
    private enum Hand { Hammer, Mine, Axe, Item };

    private float hp = 100;
    private Vector3 mousePos;
    private ProductTerrain to;

    void Start()
    {
        PV = photonView;
    }

    void Dash()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.01f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.01f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -0.01f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.01f, 0, 0);
        }
    }

    void OnTriggerStay2D(Collider2D collision)//�浹ü ������Ʈ�� ��ũ��Ʈ�� ����
    {
        if (collision.CompareTag("Tree"))
        {
            Debug.Log("Meet Tree");
            io = collision.gameObject;
        }
        else if (collision.CompareTag("Stone"))
        {
            Debug.Log("You Can Mining");
            io = collision.gameObject;
        }
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 2, 0) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-2, 0, 0) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -2, 0) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(2, 0, 0) * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (io != null)
                {
                    Debug.Log("Tree To Wood");
                    to = io.GetComponent<ProductTerrain>();
                    to.Damage(Dmg_Type.None, 30);
                    //io = io.getscript
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Dash();
            }
            //if()   
        }
        /*
        Hand�� ax, Mine�϶� ���� �ݶ��̴�
        Terrain ���� interface�� �ҷ��� ���
        Hand���� Terrain ���� ���� �� ������ ����.
        if(Hand == AX){
            ����
            if(Input.GetMouseButton(0)&&���� object){


        }
        }
        if(Hand == MINE){
            if(Input.GetMouseButton(0)&&���� object)

            ä��
        }
        if(Hand == ITEM){
            if(Input.GetMouseButton(0)){
            mousePos = Input.mousePosition;
            effect
        }
            ����(���� ����)
        }
        if(Hand == NULL){
        //�ٴ��� ���� �ݴ� �ݶ��̴�
        }
        */
    }
}
