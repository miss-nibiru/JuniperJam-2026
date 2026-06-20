using UnityEngine;

/// <summary>
/// Controls the main grid that needs to know where the player, enemies, projectiles and everything is
/// detects columns and rows
/// </summary>

public class MainGridManager : MonoBehaviour
{

    [SerializeField] private GameObject gridStart;
    [SerializeField] private GameObject gridEnd;

    [SerializeField] public int columns;
    [SerializeField] public int rows;
    [SerializeField] private float gizmoSize;
    
    public bool drawGrid = true;
    
    
    public Vector3 GetGridLocation(int columnNumber, int rowNumber)
    {
        Vector3 startPosition = gridStart.transform.position;
        Vector3 endPosition = gridEnd.transform.position;

        float columnWidth = (endPosition.x - startPosition.x) / (columns - 1);
        float rowHeight = (endPosition.y - startPosition.y) / (rows - 1);
        
        Vector3 gridLocation = startPosition;

        gridLocation.x += columnNumber * columnWidth;
        gridLocation.y += rowNumber * rowHeight;

        return gridLocation;

    }

    public bool IsThingOutOfBounds(Vector3 worldPosition, float padding = 0f)
    {
        Vector3 cornerA = GetGridLocation(0, 0);
        Vector3 cornerB = GetGridLocation(columns - 1, rows - 1);
        
        float minX = Mathf.Min(cornerA.x, cornerB.x) - padding;
        float maxX = Mathf.Max(cornerA.x, cornerB.x) + padding;
        float minY = Mathf.Min(cornerA.y, cornerB.y) - padding;
        float maxY = Mathf.Max(cornerA.y, cornerB.y) + padding;
        
        return worldPosition.x < minX || worldPosition.x > maxX || worldPosition.y < minY || worldPosition.y > maxY;
    }
    
    private void OnDrawGizmos()
    {
        if (!drawGrid || !gridStart || !gridEnd) return;

        Gizmos.color = Color.hotPink;

        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                Vector3 gridLocation = GetGridLocation(column, row);
                Gizmos.DrawWireSphere(gridLocation, gizmoSize);
            }
        }
    }
    
}