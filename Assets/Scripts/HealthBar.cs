using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float maxHealth; // PlayerHealth ��ũ��Ʈ���� maxHealth �������� ���� public���� ����
    private float currentHealth;

    private PlayerHealth playerHealth; // PlayerHealth ������Ʈ ����

    void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>(); // PlayerHealth ������Ʈ ��������
        if (playerHealth != null)
        {
            maxHealth = playerHealth.maxHealth;
            currentHealth = playerHealth.currentHealth;
            SetHealth(currentHealth);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found!");
        }
    }


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0f, maxHealth);
        slider.value = currentHealth;
    }



    void Update()
    {
        if (playerHealth != null)
        {
            SetHealth(playerHealth.currentHealth);

            if (maxHealth != playerHealth.maxHealth)
            {
                maxHealth = playerHealth.maxHealth;
                SetMaxHealth(maxHealth);
            }

        }
    }

}