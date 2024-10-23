using System.Collections;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 1f;          // ����ü �ӵ�
    public float lifetime = 4f;        // ����ü ���� (��)
    public GameObject player;         // �÷��̾� ������Ʈ
    protected Rigidbody2D rb;          // Rigidbody2D ������Ʈ (protected�� ����)


    protected virtual void OnEnable() // Start() ��� OnEnable() ���
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;

        if (player == null)
            player = GameObject.Find("Salmon");

        SetInitialVelocity();

        StartCoroutine(DisableAfterLifetime());
    }

    // lifetime ���Ŀ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator DisableAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        // Ǯ�� ��ȯ�ϰ� ��Ȱ��ȭ
        ProjectilePool.Instance.ReturnProjectile(gameObject);
    }

    // �ʱ� �ӵ� ���� �Լ� (�ڽ� Ŭ�������� ������)
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
           Debug.Log("�÷��̾�� ��Ʈ!");
           // �÷��̾�� �������� �ִ� ���� ���� �߰�

           // Ǯ�� źȯ ��ȯ
           ProjectilePool.Instance.ReturnProjectile(gameObject);
       }
   }
    */

    protected virtual void OnCollisionEnter2D(Collision2D collision) // Collision2D Ÿ�� ���
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� ��Ʈ!");
            // �÷��̾�� �������� �ִ� ���� ���� �߰�

            ProjectilePool.Instance.ReturnProjectile(gameObject);
        }
    }
}