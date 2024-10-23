using UnityEngine;

public class Projectile_Normal : ProjectileBase
{
    protected override void SetInitialVelocity()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}