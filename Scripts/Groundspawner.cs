using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefabs; // The ground prefab to be spawned
    public Vector3 nextSpawnPos;     // The position where the next ground will spawn

    public ObjectSpawner objectSpawner;

    void Start()
    {
        // Spawn initial grounds
        for (int i = 0; i < 3; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame (if needed for any dynamic behavior)
    void Update()
    {
    }

    public void Spawn()
    {
        //int randomPrefab = Random.Range(0, groundPrefabs.Length);
        
       GameObject tempGround = Instantiate(groundPrefabs, nextSpawnPos, Quaternion.identity);
       

        nextSpawnPos = new Vector3(nextSpawnPos.x, nextSpawnPos.y, tempGround.transform.GetChild(0).transform.position.z);
    }
}
