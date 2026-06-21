using UnityEngine;

public class RunStateManager : MonoBehaviour
{
    private RunBaseState currentRunState;

    public SpinState SpinRunState = new SpinState();
    public CombatState CombatRunState = new CombatState();
    public EndState EndRunState = new EndState();

    public SpinningWheel spinningWheel;

    public bool finishedSpinning;


    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private UIManager uiManager;
    public GunController gunController;
    [SerializeField] private EnemyManager enemyManager;

    private void OnEnable()
    {
        spinningWheel.SpinFinished += HandleSpinFinished;
    }

    private void OnDisable()
    {
        spinningWheel.SpinFinished -= HandleSpinFinished;
    }

    void Start()
    {
        currentRunState = SpinRunState;

        currentRunState.EnterState(this);
    }

    void Update()
    {
        currentRunState.UpdateState(this);

        if (finishedSpinning)
        {
            SwitchState(CombatRunState);
            finishedSpinning = false;
        }
    }

    public void SwitchState(RunBaseState newState)
    {
        currentRunState.ExitState(this);
        currentRunState = newState;
        currentRunState.EnterState(this);
    }

    public void SetCombatSystemsEnabled(bool enabled)
    {
        UIManager.Instance.ShowSpinPanel(!enabled);
        cursorManager?.SetShootingCursorEnabled(enabled);
        gunController?.SetShootingEnabled(enabled);
        enemyManager?.RemoveAllEnemies();
    }

    void HandleSpinFinished(SpinningWheel.WeaponChoice weaponChoice)
    {
        gunController.SelectWeapon(weaponChoice);
        SwitchState(CombatRunState);
    }
}
