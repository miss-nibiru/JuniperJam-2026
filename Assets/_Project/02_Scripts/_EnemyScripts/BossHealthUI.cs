using UnityEngine;

/// <summary>
/// Controls the big boss HP bar on the main canvas -- should make it so i can make a more dramatic entrance i think
/// It starts hidden, then connects to the spawned boss when the boss entrance is done
/// </summary>
public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthBarObject;
    [SerializeField] private HealthBarUI healthBarUI;

    private void Awake()
    {
        if (!healthBarUI) healthBarUI = GetComponentInChildren<HealthBarUI>(true);
        if (!bossHealthBarObject && healthBarUI) bossHealthBarObject = healthBarUI.gameObject;
        Hide();
    }

    public void ShowForBoss(IHealthBars bossHealth)
    {
        if (bossHealth == null) return;
        if (bossHealthBarObject) bossHealthBarObject.SetActive(true);
        if (healthBarUI) healthBarUI.SetHealthTarget(bossHealth);
    }

    public void Hide()
    {
        if (bossHealthBarObject) bossHealthBarObject.SetActive(false);
        
    }
}