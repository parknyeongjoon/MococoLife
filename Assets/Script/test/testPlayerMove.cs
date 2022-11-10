using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class testPlayerMove : MonoBehaviourPun
{
    PhotonView PV;
    private enum Hand { MINE, AX, ITEM };
    private bool state = true;
    void Start()
    {
        PV = photonView;
    }
    void Dash()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.1f, 0, 0);
        }
    }
    void Update()
    {
        if (PV.IsMine)
        {
            if (state)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += new Vector3(0, 0.03f, 0);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position += new Vector3(-0.03f, 0, 0);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position += new Vector3(0, -0.03f, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(0.03f, 0, 0);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    Dash();
                }
            }
            /*
            if(Hand == AX){
                ����
            }
            if(Hand == MINE){
                ä��
            }
            if(Hand == ITEM){
                ����(���� ����)
            }
            */
        }

    }
}
/*
public class Slot : MonoBehaviour
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage;  // �������� �̹���

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    // ������ �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    // �ش� ������ ������ ���� ������Ʈ
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // �ش� ���� �ϳ� ����
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }
}
 public class Inventory : MonoBehaviour
{
    public static bool invectoryActivated = false;  // �κ��丮 Ȱ��ȭ ����. true�� �Ǹ� ī�޶� �����Ӱ� �ٸ� �Է��� ���� ���̴�.

    [SerializeField]
    private GameObject go_InventoryBase; // Inventory_Base �̹���
    [SerializeField] 
    private GameObject go_SlotsParent;  // Slot���� �θ��� Grid Setting 

    private Slot[] slots;  // ���Ե� �迭

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            invectoryActivated = !invectoryActivated;

            if (invectoryActivated)
                OpenInventory();
            else
                CloseInventory();

        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null �̶�� slots[i].item.itemName �� �� ��Ÿ�� ���� ����
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}

 
 */
