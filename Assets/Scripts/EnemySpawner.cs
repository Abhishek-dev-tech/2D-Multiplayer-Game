using Photon.Pun;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float maxTime;
    private float timer;

    [Space(10)]
    [SerializeField]
    private GameObject[] spawnPoints;


    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient || PhotonNetwork.CurrentRoom.PlayerCount != 2)
            return;

        if (GameManager.instance.GameState != GameState.Playing)
            return;

         SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if(timer > maxTime)
        {
            GameObject _enemy;

            float rand = Random.value;

            if (rand <= 0.66f)
                _enemy = ObjectPool.instance.GetFollowEnemy();
            else if(rand > 0.66f && rand <= 0.90f)
                _enemy = ObjectPool.instance.GetShootingEnemy();
            else
                _enemy = ObjectPool.instance.GetlaserEnemy();


            if (!_enemy)
                return;

            Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;

            _enemy.GetComponent<EnemyController>().SendInfo(true, pos);
            _enemy.GetComponent<EnemyController>().ResetInfo();

            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
