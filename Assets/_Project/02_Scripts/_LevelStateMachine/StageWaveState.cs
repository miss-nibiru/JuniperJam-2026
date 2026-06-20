using UnityEngine;

/// <summary>
/// controls the waves of the enemies that spawn -- interface connected to the stateinterface thingy
/// </summary>

public class StageWaveState : ILevelState
{
    public void StartLevel()
    {
        Debug.Log("Level has started");
    }

    public void ExecuteLevel()
    {
        // Later this will spawn enemies
    }

    public void StopLevel()
    {
        Debug.Log("Incomming next wave");
    }
}