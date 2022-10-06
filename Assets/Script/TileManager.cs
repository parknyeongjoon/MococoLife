using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> BaltanAreas;

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            int rand = Random.Range(0, BaltanAreas.Count);
            Instantiate(BaltanAreas[rand], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
