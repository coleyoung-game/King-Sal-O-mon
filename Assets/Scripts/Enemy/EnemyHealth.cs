using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;      // ���� �ִ� ü�� (�÷��̾�� �ٸ��� ����)
    public float currentHealth;         // ���� ü��

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy is dead!");
        //Destroy(gameObject); // �Ǵ� �ٸ� ��� ó�� ����
    }
}