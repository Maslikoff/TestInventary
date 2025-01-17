using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float CurrentHealth;

    [SerializeField] private Slider _healthSlider; 
    [SerializeField] private float _maxHealth = 100f; 

    void Start()
    {
        CurrentHealth = _maxHealth; 
        _healthSlider.value = CalculateSliderPercentage(CurrentHealth, _maxHealth);
    }

    /// <summary>
    /// ��������� ��������
    /// </summary>
    /// <param name="amount">damage</param>
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount; 
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth); 
        UpdateHealthBar();
    }

    /// <summary>
    /// ����������� ��������
    /// </summary>
    /// <param name="amount">HP</param>
    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// � ������ ���� ����� �����
    /// </summary>
    public void IsAlive() => gameObject.SetActive(CurrentHealth > 0);

    /// <summary>
    /// ��������� �������� Slider
    /// </summary>
    private void UpdateHealthBar() => _healthSlider.value = CalculateSliderPercentage(CurrentHealth, _maxHealth); 

    /// <summary>
    /// ����������� ��� ��������
    /// </summary>
    /// <param name="currentHealth"></param>
    /// <param name="maxHealth"></param>
    /// <returns></returns>
    private float CalculateSliderPercentage(float currentHealth, float maxHealth) => currentHealth / maxHealth;
}
