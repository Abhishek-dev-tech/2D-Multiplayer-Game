using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> bulletPool;
    private List<GameObject> deathParliclesPool;
    private List<GameObject> followEnemiesPool;
    private List<GameObject> shootingEnemiesPool;
    private List<GameObject> laserEnemiesPool;

    [SerializeField] private float maxBullets;
    [SerializeField] private float maxDeathParlicles;
    [SerializeField] private float maxFollowEnemies;
    [SerializeField] private float maxShootingEnemies;
    [SerializeField] private float maxlaserEnemies;

    [Space(10)]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject deathParlicle;
    [SerializeField] private GameObject followEnemy;
    [SerializeField] private GameObject shootingEnemy;
    [SerializeField] private GameObject laserEnemy;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        bulletPool = new List<GameObject>();
        deathParliclesPool = new List<GameObject>();
        followEnemiesPool = new List<GameObject>();
        shootingEnemiesPool = new List<GameObject>();
        laserEnemiesPool = new List<GameObject>();

        InstantiateBullets();
        InstantiateDeathParticles();

        if (!PhotonNetwork.IsMasterClient)
            return;

        InstantiateFollowEnemies();
        InstantiateShootingEnemies();
        InstantiateLaserEnemies();
    }

    private void InstantiateBullets()
    {
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject _bullet = Instantiate(bullet);

            _bullet.SetActive(false);

            bulletPool.Add(_bullet);
        }
    }

    private void InstantiateDeathParticles()
    {
        for (int i = 0; i < maxDeathParlicles; i++)
        {
            GameObject _particle = Instantiate(deathParlicle);

            _particle.GetComponent<ParticleSystem>().Stop();

            deathParliclesPool.Add(_particle);
        }
    }

    private void InstantiateFollowEnemies()
    {

        for (int i = 0; i < maxFollowEnemies; i++)
        {
            GameObject _followEnemy = PhotonNetwork.Instantiate(followEnemy.name, Vector3.zero ,Quaternion.identity);

            _followEnemy.GetComponent<EnemyController>().SendInfo(false, Vector3.zero);

            followEnemiesPool.Add(_followEnemy);
        }
    }
    
    private void InstantiateShootingEnemies()
    {
        for (int i = 0; i < maxShootingEnemies; i++)
        {
            GameObject _shootingEnemy = PhotonNetwork.Instantiate(shootingEnemy.name, Vector3.zero ,Quaternion.identity);

            _shootingEnemy.GetComponent<EnemyController>().SendInfo(false, Vector3.zero);

            shootingEnemiesPool.Add(_shootingEnemy);
        }
    }

    private void InstantiateLaserEnemies()
    {
        for (int i = 0; i < maxlaserEnemies; i++)
        {
            GameObject _laserEnemy = PhotonNetwork.Instantiate(laserEnemy.name, Vector3.zero, Quaternion.identity);

            _laserEnemy.GetComponent<EnemyController>().SendInfo(false, Vector3.zero);

            laserEnemiesPool.Add(_laserEnemy);
        }
    }

    public GameObject GetBullets()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
                return bulletPool[i];
        }

        return null;
    }

    public GameObject GetDeathParticle()
    {
        for (int i = 0; i < deathParliclesPool.Count; i++)
        {
            if (deathParliclesPool[i].GetComponent<ParticleSystem>().isStopped)
                return deathParliclesPool[i];
        }

        return null;
    }

    public GameObject GetFollowEnemy()
    {
        for (int i = 0; i < followEnemiesPool.Count; i++)
        {
            if (!followEnemiesPool[i].activeInHierarchy)
                return followEnemiesPool[i];
        }

        return null;
    }

    public GameObject GetShootingEnemy()
    {
        for (int i = 0; i < shootingEnemiesPool.Count; i++)
        {
            if (!shootingEnemiesPool[i].activeInHierarchy)
                return shootingEnemiesPool[i];
        }

        return null;
    }

    public GameObject GetlaserEnemy()
    {
        for (int i = 0; i < laserEnemiesPool.Count; i++)
        {
            if (!laserEnemiesPool[i].activeInHierarchy)
                return laserEnemiesPool[i];
        }

        return null;
    }

}
