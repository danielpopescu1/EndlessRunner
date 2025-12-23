using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform player;
    public float offsetZ = 10f;



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offsetZ);
    }
}
