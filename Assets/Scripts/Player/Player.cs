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


    public float rotationSpeed = 2f; // ȸ�� �ӵ�
    private Quaternion targetRotation; // ��ǥ ȸ���� ����
    public float acceleration = 0.1f;   // ���ӵ�
    public float maxSpeed = 1f;      // �ִ� �ӵ�
    public float deceleration = 2f; // ���ӵ�
    private Vector2 currentVelocity; // ���� �ӵ�

    // ���� ���� ����
    public Collider2D attackCollider; // ���ݿ� ����� Collider
    public float attackDuration = 0.2f; // ���� ���� �ð�

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
//joystick = FindObjectOfType<FloatingJoystick>();
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
        Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 rotationDir = dir;

        // ĳ���Ͱ� �����̰� ���� ���� ��� ��ũ��
        if (rb.velocity.magnitude > 0.1f)  // �ӵ� ũ�Ⱑ 0.1f���� Ŭ ��
        {
            // ĳ������ ���� ���� ���� ���
            Vector2 upDirection = transform.up;

            // ĳ���Ͱ� ���� 180�� �ݿ��� �ٶ󺸴� ���
            if (Vector2.Dot(upDirection, Vector2.up) > 0)
            {
                MoveBackground(Vector2.down); // �Ʒ��� ��ũ��
            }
            // ĳ���Ͱ� ���� 180�� �ݿ��� �ٶ󺸴� ���
            else
            {
                MoveBackground(Vector2.up); // ���� ��ũ��
            }
        }


        // ���ӵ� ����
        if (dir.magnitude > 0.1f) // ���̽�ƽ �Է��� ���� ��
        {
            currentVelocity += dir * acceleration * Time.deltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
        }
        else // ���̽�ƽ �Է��� ���� ��
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        if (rb != null)
        {
            rb.velocity = currentVelocity;
        }

        // �̵� �������� ȸ�� (ĳ������ ������ �̵� ������ ����Ŵ)
        if (rotationDir.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
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

    // �÷��̾ ȭ�� ��踦 ���� �ʵ��� ��ġ ����
    private void ClampPlayerPosition()
    {
        // ȭ�� ���� ���
        float screenHeight = Camera.main.orthographicSize * 2.0f;

        Vector3 newPosition = transform.position;

        // �¿� ��� ����
        newPosition.x = Mathf.Clamp(newPosition.x, -screenWidth / 2 + boundaryPadding, screenWidth / 2 - boundaryPadding);

        // ���� ��� ����
        newPosition.y = Mathf.Clamp(newPosition.y, -screenHeight / 2 + boundaryPadding, screenHeight / 2 + boundaryPadding); // + screenHeight / 2 �߰�

        transform.position = newPosition;
    }

    // ��� ������ �Լ� 
    private void MoveBackground(Vector2 direction)
    {
        // ĳ������ ���� ���� ���� ���
        Vector2 upDirection = transform.up;

        // ����/���� ������� ���� ���� ��� (0 ~ 1)
        float alignment = Mathf.Abs(Vector2.Dot(upDirection, Vector2.up));

        // ���� ������ ���� ��� ��ũ�� �ӵ� ����
        float scrollSpeed = playerMoveSpeed * alignment;

        background.transform.Translate(direction * scrollSpeed * Time.deltaTime);
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