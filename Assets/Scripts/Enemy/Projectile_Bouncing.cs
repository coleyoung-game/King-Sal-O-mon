using UnityEngine;

public class Projectile_Bouncing : ProjectileBase
{
    private Camera mainCamera; // ���� ī�޶� ����
    private Vector2 screenBounds; // ȭ�� ���

    protected override void OnEnable() // OnEnable() �Լ����� �ʱ�ȭ
    {
        base.OnEnable(); // �θ� Ŭ������ OnEnable() �Լ� ȣ��

        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    protected override void SetInitialVelocity()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        // ȭ�� ��踦 �Ѿ�� �ӵ� ����
        if (transform.position.x > screenBounds.x || transform.position.x < -screenBounds.x)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
        if (transform.position.y > screenBounds.y || transform.position.y < -screenBounds.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }
}