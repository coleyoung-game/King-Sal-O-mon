using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;      // 적의 최대 체력 (플레이어와 다르게 설정)
    public float currentHealth;         // 현재 체력

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
        //Destroy(gameObject); // 또는 다른 사망 처리 로직
    }
}