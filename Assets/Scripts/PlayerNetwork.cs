using UnityEngine;
using Photon.Pun;

public class PlayerNetwork : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            Destroy(GetComponent<PlayerMovement>());
            return;
        }
    }
}
