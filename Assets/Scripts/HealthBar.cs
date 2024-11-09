using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float maxHealth; // PlayerHealth 스크립트에서 maxHealth 가져오기 위해 public으로 변경
    private float currentHealth;

    private PlayerHealth playerHealth; // PlayerHealth 컴포넌트 참조

    void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>(); // PlayerHealth 컴포넌트 가져오기
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