using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] List<GameObject> BaltanAreas;
    [SerializeField] GameObject rightBoundary;
    [SerializeField] int tileCount;

    void Start()
    {
        for (int i = 0; i < tileCount; i++)
        {
            int rand = Random.Range(0, BaltanAreas.Count);
            Instantiate(BaltanAreas[rand], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }
        rightBoundary.transform.position = new Vector3(30 * tileCount - 15, 0, 0);
    }
}
