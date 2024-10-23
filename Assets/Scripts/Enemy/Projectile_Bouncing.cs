using UnityEngine;

public class Projectile_Bouncing : ProjectileBase
{
    private Camera mainCamera; // 메인 카메라 참조
    private Vector2 screenBounds; // 화면 경계

    protected override void OnEnable() // OnEnable() 함수에서 초기화
    {
        base.OnEnable(); // 부모 클래스의 OnEnable() 함수 호출

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
        // 화면 경계를 넘어가면 속도 반전
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