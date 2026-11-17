using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector3 offset;

    private bool isFollowingPlayer = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(isFollowingPlayer)
        {
            transform.position = offset + player.transform.position;

        }
    }

    public void FlipFollow()
    {
        isFollowingPlayer = !isFollowingPlayer;
    }
}
