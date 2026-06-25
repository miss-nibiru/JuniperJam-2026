using System;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// this script controls the entirety of the queen entrace - starts completely deactivated, then
/// activates the timeline and checks when it ended
/// </summary>
public class BossDramaQueenEntrance : MonoBehaviour
{
    [Header("Boss Entrance Sequence")]
    [SerializeField] private PlayableDirector bossIntroTimeline;

    [Header("Mama Queen")]
    [SerializeField] private GameObject finalBossRoot;
    [SerializeField] private EnemyController bossController;
    [SerializeField] private EnemyData bossData;
    [SerializeField] private Collider2D[] bossHitboxes;
    [SerializeField] private EnemyShooterController[] bossShooters;
    
    [SerializeField] private RunStateManager runStateManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private Transform playerTarget;

    [Header("UI")]
    [SerializeField] private BossHealthBarUI bossHealthBarUI;

    private Action _onIntroFinished;
    private bool _introIsPlaying;
    private bool _introFinished;

    private void Awake() //everything is off until timeline starts it up
    {
        if (!bossIntroTimeline) bossIntroTimeline = GetComponent<PlayableDirector>();
        if (!bossController && finalBossRoot) bossController = finalBossRoot.GetComponent<EnemyController>();
        if (!bossHealthBarUI) bossHealthBarUI = FindFirstObjectByType<BossHealthBarUI>();
        if (!runStateManager) runStateManager = FindFirstObjectByType<RunStateManager>();
        if (!enemyManager) enemyManager = FindFirstObjectByType<EnemyManager>();
        if (!mainGrid) mainGrid = FindFirstObjectByType<MainGridManager>();

        if (finalBossRoot)
        {
            bossShooters = finalBossRoot.GetComponentsInChildren<EnemyShooterController>(true);
            bossHitboxes = finalBossRoot.GetComponentsInChildren<Collider2D>(true);
        }
    }

    private void Start()
    {
        PrepareBossForIntro();
    }

    public void PlayBossIntro(Action onIntroFinished)
    {
        if (_introFinished)
        {
            onIntroFinished?.Invoke();
            return;
        }

        _onIntroFinished = onIntroFinished;
        _introIsPlaying = true;

        PrepareBossForIntro();

        if (bossIntroTimeline)
        {
            bossIntroTimeline.stopped += HandleTimelineStopped;
            bossIntroTimeline.Play();
        }
        else
        {
            FinishBossIntro();
        }
    }

    private void PrepareBossForIntro()
    {
        if (finalBossRoot) finalBossRoot.SetActive(true);
        if (bossHealthBarUI) bossHealthBarUI.Hide();

        foreach (Collider2D hitbox in bossHitboxes)
        {
            if (!hitbox) continue;
            hitbox.enabled = false;
        }

        foreach (EnemyShooterController shooter in bossShooters)
        {
            if (!shooter) continue;
            shooter.enabled = false;
        }

        runStateManager?.SetCombatSystemsEnabled(false);
    }

    private void HandleTimelineStopped(PlayableDirector director)
    {
        if (director != bossIntroTimeline) return;

        bossIntroTimeline.stopped -= HandleTimelineStopped;
        FinishBossIntro();
    }

    private void FinishBossIntro()
    {
        if (!_introIsPlaying) return;

        _introIsPlaying = false;
        _introFinished = true;

        if (bossController && bossData && mainGrid && playerTarget)
        {
            bossController.InitializeEnemy(bossData, mainGrid, playerTarget);
        }

        if (enemyManager && bossController)
        {
            enemyManager.DetectEnemy(bossController);
        }

        foreach (Collider2D hitbox in bossHitboxes)
        {
            if (!hitbox) continue;
            hitbox.enabled = true;
        }

        foreach (EnemyShooterController shooter in bossShooters)
        {
            if (!shooter) continue;

            shooter.enabled = true;
            shooter.InitializeShooter(mainGrid, playerTarget);
        }

        if (bossHealthBarUI && bossController)
        {
            bossHealthBarUI.ShowForBoss(bossController);
        }

        runStateManager?.SetCombatSystemsEnabled(true);

        _onIntroFinished?.Invoke();
        _onIntroFinished = null;
    }

}
