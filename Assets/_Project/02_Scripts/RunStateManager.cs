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
    [SerializeField] private GunController gunController;
    [SerializeField] private EnemyManager enemyManager;
    
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

        }
    }

    public void SwitchState(RunBaseState state)
    {
        state.ExitState(this);
        currentRunState = state;
        state.EnterState(this);
    }

    public void SetCombatSystemsEnabled(bool enabled)
    {
        UIManager.Instance.ShowSpinPanel(!enabled);
        cursorManager?.SetShootingCursorEnabled(enabled);
        gunController?.SetShootingEnabled(enabled);
        enemyManager?.RemoveAllEnemies();
    }
}
