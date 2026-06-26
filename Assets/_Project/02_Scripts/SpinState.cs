public class SpinState : RunBaseState
{
    public override void EnterState(RunStateManager runStateManager) //found a bit of a cleaner version of running both state machines!
    {
        runStateManager.SetCombatSystemsEnabled(false);
        runStateManager.ResetSpinWheel();
        runStateManager.ShowSpinPanel(true);
        runStateManager.ShowGameOverPanel(false);
        runStateManager.ShowWinPanel(false);
    }

    public override void UpdateState(RunStateManager runStateManager)
    {
        
    }

    public override void ExitState(RunStateManager runStateManager)
    {
        runStateManager.ShowSpinPanel(false);
    }
}
