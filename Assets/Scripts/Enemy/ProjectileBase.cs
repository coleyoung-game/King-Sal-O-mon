using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 1f;          // ����ü �ӵ�
    public float lifetime = 4f;        // ����ü ���� (��)
    protected Rigidbody2D rb;          // Rigidbody2D ������Ʈ
    public GameObject player;

    protected virtual void OnEnable()
    {
        player = GameObject.Find("Salmon");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        SetInitialVelocity();
        StartCoroutine(DisableAfterLifetime());
    }

    // lifetime ���Ŀ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator DisableAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    // �ʱ� �ӵ� ���� �Լ� (�ڽ� Ŭ�������� ������)
    protected virtual void SetInitialVelocity()
    {
        // �⺻������ �ƹ� ���۵� ���� �ʽ��ϴ�. �ڽ� Ŭ�������� �������ؾ� �մϴ�. 
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� ������ ����
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f); // ����: 10 ������ ����
            }

            ProjectilePool.Instance.ReturnProjectile(gameObject);
        }
    }
}