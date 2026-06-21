using UnityEngine;

public class CombatState : RunBaseState
{
    public override void EnterState(RunStateManager runStateManager)
    {
        runStateManager.SetCombatSystemsEnabled(true);
    }

    public override void UpdateState(RunStateManager runStateManager)
    {

    }

    public override void ExitState(RunStateManager runStateManager)
    {
        runStateManager.SetCombatSystemsEnabled(false);
    }
}
