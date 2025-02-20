using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject coinPrefab;
    public Transform[] spawnPoints;
    public TextMeshProUGUI scoreText;

    private const string SCORE_KEY = "TotalScore";
    private int score = 0;
    public float spawnInterval = 2f;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnCoinsContinuously());
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(SCORE_KEY))
        {
            score = (int)PhotonNetwork.CurrentRoom.CustomProperties[SCORE_KEY];
            UpdateScoreText();
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(SCORE_KEY))
        {
            score = (int)PhotonNetwork.CurrentRoom.CustomProperties[SCORE_KEY];
            UpdateScoreText();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnCoinsContinuously());
        }
        else
        {
            // If not MasterClient, ask for score
            photonView.RPC("RequestScoreSync", RpcTarget.MasterClient);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // New player, MasterClient sends actual score
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncScore", newPlayer, score);
        }
    }

    IEnumerator SpawnCoinsContinuously()
    {
        while (true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnCoin();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCoin()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        if (!Physics2D.OverlapCircle(randomSpawnPoint.position, 0.1f))
        {
            PhotonNetwork.Instantiate(coinPrefab.name, randomSpawnPoint.position, Quaternion.identity);
        }
    }

    public void UpdateScoreForAll(int amount)
    {
        score += amount;
        photonView.RPC("SyncScore", RpcTarget.All, score);
    }

    [PunRPC]
    public void SyncScore(int newScore)
    {
        score = newScore;
        UpdateScoreText();
    }

    [PunRPC]
    void RequestScoreSync()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncScore", RpcTarget.Others, score);
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
