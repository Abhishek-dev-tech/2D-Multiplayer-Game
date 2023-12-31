using Photon.Pun;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector2 moveDirection;

    [SerializeField, Space(10)]
    private float bulletSpeed;

    [SerializeField, Range(0f, 10f)]
    private float bulletSpread;

    [SerializeField, Range(0f, 10f)]
    private float rotateAngle;

    [SerializeField]
    public float maxHealth;

    [HideInInspector]
    public PopUp popUp;

    private Camera cam;

    private PhotonView view;

    private float m_health;

    private CameraShake cameraShake;

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

    private void Start()
    {
        cam =  Camera.main;
        view = GetComponent<PhotonView>();
        popUp = GetComponent<PopUp>();

        cameraShake = GameManager.instance.GetCameraShake();

        m_health = maxHealth;
    }

    private void Update()
    {
        Move();
        UserInput();

    }

    private void UserInput()
    {
        if(view.IsMine && GameManager.instance.GameState == GameState.Playing)
        {
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetMouseButtonDown(0))
                Shoot();
        }
    }

    private void Move()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, -moveDirection.x * rotateAngle);
    }

    private void Shoot()
    {
        float desiredBulletSpread = UnityEngine.Random.Range(-bulletSpread, bulletSpread);

        cameraShake.TriggerShake(0.15f, 0.15f);

        view.RPC("RPC_Shoot", RpcTarget.All, GetBulletAngle(), desiredBulletSpread);
    }

    [PunRPC]
    private void RPC_Shoot(float bulletAngle, float _desiredBulletSpread)
    {
        GameObject tempBullet = ObjectPool.instance.GetBullets();

        if (!tempBullet) return;

        tempBullet.GetComponent<Bullet>().bulletParent = BulletParent.Player;

        popUp.Popup();

        AudioManager.instance.PlaySoundEffect("Shooting");

        tempBullet.transform.position = transform.position;

        tempBullet.SetActive(true);

        tempBullet.GetComponent<Bullet>().Init(bulletSpeed, _desiredBulletSpread, bulletAngle);
        
    }

    public void TakeDamage(float value)
    {
        Health -= value;

        if(view.IsMine)
            UIManager.instance.UpdateHealthBar(Health);

        popUp.Popup();

        if (Health <= 0) Die();
        else AudioManager.instance.PlaySoundEffect("Hit");
    }

    private void Die()
    {
        view.RPC("RPC_Die", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Die()
    {
        AudioManager.instance.PlaySoundEffect("Die");

        GameManager.instance.GameOver();
        UIManager.instance.GameOver();
    }

    private float GetBulletAngle()
    {
        Vector3 lookPos = GetBulletDirection();

        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

        return angle;
    }

    private Vector3 GetBulletDirection()
    {
        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition);

        dir = dir - transform.position;
        dir.Normalize();

        return dir;
    }
}
