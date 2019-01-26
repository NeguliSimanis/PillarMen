using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawnPoint : MonoBehaviour
{
    [SerializeField]
    GameObject[] cloudsToSpawn;
    int lastSpawnedCloudID = -1;
    int nextSpawnedCloudID;
    int cloudArrayLength;

    //float cloudLifeTime = 7f;
    float cloudRespawnTime = 0f;
    float cloudRespawnDelay = 9.2f;

    void Start()
    {
        SpawnCloud();
        Debug.Log("clouds " + cloudsToSpawn.Length);
    }

    void SpawnCloud()
    {
        cloudRespawnTime = Time.time + cloudRespawnDelay;
        ChooseCloudToSpawn();
        GameObject newCloud = Instantiate(cloudsToSpawn[nextSpawnedCloudID], gameObject.transform);
        newCloud.transform.position = transform.position;
        lastSpawnedCloudID = nextSpawnedCloudID;
    }

    private void Update()
    {
        if (Time.time > cloudRespawnTime)
        {
            SpawnCloud();
        }
    }

    void ChooseCloudToSpawn()
    {
        if (lastSpawnedCloudID == -1)
        {
            nextSpawnedCloudID = Random.Range(0, cloudsToSpawn.Length-1);
        }
        else
        {
            int tempCloudID = Random.Range(0, cloudsToSpawn.Length - 1);
            if (tempCloudID == lastSpawnedCloudID && tempCloudID != cloudsToSpawn.Length-1)
            {
                nextSpawnedCloudID++;
            }
        }
    }
}
