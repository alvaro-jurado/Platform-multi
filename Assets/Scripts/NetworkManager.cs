using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Conectando a Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinOrCreateRoom("Sala", new Photon.Realtime.RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
    }
}
