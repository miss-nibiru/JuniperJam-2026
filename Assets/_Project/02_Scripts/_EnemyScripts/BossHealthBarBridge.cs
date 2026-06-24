using System.Collections;
using UnityEngine;

/// <summary>
/// connects the spawned boss to the big boss HP bar
/// </summary>
public class BossHealthBarConnector : MonoBehaviour
{
    [SerializeField] private EnemyController bossHealth;
    [SerializeField] private BossHealthBarUI bossHealthBarUI;
    [SerializeField] private float showDelay = 1.5f;

    private bool _hasShown;

    private void Awake()
    {
        if (!bossHealth) bossHealth = GetComponent<EnemyController>();
        if (!bossHealthBarUI) bossHealthBarUI = FindFirstObjectByType<BossHealthBarUI>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(showDelay);
        ShowBossHealthBar();
    }

    public void ShowBossHealthBar()
    {
        if (_hasShown) return;
        if (!bossHealth) return;
        if (!bossHealthBarUI) return;

        _hasShown = true;
        bossHealthBarUI.ShowForBoss(bossHealth);
    }

    private void OnDestroy()
    {
        if (bossHealthBarUI) bossHealthBarUI.Hide();
        
    }
}