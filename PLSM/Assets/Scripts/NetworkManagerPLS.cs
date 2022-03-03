using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManagerPLS : NetworkManager
{
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;
    public Transform scoreSpawn;

    public GameObject BrickPrefab;
    public GameObject ScorePrefab;

    private List<GameObject> bricks;
    private GameObject score;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        score = Instantiate(ScorePrefab, scoreSpawn.position, scoreSpawn.rotation);
        ScoreController scoreController = score.GetComponent<ScoreController>();
        scoreController.score = 0;

        NetworkServer.Spawn(score);

        bricks = new List<GameObject>();
        CreateBricks(50);
    }

    public override void OnStopServer()
    {
        foreach (GameObject brick in bricks)
        {
            NetworkServer.Destroy(brick);
        }

        bricks.Clear();

        if (score != null)
        {
            NetworkServer.Destroy(score);
        }

        base.OnStopServer();
    }
    
    private void CreateBricks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject BrickGO = Instantiate(BrickPrefab);
            
            Brick brick = BrickGO.GetComponent<Brick>();

            Color col = new Color(1f, 1f, 1f);

            if (i < 10)
            {
                col = new Color(Random.Range(0.6f, 1f), 0f, 0f);
            }
            else if (i < 20)
            {
                col = new Color(1f, Random.Range(0.4f, 0.6f), 0f);
            }
            else if (i < 30)
            {
                col = new Color(1f, 1f, Random.Range(0f, 0.5f));
            }
            else if (i < 40)
            {
                col = new Color(0f, Random.Range(0.4f, 1f), 0f);
            }
            else if (i < 50)
            {
                col = new Color(0f, 0f, Random.Range(0.5f, 1f));
            }

            brick.scoreController = score.GetComponent<ScoreController>();
            brick.alive = true;
            brick.color = col;
            
            NetworkServer.Spawn(BrickGO);

            bricks.Add(BrickGO);
        }
    }
}

