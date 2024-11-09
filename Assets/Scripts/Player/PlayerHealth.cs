using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;      // �ִ� ü��
    public float currentHealth;         // ���� ü��

    public void Start()
    {
        currentHealth = maxHealth;
    }

    // ü�� ���� �Լ�
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // ���� ü�µ� �Բ� ����
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