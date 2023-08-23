using UnityEngine;
using Photon.Pun;

public class FollowEnemy : EnemyController
{
    private void Update()
    {
        target = GetClosestTarget();

        Follow();
    }

    public override void Follow()
    {
        transform.position += (Vector3)GetDirection(target) * moveSpeed * Time.deltaTime;
    }

    public override void Attack()
    {

    }
    
    public override void TakeDamage(float value)
    {
        Health -= value;

        spriteRenderer.color = Color.white;

        popUp.Popup();

        Invoke("ResetColor", 0.075f);

        if (Health <= 0)  Die();
    }

    private void ResetColor()
    {
        spriteRenderer.color = new Color(1f, 0.25f, 0.25f);
    }
}
