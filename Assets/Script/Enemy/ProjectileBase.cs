using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 1f;          // 투사체 속도
    public float lifetime = 4f;        // 투사체 수명 (초)
    public GameObject player;         // 플레이어 오브젝트
    protected Rigidbody2D rb;          // Rigidbody2D 컴포넌트 (protected로 변경)


    protected virtual void OnEnable() // Start() 대신 OnEnable() 사용
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;

        if (player == null)
            player = GameObject.Find("Salmon");

        SetInitialVelocity();

        StartCoroutine(DisableAfterLifetime());
    }

    // lifetime 이후에 비활성화하는 코루틴
    private IEnumerator DisableAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        // 풀에 반환하고 비활성화
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    // 초기 속도 설정 함수 (자식 클래스에서 재정의)
    protected virtual void SetInitialVelocity()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    /*
   protected virtual void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("Player"))
       {
           Debug.Log("플레이어에게 히트!");
           // 플레이어에게 데미지를 주는 등의 로직 추가

           // 풀에 탄환 반환
           ProjectilePool.Instance.ReturnProjectile(gameObject);
       }
   }
    */

    protected virtual void OnCollisionEnter2D(Collision2D collision) // Collision2D 타입 사용
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 히트!");
            // 플레이어에게 데미지를 주는 등의 로직 추가

            ProjectilePool.Instance.ReturnProjectile(gameObject);
        }
    }
}