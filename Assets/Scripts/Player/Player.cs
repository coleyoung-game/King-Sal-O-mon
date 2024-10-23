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

    // ���� ���� ����
    public Collider2D attackCollider; // ���ݿ� ����� Collider
    public float attackDuration = 0.2f; // ���� ���� �ð�

    void Start()
    {
        joystickMove = FindObjectOfType<JoystickMove>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D Ȯ��");
        }
        else
        {
            rb.gravityScale = 0f;
        }

        screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;

        // ���� Collider �ʱ� ��Ȱ��ȭ
        attackCollider.enabled = false;
    }

    void Update()
    {
        Vector2 dir = new Vector2(joystickMove.Horizontal, joystickMove.Vertical);

        // Top �Ǵ� Bottom�� ������ X�� �ӵ� ���� �� Ư�� ���� �����Ӹ� ���
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

        // ���� �Է� ó�� (����: �����̽���)
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

    // �÷��̾ ȭ�� �¿� ��踦 ���� �ʵ��� ��ġ ����
    private void ClampPlayerPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, -screenWidth / 2 + boundaryPadding, screenWidth / 2 - boundaryPadding);
        transform.position = newPosition;
    }

    // ��� ������ �Լ� 
    private void MoveBackground(Vector2 direction)
    {
        background.transform.Translate(direction * backgroundMoveSpeed * Time.deltaTime);
    }

    // ���� �ڷ�ƾ
    IEnumerator Attack()
    {
        Debug.Log("ATTACK");
        attackCollider.enabled = true; // ���� Collider Ȱ��ȭ
        yield return new WaitForSeconds(attackDuration); // ���� �ð� ���� ���
        attackCollider.enabled = false; // ���� Collider ��Ȱ��ȭ

        // ���� ���� ���� ������ ������ ����
        Collider2D[] hitEnemies = new Collider2D[10]; // �ӽ�, �ִ� 10���� ������ �浹 ���� 
        int numEnemiesHit = Physics2D.OverlapCollider(attackCollider, new ContactFilter2D(), hitEnemies); // �浹 ��� ����

        for (int i = 0; i < numEnemiesHit; i++)
        {
            EnemyHealth enemyHealth = hitEnemies[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20f); // �ӽ�, 20 ������ ����
            }
        }
    }
}