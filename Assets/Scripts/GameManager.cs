using Photon.Pun;
using UnityEngine;

public enum GameState
{
    Preparing,
    Playing,
    GameOver
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameState gameState;

    [SerializeField]
    private GameObject player;

    [Space(10)]
    public Vector2 min, max;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        gameState = GameState.Preparing;
    }

    private void SpawnPlayer()
    {
        Vector2 randomPos = new Vector2 (Random.Range(min.x, max.x), Random.Range(min.y, max.y));

        PhotonNetwork.Instantiate(player.name, randomPos, Quaternion.identity);
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
    }

    public GameState GameState { 
        
        get { return gameState; }

        set 
        { 
            gameState = value;

            if (gameState == GameState.Playing)
                SpawnPlayer();
        }
    }
}