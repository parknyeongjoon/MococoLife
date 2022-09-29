using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap tilemap;

    //���콺�� Ÿ�� ���� ��ġ�� ���� �۾��� ���̱� ������ onMouseOver�� ����߽��ϴ�.

    //�����ϸ� ������ �ϴ°͵� ������ ���׿�.

    void Start()
    {
        try
        {
            tilemap = transform.GetComponent<Tilemap>();

            Vector3Int temp = tilemap.WorldToCell(new Vector3(4,0,0));

            tilemap.SetTileFlags(temp, TileFlags.None);

            //Ÿ�� �� �ٲٱ�
            tilemap.SetColor(temp, (Color.blue));
            
        }
        catch (NullReferenceException)
        {
            Debug.Log("null");
        }
    }

    void OnMouseOver()
    {
        try
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);



            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);



            if (tilemap = hit.transform.GetComponent<Tilemap>())
            {
                tilemap.RefreshAllTiles();

                int x, y;
                x = tilemap.WorldToCell(ray.origin).x;
                y = tilemap.WorldToCell(ray.origin).y;

                Vector3Int v3Int = new Vector3Int(x, y, 0);



                //Ÿ�� �� �ٲ� �� �̰� �־�� �ϴ�����
                tilemap.SetTileFlags(v3Int, TileFlags.None);

                //Ÿ�� �� �ٲٱ�
                tilemap.SetColor(v3Int, (Color.red));

            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("null");
        }
    }

    void OnMouseExit()
    {
        tilemap.RefreshAllTiles();

    }
}