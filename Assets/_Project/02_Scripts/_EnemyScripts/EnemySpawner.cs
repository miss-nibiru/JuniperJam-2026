using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Knows how to spawn a monster on the grid and where to sapwn them // calls for the enemy to be reistered as active
/// </summary>

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemySpawnGroupData testSpawnGroupData;
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform playerTarget;
    

    private Vector3 GetSpawnPosition(EnemySpawnGroupData spawnGroupData, out EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate)
    {
    chosenCoordinate = EnemySpawnGroupData.EnemySpawnCoordinate.North;

    if (!spawnGroupData) return transform.position;

    EnemySpawnGroupData.EnemySpawnCoordinate[] coordinates = spawnGroupData.SpawnCoordinate;

    if (coordinates != null && coordinates.Length > 0)
    {
        chosenCoordinate = coordinates[0];
    }

    // STRATEGY ONE
    if (spawnGroupData.SpawnLocation == EnemySpawnGroupData.EnemySpawnLocation.SelectedGridCell)
    {
        Vector2Int[] selectedCells = spawnGroupData.SelectedGridCell;

        if (selectedCells == null || selectedCells.Length == 0)
        {
            return transform.position;
        }

        int randomCellIndex = Random.Range(0, selectedCells.Length);
        Vector2Int chosenCell = selectedCells[randomCellIndex];

        return mainGrid.GetGridLocation(chosenCell.x, chosenCell.y);
    }

    // STRATEGY TWO
    if (spawnGroupData.SpawnLocation == EnemySpawnGroupData.EnemySpawnLocation.RandomCoordinate)
    {
        if (coordinates == null || coordinates.Length == 0)
        {
            return transform.position;
        }

        int randomCoordinateIndex = Random.Range(0, coordinates.Length);
        chosenCoordinate = coordinates[randomCoordinateIndex];

        int column = 0;
        int row = 0;

        if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.North)
        {
            column = Random.Range(0, mainGrid.columns);
            row = mainGrid.rows - 1;
        }
        else if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.South)
        {
            column = Random.Range(0, mainGrid.columns);
            row = 0;
        }
        else if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.East)
        {
            column = mainGrid.columns - 1;
            row = Random.Range(0, mainGrid.rows);
        }
        else if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.West)
        {
            column = 0;
            row = Random.Range(0, mainGrid.rows);
        }

        return mainGrid.GetGridLocation(column, row);
    }

    return transform.position;
}

    public void SpawnEnemy(EnemySpawnGroupData spawnGroupData)
    {
        if (!spawnGroupData) return;
        if (!enemyManager.CanSpawnEnemy(spawnGroupData)) return;
        
        EnemySpawnGroupData.EnemySpawnPattern[] spawnPattern = spawnGroupData.SpawnPattern;
        EnemySpawnGroupData.EnemySpawnPattern chosenPattern = EnemySpawnGroupData.EnemySpawnPattern.Single;

        if (spawnPattern != null && spawnPattern.Length > 0)
        {
            int randomPatternIndex = Random.Range(0, spawnPattern.Length);
            chosenPattern = spawnPattern[randomPatternIndex];
        }
        
        EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate;
        Vector3 spawnPosition = GetSpawnPosition(spawnGroupData, out chosenCoordinate);
        
        if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Single)
            CreateEnemyAtPosition(spawnGroupData, spawnPosition);
        
        else if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Multiple)
            SpawnMultiple(spawnGroupData, spawnPosition);
        
        else if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Line)
            SpawnLine(spawnGroupData, spawnPosition, chosenCoordinate);
    }
    

    private void SpawnLine(
        EnemySpawnGroupData spawnGroupData,
        Vector3 spawnPosition,
        EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate
    )
    
    {
        int enemiesToSpawn = spawnGroupData.EnemiesPerPattern;
        float spacing = spawnGroupData.PatternSpacing;

        Vector3 direction = Vector3.right;

        if (spawnGroupData.PatternDirection == EnemySpawnGroupData.EnemyPatternDirection.Horizontal)
            direction = Vector3.right;

        else if (spawnGroupData.PatternDirection == EnemySpawnGroupData.EnemyPatternDirection.Vertical)
            direction = Vector3.up;

        else if (spawnGroupData.PatternDirection == EnemySpawnGroupData.EnemyPatternDirection.DiagonalUp)
            direction = new Vector3(1, 1, 0).normalized;

        else if (spawnGroupData.PatternDirection == EnemySpawnGroupData.EnemyPatternDirection.DiagonalDown)
            direction = new Vector3(1, -1, 0).normalized;

        Transform formationParent = null;

        if (spawnGroupData.GroupMovementType == EnemySpawnGroupData.ClusterMovementType.SpaceInvaders)
        {
            GameObject formationObject = new GameObject("EnemyFormation_SpaceInvaders");
            formationObject.transform.position = spawnPosition;

            EnemyFormationController formationController = formationObject.AddComponent<EnemyFormationController>();

            formationController.InitializeFormation(
                chosenCoordinate,
                spawnGroupData.GroupHorizontalDistance,
                spawnGroupData.GroupHorizontalSpeed,
                spawnGroupData.GroupAdvanceSpeed
            );

            formationParent = formationObject.transform;
        }

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (!enemyManager.CanSpawnEnemy(spawnGroupData)) return;

            Vector3 finalSpawnPosition = spawnPosition + direction * (spacing * i);

            GameObject spawnedEnemy = CreateEnemyAtPosition(spawnGroupData, finalSpawnPosition);

            if (spawnedEnemy && formationParent)
            {
                spawnedEnemy.transform.SetParent(formationParent, true);
            }
        }
    }

    private void SpawnMultiple(EnemySpawnGroupData spawnGroupData, Vector3 spawnPosition)
    {
        int enemiesToSpawn = spawnGroupData.EnemiesPerPattern;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (!enemyManager.CanSpawnEnemy(spawnGroupData)) return;

            // little random offset so the enemies spawn as a messy cluster instead of a stack
            Vector2 randomOffset = Random.insideUnitCircle * spawnGroupData.PatternSpacing;

            Vector3 finalSpawnPosition = spawnPosition;
            finalSpawnPosition.x += randomOffset.x;
            finalSpawnPosition.y += randomOffset.y;

            CreateEnemyAtPosition(spawnGroupData, finalSpawnPosition);
        }
    }
    
    private GameObject CreateEnemyAtPosition(EnemySpawnGroupData spawnGroupData, Vector3 spawnPosition)
    {

        EnemyData enemy = spawnGroupData.EnemyData;
        if (!enemy) return null;
        GameObject enemyPrefab = enemy.EnemyPrefab;
        if (!enemyPrefab) return null;
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        if (!enemyController) return null;

        enemyController.InitializeEnemy(enemy, mainGrid, playerTarget);
        EnemyShooterController shooterController = spawnedEnemy.GetComponent<EnemyShooterController>();
        if (shooterController) shooterController.InitializeShooter(mainGrid, playerTarget);
        enemyManager.DetectEnemy(enemyController);
        return spawnedEnemy;
        
    }

}
