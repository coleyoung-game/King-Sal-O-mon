using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth = 100f; // 현재 체력
    public float maxHealth = 100f;     // 최대 체력

    // HealthBar 스크립트를 할당할 필드
    [SerializeField] private HealthBar healthBar;

    // 게임 시작 시 초기화
    void Start()
    {
        // HealthBar 스크립트가 할당되어 있지 않다면, 에러를 출력
        if (healthBar == null)
        {
            Debug.LogError("HealthBar가 할당되지 않았습니다!");
        }
        else
        {
            // 시작 시 체력 바 초기화
            healthBar.UpdateHealthBar();
        }
    }

    // 체력 감소 함수
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // 체력 바 갱신
        healthBar.TakeDamage(damage);  // 체력 바에서 체력 감소 처리
    }

    // 체력 회복 함수
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // 체력 바 갱신
        healthBar.Heal(healAmount);  // 체력 바에서 체력 회복 처리
    }

    // 플레이어 사망 처리 (선택 사항)
    private void Die()
    {
        Debug.Log("Player has died.");
        // 여기서 사망 후 처리할 로직을 작성할 수 있습니다.
    }

    // 체력 업데이트 함수
    public void UpdateHealth()
    {
        healthBar.UpdateHealthBar();  // 체력 바 업데이트
    }
}