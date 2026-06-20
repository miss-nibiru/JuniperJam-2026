using UnityEngine;

/// <summary>
/// this script is an interface that is sort of like a checklist
/// </summary>
public interface ILevelState
{

    void StartLevel();


    void ExecuteLevel();


    void StopLevel();


}