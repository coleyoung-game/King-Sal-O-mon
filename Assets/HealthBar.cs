using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;  // 체력 바 이미지
    [SerializeField] private Image backgroundImage; // 배경 이미지 (필요 시)
    [SerializeField] private float currentHealth = 100f;  // 현재 체력
    [SerializeField] private float maxHealth = 100f;     // 최대 체력

    // 시작 시 초기화
    void Start()
    {
        UpdateHealthBar();  // 시작 시 체력 바 초기화
    }

    // 체력 바 업데이트
    public void UpdateHealthBar()
    {
        // 체력 비율을 0과 1 사이의 값으로 계산
        float healthPercentage = currentHealth / maxHealth;

        // 체력 바의 fillAmount를 변경
        barImage.fillAmount = healthPercentage;

        // HP가 0이거나 100일 때, HP 바 숨기기
        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f)
        {
            HideHealthBar();  // 체력 바 숨기기
        }
        else
        {
            ShowHealthBar();  // 체력 바 보이기
        }
    }

    // 체력 바 숨기기
    private void HideHealthBar()
    {
        barImage.gameObject.SetActive(false);
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(false);  // 배경 이미지도 숨길 경우
        }
    }

    // 체력 바 보이기
    private void ShowHealthBar()
    {
        barImage.gameObject.SetActive(true);
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(true);  // 배경 이미지도 보이게
        }
    }

    // 체력 감소 함수
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();  // 체력 바 갱신
    }

    // 체력 회복 함수
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();  // 체력 바 갱신
    }
}
