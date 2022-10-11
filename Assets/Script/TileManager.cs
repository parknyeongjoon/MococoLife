using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] List<GameObject> BaltanAreas;
    [SerializeField] GameObject rightBoundary;
    GameManager gameManager;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        gameManager = GameManager.Instance;

        for (int i = 0; i < gameManager.tileCount; i++)
        {
            int rand = Random.Range(0, BaltanAreas.Count);
            Instantiate(BaltanAreas[rand], new Vector3(30 * i, 0, 0), Quaternion.identity);
        }
        rightBoundary.transform.position = new Vector3(30 * gameManager.tileCount - 15, 0, 0);
    }
}
