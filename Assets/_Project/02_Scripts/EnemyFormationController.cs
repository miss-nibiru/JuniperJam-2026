using UnityEngine;

/// <summary>
/// this has all the info of how the clusters and groups of enemies move together in formations
/// </summary>
public class EnemyFormationController : MonoBehaviour
{
    private float _horizontalDistance;
    private float _horizontalSpeed;
    private float _advanceSpeed;

    private Vector3 _startPosition;
    private Vector3 _advanceDirection;
    private int _horizontalDirection = 1;

    public void InitializeFormation(
        EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate,
        float horizontalDistance,
        float horizontalSpeed,
        float advanceSpeed
    )
    {
        this._horizontalDistance = horizontalDistance;
        this._horizontalSpeed = horizontalSpeed;
        this._advanceSpeed = advanceSpeed;

        _startPosition = transform.position;
        _advanceDirection = GetAdvanceDirection(chosenCoordinate);
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        MoveHorCluster();
    }

    private Vector3 GetAdvanceDirection(EnemySpawnGroupData.EnemySpawnCoordinate chosenCoordinate)
    {
        if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.North)
            return Vector3.down;

        if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.South)
            return Vector3.up;

        if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.East)
            return Vector3.left;

        if (chosenCoordinate == EnemySpawnGroupData.EnemySpawnCoordinate.West)
            return Vector3.right;

        return Vector3.down;
    }

    private void MoveHorCluster()
    {
        Vector3 movement = Vector3.zero;

        movement.x = _horizontalDirection * _horizontalSpeed * Time.deltaTime;
        movement += _advanceDirection * (_advanceSpeed * Time.deltaTime);

        transform.position += movement;

        float distanceFromStartX = transform.position.x - _startPosition.x;

        if (Mathf.Abs(distanceFromStartX) >= _horizontalDistance)
        {
            _horizontalDirection *= -1;
        }
    }
}