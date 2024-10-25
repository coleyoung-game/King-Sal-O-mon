using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Player : MonoBehaviour
{
    public float playerMoveSpeed = 0.2f;
    private Joystick joystick;
    //private FloatingJoystick joystick;
    private Rigidbody2D rb;

    private float screenWidth;
    public float boundaryPadding = 0.1f;

    public GameObject background;

    private bool isTouchingTop = false;
    private bool isTouchingBottom = false;


    public float rotationSpeed = 2f; // 회전 속도
    private Quaternion targetRotation; // 목표 회전값 저장
    public float acceleration = 0.1f;   // 가속도
    public float maxSpeed = 1f;      // 최대 속도
    public float deceleration = 2f; // 감속도
    private Vector2 currentVelocity; // 현재 속도

    // 공격 관련 변수
    public Collider2D attackCollider; // 공격에 사용할 Collider
    public float attackDuration = 0.2f; // 공격 지속 시간

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
//joystick = FindObjectOfType<FloatingJoystick>();
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
        Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 rotationDir = dir;

        // Top 또는 Bottom에 닿으면 X축 속도 감소 및 특정 방향 움직임만 허용
        if (isTouchingTop && rb.velocity.y > 0.1f)
        //if (isTouchingTop && dir.y > 0f)
        {
            dir.y = 0f;
            dir.x *= 0.5f;

            MoveBackground(Vector2.down);
        }
        else if (isTouchingBottom && rb.velocity.y < -0.1f)
        //else if (isTouchingBottom && dir.y < 0f)
        {
            dir.y = 0f;
            dir.x *= 0.5f;

            MoveBackground(Vector2.up);
        }

        // 가속도 적용
        if (dir.magnitude > 0.1f) // 조이스틱 입력이 있을 때
        {
            currentVelocity += dir * acceleration * Time.deltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
        }
        else // 조이스틱 입력이 없을 때
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        if (rb != null)
        {
            rb.velocity = currentVelocity;
        }

        // 이동 방향으로 회전 (캐릭터의 위쪽이 이동 방향을 가리킴)
        if (rotationDir.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
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
        background.transform.Translate(direction * playerMoveSpeed * Time.deltaTime);
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