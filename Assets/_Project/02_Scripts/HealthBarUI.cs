using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Works for player, enemies, and boss as long as the target uses IHealthBars.
/// </summary>

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image heartsBar;
    [SerializeField] private MonoBehaviour healthTarget;

    private IHealthBars _healthTarget;
    private bool _isSubscribed;

    private void Awake()
    {
        if (healthTarget) _healthTarget = healthTarget as IHealthBars;
        
    }

    private void OnEnable()
    {
        SubscribeToTarget();
        RefreshBar();
    }

    private void OnDisable()
    {
        UnsubscribeFromTarget();
    }

    public void SetHealthTarget(IHealthBars newHealthTarget)
    {
        UnsubscribeFromTarget();
        _healthTarget = newHealthTarget;
        SubscribeToTarget();
        RefreshBar();
    }

    private void SubscribeToTarget()
    {
        if (_healthTarget == null) return;
        if (_isSubscribed) return;
        _healthTarget.HealthChanged += UpdateHealthBar;
        _isSubscribed = true;
    }

    private void UnsubscribeFromTarget()
    {
        if (_healthTarget == null) return;
        if (!_isSubscribed) return;
        _healthTarget.HealthChanged -= UpdateHealthBar;
        _isSubscribed = false;
    }

    private void RefreshBar()
    {
        if (_healthTarget == null) return;
        UpdateHealthBar(_healthTarget.CurrentHealth, _healthTarget.MaxHealth);
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (heartsBar == null) return;
        if (maxHealth <= 0) return;
        heartsBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
