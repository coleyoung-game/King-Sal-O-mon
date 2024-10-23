using UnityEngine;

public class Projectile_Bouncing : ProjectileBase
{
    /*
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other); // 부모 클래스의 OnTriggerEnter2D 호출

        if (other.CompareTag("Wall")) // 벽과 충돌 시 튕김
        {
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, other.ClosestPoint(transform.position) - (Vector2)transform.position);
            rb.velocity = reflectedVelocity;
        }
    }
     */
    protected override void OnCollisionEnter2D(Collision2D collision) // 2D 함수 사용
    {
        base.OnCollisionEnter2D(collision); // 부모 클래스의 OnCollisionEnter2D 호출

        if (collision.gameObject.CompareTag("Wall")) // 벽과 충돌 시 튕김
        {
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collision.contacts[0].point - (Vector2)transform.position); // contact points 사용
            rb.velocity = reflectedVelocity;
        }
    }
}