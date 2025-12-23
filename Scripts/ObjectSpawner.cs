using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // To use List

public class ObjectSpawner : MonoBehaviour
{
    // Reference to the Barrier prefab
    public GameObject barrierPrefab;

    // Starting position for spawning (we'll still use this for Y and Z)
    public Vector3 startPos;

    // Offset to control spacing between spawns on Z-axis (increment of 10)
    public int spawnOffsetZ = 10;

    // Rotation value (90 degrees on Y-axis)
    public float rotationY = 90f;

    // Looping X positions (0, 3, 4)
    private int[] spawnXPositions = { 0, 3, 4 };
    private int xPositionIndex = 0;  // To track which X position to spawn at next

    // Time between each spawn (optional)
    public float spawnRate = 1f;

    // List to keep track of all spawned barriers
    private List<GameObject> spawnedBarriers = new List<GameObject>();

    // Start spawning when the game starts
    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnBarriers());
    }

    // Coroutine to spawn barriers over time
    public IEnumerator SpawnBarriers()
    {
        // We'll start spawning at position 10 on the Z-axis
        int zPosition = 10;

        while (true)  // Infinite loop to keep spawning
        {
            // Get the X position from the looping array (0, 3, 4)
            int xPosition = spawnXPositions[xPositionIndex];

            // Calculate the spawn position
            Vector3 spawnPos = new Vector3(startPos.x + xPosition, startPos.y, startPos.z + zPosition);

            // Create the rotation Quaternion for 90 degrees on Y-axis
            Quaternion spawnRotation = Quaternion.Euler(0, rotationY, 0);

            // Instantiate the Barrier prefab at the calculated position with the 90-degree rotation on Y-axis
            GameObject newBarrier = Instantiate(barrierPrefab, spawnPos, spawnRotation);

            // Add the new barrier to the list
            spawnedBarriers.Add(newBarrier);

            // Wait for the specified spawn rate before continuing the loop
            yield return new WaitForSeconds(spawnRate);

            // Increment the Z position by 10 for the next spawn
            zPosition += spawnOffsetZ;

            // Update the X position for the next spawn (loop through 0, 3, 4)
            xPositionIndex = (xPositionIndex + 1) % spawnXPositions.Length;

            // Destroy any barriers that have been in the scene for more than 5 seconds
            //DestroyOldBarriers();
        }
    }

    // Method to destroy barriers older than 5 seconds
   
}
