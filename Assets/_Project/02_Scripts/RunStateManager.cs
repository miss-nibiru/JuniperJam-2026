using UnityEngine;
/// <summary>
/// this guy controls the entire game!
/// </summary>
public class RunStateManager : MonoBehaviour
{
    private RunBaseState _currentRunState;

    public SpinState SpinRunState = new SpinState();
    public CombatState CombatRunState = new CombatState();
    public EndState EndRunState = new EndState();

    //game references
    [SerializeField] private SpinningWheel spinningWheel;
    [SerializeField] private LevelStateMachine levelStateMachine;
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyManager enemyManager;

    public GunController gunController;

    private void OnEnable()
    {
        if(spinningWheel) spinningWheel.SpinFinished += HandleSpinFinished;
        if (levelStateMachine)
        {
            levelStateMachine.LevelComplete += HandleLevelCompleted;
            levelStateMachine.LevelComplete += HandleAllLevelsCompleted;
        }
    }

    private void OnDisable()
    {
        if(spinningWheel) spinningWheel.SpinFinished -= HandleSpinFinished;
        if (levelStateMachine)
        {
            levelStateMachine.LevelComplete -= HandleLevelCompleted;
            levelStateMachine.LevelComplete -= HandleAllLevelsCompleted;
        }
    }

    void Start()
    {
        ShowGameOverPanel(false);
        ShowWinPanel(false);
        SwitchState(SpinRunState);
    }

    void Update()
    {
        _currentRunState?.UpdateState(this);
    }

    public void SwitchState(RunBaseState newState)
    {
        _currentRunState?.ExitState(this);
        _currentRunState = newState;
        _currentRunState?.EnterState(this);
    }

    public void SetCombatSystemsEnabled(bool enabled)
    {
        cursorManager?.SetShootingCursorEnabled(enabled);
        gunController?.SetShootingEnabled(enabled);
        if(!enabled) enemyManager?.RemoveAllEnemies();
    }
    
    private void HandleSpinFinished(SpinningWheel.WeaponChoice weaponChoice)
    {
        gunController?.SelectWeapon(weaponChoice);
        SwitchState(CombatRunState);
        if (levelStateMachine) levelStateMachine.StartNewLevel();
        
    }

    private void HandleLevelCompleted()
    {
        SwitchState(SpinRunState);
    }

    private void HandleAllLevelsCompleted()
    {
        WinGame();
    }

    public void GameOver()
    {
        levelStateMachine?.StopCurrentLevel();
        SwitchState(EndRunState);
        SetCombatSystemsEnabled(false);
        ShowSpinPanel(false);
        ShowWinPanel(false);
        ShowGameOverPanel(true);

        Debug.Log("Game Over");
    }

    public void WinGame()
    {
        levelStateMachine?.StopCurrentLevel();
        SwitchState(EndRunState);
        SetCombatSystemsEnabled(false);
        ShowSpinPanel(false);
        ShowGameOverPanel(false);
        ShowWinPanel(true);

        Debug.Log("You Win");
    }

    public void ShowSpinPanel(bool enabled)
    {
        GetUIManager()?.ShowSpinPanel(enabled);
    }

    public void ShowGameOverPanel(bool enabled)
    {
        GetUIManager().ShowDeadCanvas(enabled);
    }

    public void ShowWinPanel(bool enabled)
    {
        GetUIManager()?.ShowWinCanvas(enabled);
    }
    

    private UIManager GetUIManager()
    {
        if (uiManager) return uiManager;
        return UIManager.Instance;
    }
    
}
