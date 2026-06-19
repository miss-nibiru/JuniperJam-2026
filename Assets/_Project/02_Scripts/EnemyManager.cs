using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the neemy spwan nad removes it when it dies or gets the player
/// checks all enemy, knows how many and which ones
/// what happens when enemy dies
/// </summary>

public class EnemyManager : MonoBehaviour
{

    private List<EnemyController> _activeEnemies = new List<EnemyController>();

    
    public bool CanSpawnEnemy(EnemySpawnGroupData spawnGroupData)
    {
        if (!spawnGroupData) return false;
        EnemyData enemyData = spawnGroupData.EnemyData;
        
        if(!enemyData) return false;

        int activeEnemies = CountActiveEnemiesOfType(enemyData);
        if (activeEnemies >= spawnGroupData.MaxActiveOfType) return false;
        
        return true;

    }
    

    public int CountActiveEnemiesOfType(EnemyData enemyData) 
    {
        if(!enemyData) return 0;
        _activeEnemies.RemoveAll(enemy => !enemy);
        int count = 0;

        foreach (EnemyController enemy in _activeEnemies)
        {
            if (!enemy) continue;
            if(enemy.EnemyData == enemyData) count++;
            
        }
        
        return count;
        
    }
    
    public int ActiveEnemyCount
    {
        get
        {
            _activeEnemies.RemoveAll(enemy => !enemy);
            return _activeEnemies.Count;
            
        }
        
    }
    

    public void DetectEnemy(EnemyController enemy)
    {
        if (!enemy) return;
        _activeEnemies.Add(enemy);
    }
    

    public void RemoveEnemy(EnemyController enemy)
    {
        if (!enemy) return;
        _activeEnemies.Remove(enemy);

    }

    public void RemoveAllEnemies()
    {
        _activeEnemies.RemoveAll(enemy => !enemy);
        foreach (EnemyController enemy in _activeEnemies)
        {
            if (enemy)
                Destroy(enemy.gameObject);
            
        }
        _activeEnemies.Clear();
    }
    
    
}
