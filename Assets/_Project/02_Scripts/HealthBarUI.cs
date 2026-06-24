using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this is connected to player hp, boss hp and all enemies - we just need those to call this in
/// </summary>

public class HealthBarUI : MonoBehaviour
{
    
    [SerializeField]  private Image heartsBar;
    [SerializeField] private MonoBehaviour healthTarget; // this makes it so i can assingn scripts as targets -- would work on enemy data i belive
    
    private IHealthBars _healthTarget;
    
    private void  Awake()
    {
        _healthTarget = healthTarget as IHealthBars;
        if (_healthTarget == null) Debug.LogError("HealthBarUI: Health target is null");
        
    }

    private void OnEnable()
    {
        if (_healthTarget == null) Debug.LogError("HealthBarUI: Health target is null");
        if (_healthTarget != null) _healthTarget.HealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(heartsBar == null) return;
        if(maxHealth <= 0) return;
        float currentHealthPercentage = (float)currentHealth / maxHealth;
        heartsBar.fillAmount = currentHealthPercentage;
        
    }
    


}
