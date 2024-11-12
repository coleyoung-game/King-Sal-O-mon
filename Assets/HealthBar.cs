using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;  // ü�� �� �̹���
    [SerializeField] private Image backgroundImage; // ��� �̹��� (�ʿ� ��)
    [SerializeField] private float currentHealth = 100f;  // ���� ü��
    [SerializeField] private float maxHealth = 100f;     // �ִ� ü��

    // ���� �� �ʱ�ȭ
    void Start()
    {
        UpdateHealthBar();  // ���� �� ü�� �� �ʱ�ȭ
    }

    // ü�� �� ������Ʈ
    public void UpdateHealthBar()
    {
        // ü�� ������ 0�� 1 ������ ������ ���
        float healthPercentage = currentHealth / maxHealth;

        // ü�� ���� fillAmount�� ����
        barImage.fillAmount = healthPercentage;

        // HP�� 0�̰ų� 100�� ��, HP �� �����
        if (barImage.fillAmount == 0f || barImage.fillAmount == 1f)
        {
            HideHealthBar();  // ü�� �� �����
        }
        else
        {
            ShowHealthBar();  // ü�� �� ���̱�
        }
    }

    // ü�� �� �����
    private void HideHealthBar()
    {
        barImage.gameObject.SetActive(false);
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(false);  // ��� �̹����� ���� ���
        }
    }

    // ü�� �� ���̱�
    private void ShowHealthBar()
    {
        barImage.gameObject.SetActive(true);
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(true);  // ��� �̹����� ���̰�
        }
    }

    // ü�� ���� �Լ�
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();  // ü�� �� ����
    }

    // ü�� ȸ�� �Լ�
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();  // ü�� �� ����
    }
}
