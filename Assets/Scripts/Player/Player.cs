using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerMoveSpeed = 5f;
    private JoystickMove joystickMove;
    private Rigidbody2D rb;

    private float screenWidth;
    public float boundaryPadding = 0.1f;

    public GameObject background;
    public float backgroundMoveSpeed = 2f;

    private bool isTouchingTop = false;
    private bool isTouchingBottom = false;

    // 공격 관련 변수
    public Collider2D attackCollider; // 공격에 사용할 Collider
    public float attackDuration = 0.2f; // 공격 지속 시간

    void Start()
    {
        joystickMove = FindObjectOfType<JoystickMove>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D 확인");
        }
        else
        {
            rb.gravityScale = 0f;
        }

        screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;

        // 공격 Collider 초기 비활성화
        attackCollider.enabled = false;
    }

    void Update()
    {
        Vector2 dir = new Vector2(joystickMove.Horizontal, joystickMove.Vertical);

        // Top 또는 Bottom에 닿으면 X축 속도 감소 및 특정 방향 움직임만 허용
        if (isTouchingTop && dir.y > 0f)
        {
            dir.y = 0f;
            dir.x *= 0.5f;

            MoveBackground(Vector2.down);
        }
        else if (isTouchingBottom && dir.y < 0f)
        {
            dir.y = 0f;
            dir.x *= 0.5f;

            MoveBackground(Vector2.up);
        }

        if (rb != null)
        {
            rb.velocity = dir.normalized * playerMoveSpeed;
        }

        ClampPlayerPosition();

        // 공격 입력 처리 (예시: 스페이스바)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Top")
        {
            isTouchingTop = true;
        }
        else if (other.gameObject.name == "Bottom")
        {
            isTouchingBottom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Top")
        {
            isTouchingTop = false;
        }
        else if (other.gameObject.name == "Bottom")
        {
            isTouchingBottom = false;
        }
    }

    // 플레이어가 화면 좌우 경계를 넘지 않도록 위치 제한
    private void ClampPlayerPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, -screenWidth / 2 + boundaryPadding, screenWidth / 2 - boundaryPadding);
        transform.position = newPosition;
    }

    // 배경 움직임 함수 
    private void MoveBackground(Vector2 direction)
    {
        background.transform.Translate(direction * backgroundMoveSpeed * Time.deltaTime);
    }

    // 공격 코루틴
    IEnumerator Attack()
    {
        Debug.Log("ATTACK");
        attackCollider.enabled = true; // 공격 Collider 활성화
        yield return new WaitForSeconds(attackDuration); // 지속 시간 동안 대기
        attackCollider.enabled = false; // 공격 Collider 비활성화

        // 공격 범위 내의 적에게 데미지 적용
        Collider2D[] hitEnemies = new Collider2D[10]; // 임시, 최대 10개의 적까지 충돌 감지 
        int numEnemiesHit = Physics2D.OverlapCollider(attackCollider, new ContactFilter2D(), hitEnemies); // 충돌 결과 저장

        for (int i = 0; i < numEnemiesHit; i++)
        {
            EnemyHealth enemyHealth = hitEnemies[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20f); // 임시, 20 데미지 적용
            }
        }
    }
}