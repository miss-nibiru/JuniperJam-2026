using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

/// <summary>
/// Knows how to spawn a monster on the grid and where to sapwn them // calls for the enemy to be reistered as active
/// </summary>

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemySpawnGroupData testSpawnGroupData;
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private EnemyManager enemyManager;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("T pressed, testing enemy spawn");
            SpawnEnemy(testSpawnGroupData);
        }
    }

    private Vector3 GetSpawnPosition(EnemySpawnGroupData spawnGroupData)
{
    if (!spawnGroupData) return transform.position;

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
        // get all the allowed spawn coordinates from the scriptable object
        EnemySpawnGroupData.EnemySpawnCoordinate[] coordinates = spawnGroupData.SpawnCoordinate;
        if (coordinates == null || coordinates.Length == 0)
        {
            return transform.position;
        }
        
        int randomCoordinateIndex = Random.Range(0, coordinates.Length);
        EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate = coordinates[randomCoordinateIndex];
        int column = 0; int row = 0;
        
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
        
        
        Vector3 spawnPosition = GetSpawnPosition(spawnGroupData);
        
        if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Single)
            CreateEnemyAtPosition(spawnGroupData, spawnPosition);
        
        else if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Multiple)
            SpawnMultiple(spawnGroupData, spawnPosition);
        
        else if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Line)
            SpawnLine(spawnGroupData, spawnPosition);
        
        else if (chosenPattern == EnemySpawnGroupData.EnemySpawnPattern.Circle)
            SpawnCircle(spawnGroupData, spawnPosition);
        
    }
    

    private void SpawnCircle(EnemySpawnGroupData spawnGroupData, Vector3 spawnPosition)
    {
        
    }

    private void SpawnLine(EnemySpawnGroupData spawnGroupData, Vector3 spawnPosition)
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

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (!enemyManager.CanSpawnEnemy(spawnGroupData)) return;
            Vector3 finalSpawnPosition = spawnPosition + direction * spacing * i;
            CreateEnemyAtPosition(spawnGroupData, finalSpawnPosition);
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

    
    private void CreateEnemyAtPosition(EnemySpawnGroupData spawnGroupData, Vector3 spawnPosition)
    {
        Debug.Log("Trying to create enemy at position");
        EnemyData enemyData = spawnGroupData.EnemyData;
        if (!enemyData) return;
        GameObject enemyPrefab = enemyData.EnemyPrefab;
        if (!enemyPrefab) return;
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        if (!enemyController) return;

        enemyManager.DetectEnemy(enemyController);
        
    }

}
