using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("RequestCollectCoin", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void RequestCollectCoin()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<GameManager>().UpdateScoreForAll(1);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
