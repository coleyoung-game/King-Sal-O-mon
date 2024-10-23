using UnityEngine;

public class Projectile_Bouncing : ProjectileBase
{
    /*
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other); // �θ� Ŭ������ OnTriggerEnter2D ȣ��

        if (other.CompareTag("Wall")) // ���� �浹 �� ƨ��
        {
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, other.ClosestPoint(transform.position) - (Vector2)transform.position);
            rb.velocity = reflectedVelocity;
        }
    }
     */
    protected override void OnCollisionEnter2D(Collision2D collision) // 2D �Լ� ���
    {
        base.OnCollisionEnter2D(collision); // �θ� Ŭ������ OnCollisionEnter2D ȣ��

        if (collision.gameObject.CompareTag("Wall")) // ���� �浹 �� ƨ��
        {
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collision.contacts[0].point - (Vector2)transform.position); // contact points ���
            rb.velocity = reflectedVelocity;
        }
    }
}