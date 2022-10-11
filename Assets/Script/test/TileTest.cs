using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap tilemap;

    //마우스가 타일 위에 위치할 때만 작업할 것이기 때문에 onMouseOver를 사용했습니다.

    //가능하면 기즈모로 하는것도 좋을것 같네요.

    void Start()
    {
        try
        {
            tilemap = transform.GetComponent<Tilemap>();

            Vector3Int temp = tilemap.WorldToCell(new Vector3(4,0,0));

            tilemap.SetTileFlags(temp, TileFlags.None);

            //타일 색 바꾸기
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



                //타일 색 바꿀 때 이게 있어야 하더군요
                tilemap.SetTileFlags(v3Int, TileFlags.None);

                //타일 색 바꾸기
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