using Photon.Pun;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float maxHealth;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public PlayerController[] players;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    [HideInInspector]
    public PhotonView view;

    [HideInInspector]
    public PopUp popUp;

    private float m_health;

    private int once;

    public float Health
    {
        get 
        { 
            return m_health;
        }
        set 
        { 
            m_health = value;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Health = maxHealth;
        view = GetComponent<PhotonView>();
        popUp = GetComponent<PopUp>();

        once = 0;
    }

    private void Start()
    {
        players = FindObjectsOfType<PlayerController>();

        target = GetClosestTarget();
    }

    public void Die()
    {
        view.RPC("RPC_Die", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Die()
    {
        if (once >= 1)
            return;

        once++;

        gameObject.SetActive(false);
        UIManager.instance.AddKill();

        GameObject deathParticle = ObjectPool.instance.GetDeathParticle();

        if (deathParticle)
        {
            deathParticle.transform.position = transform.position;
            deathParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    public abstract void Follow();

    public abstract void Attack();

    public abstract void TakeDamage(float value);

    public Vector2 GetDirection(GameObject _target)
    {
        Vector2 direction = _target.transform.position - transform.position;
        direction.Normalize();

        return direction;
    }

    public float GetDistance(GameObject _target)
    {
        float distance = Vector2.Distance(transform.position, _target.transform.position);

        return distance;
    }

    public GameObject GetClosestTarget()
    {
        if (players.Length < 2)
            return players[0].gameObject;

        float dis_1 = Vector2.Distance(transform.position, players[0].transform.position);
        float dis_2 = Vector2.Distance(transform.position, players[1].transform.position);

        if (dis_1 < dis_2)
            return players[0].gameObject;
        else
            return players[1].gameObject;
    }

    public void SendInfo(bool active, Vector3 pos)
    {
        view.RPC("RPC_SendInfo", RpcTarget.AllBuffered, active, pos);
    }

    [PunRPC]
    public void RPC_SendInfo(bool active, Vector3 pos)
    {
        gameObject.SetActive(active);
        transform.position = pos;
    }

    public void ResetInfo()
    {
        view.RPC("RPC_ResetInfo", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_ResetInfo()
    {
        Health = maxHealth;
        once = 0;
    }
}
