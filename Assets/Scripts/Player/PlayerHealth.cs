using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;      // 최대 체력
    public float currentHealth;         // 현재 체력

    public void Start()
    {
        currentHealth = maxHealth;
    }

    // 체력 증가 함수
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // 현재 체력도 함께 증가
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
        Debug.Log("Player is dead!");
        
    }
}