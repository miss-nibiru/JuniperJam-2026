using UnityEngine;

public abstract class RunBaseState
{
    public abstract void EnterState(RunStateManager runStateManager);

    public abstract void UpdateState(RunStateManager runStateManager);

    public abstract void ExitState(RunStateManager runStateManager);
}
