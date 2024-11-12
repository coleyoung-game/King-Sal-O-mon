using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth = 100f; // ���� ü��
    public float maxHealth = 100f;     // �ִ� ü��

    // HealthBar ��ũ��Ʈ�� �Ҵ��� �ʵ�
    [SerializeField] private HealthBar healthBar;

    // ���� ���� �� �ʱ�ȭ
    void Start()
    {
        // HealthBar ��ũ��Ʈ�� �Ҵ�Ǿ� ���� �ʴٸ�, ������ ���
        if (healthBar == null)
        {
            Debug.LogError("HealthBar�� �Ҵ���� �ʾҽ��ϴ�!");
        }
        else
        {
            // ���� �� ü�� �� �ʱ�ȭ
            healthBar.UpdateHealthBar();
        }
    }

    // ü�� ���� �Լ�
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // ü�� �� ����
        healthBar.TakeDamage(damage);  // ü�� �ٿ��� ü�� ���� ó��
    }

    // ü�� ȸ�� �Լ�
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // ü�� �� ����
        healthBar.Heal(healAmount);  // ü�� �ٿ��� ü�� ȸ�� ó��
    }

    // �÷��̾� ��� ó�� (���� ����)
    private void Die()
    {
        Debug.Log("Player has died.");
        // ���⼭ ��� �� ó���� ������ �ۼ��� �� �ֽ��ϴ�.
    }

    // ü�� ������Ʈ �Լ�
    public void UpdateHealth()
    {
        healthBar.UpdateHealthBar();  // ü�� �� ������Ʈ
    }
}