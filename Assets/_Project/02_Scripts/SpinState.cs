using UnityEngine;
using static SpinningWheel;

public class SpinState : RunBaseState
{
    private bool spinFinished;
    private WeaponChoice weaponChosen;

    public override void EnterState(RunStateManager runStateManager)
    {
        runStateManager.spinningWheel.SpinFinished += OnSpinFinished;
        runStateManager.SetCombatSystemsEnabled(false);
    }

    public override void UpdateState(RunStateManager runStateManager)
    {
        if (spinFinished)
        {
            runStateManager.finishedSpinning = true;
        }
    }

    public override void ExitState(RunStateManager runStateManager)
    {
        runStateManager.spinningWheel.SpinFinished -= OnSpinFinished;
        runStateManager.SetCombatSystemsEnabled(true);
    }

    private void OnSpinFinished(WeaponChoice weaponChoice)
    {
        spinFinished = true;
        weaponChosen = weaponChoice;
    }
}
