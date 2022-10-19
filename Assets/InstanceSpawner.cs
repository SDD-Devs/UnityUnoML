using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSpawner : MonoBehaviour
{
    public GameObject GameInstancePrefab;

    public int x;
    public int y;
    private void Awake()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject prefab = Instantiate(GameInstancePrefab);
                prefab.transform.position = new Vector2(i * 50.0f, j * 50.0f);
                prefab.name = "GameInstance";
            }
        }
    }
}
