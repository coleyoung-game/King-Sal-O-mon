using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 1f;          // 투사체 속도
    public float lifetime = 4f;        // 투사체 수명 (초)
    protected Rigidbody2D rb;          // Rigidbody2D 컴포넌트
    public GameObject player;

    protected virtual void OnEnable()
    {
        player = GameObject.Find("Salmon");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        SetInitialVelocity();
        StartCoroutine(DisableAfterLifetime());
    }

    // lifetime 이후에 비활성화하는 코루틴
    private IEnumerator DisableAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    // 초기 속도 설정 함수 (자식 클래스에서 재정의)
    protected virtual void SetInitialVelocity()
    {
        // 기본적으로 아무 동작도 하지 않습니다. 자식 클래스에서 재정의해야 합니다. 
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 적용
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f); // 예시: 10 데미지 적용
            }

            ProjectilePool.Instance.ReturnProjectile(gameObject);
        }
    }
}