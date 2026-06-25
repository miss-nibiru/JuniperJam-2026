using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// controls all enemy behaviours and strategies - feeds from enemydata and talks to enemy manager i think?
/// </summary>
public class EnemyController : MonoBehaviour, IHealthBars
{
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform playerTargetPoint;
    
    [SerializeField] private BossDeathSequence bossDeathSequence;

    private bool _canChase;
    private Coroutine _movementRoutine;
    
    private Vector3 _startPosition;
    private bool _movingSide;
    
    private int _currentHealth;
    private bool _isDying;
    
    // interface stuff
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => enemyData ? enemyData.MaxHealth : 0;
    public event Action<int, int> HealthChanged;

    public EnemyData EnemyData => enemyData;

    private void Start()
    {
        if(!bossDeathSequence) bossDeathSequence = GetComponent<BossDeathSequence>();
        if(enemyData) _currentHealth = enemyData.MaxHealth;
    }

    void Update()
    {
        if(_isDying) return;
        if(!EnemyData) return;
        if (EnemyData.MovementType == EnemyData.EnemyMovementType.Chase && _canChase)
        {
            ChasePlayer();
        }

        if (EnemyData.MovementType == EnemyData.EnemyMovementType.LeftRight || EnemyData.MovementType == EnemyData.EnemyMovementType.Boss)
        {

            MoveLeftRight();

        }
        
        CheckIfOutsideGrid();

    }

    private void MoveLeftRight()
    {
        float leftLimit = _startPosition.x - EnemyData.LeftRightDistance;
        float rightLimit = _startPosition.x + EnemyData.LeftRightDistance;

        Vector3 targetPosition = transform.position;

        if (_movingSide)
        {
            targetPosition.x = rightLimit;
        }
        else
        {
            targetPosition.x = leftLimit;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            EnemyData.MoveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) <= 0.05f)
        {
            _movingSide = !_movingSide;
        }
    }

    private void CheckIfOutsideGrid()
    {
        if (!mainGrid || !enemyData) return;

        if (mainGrid.IsThingOutOfBounds(transform.position, enemyData.GridDestroyPadding))
        {
            DieBish();
        }
    }

    public void InitializeEnemy(EnemyData enemy, MainGridManager grid, Transform playerTarget)
    {
        enemyData = enemy;
        mainGrid = grid;
        playerTargetPoint = playerTarget;
        _startPosition = transform.position;
        
        _currentHealth = enemyData.MaxHealth;
        HealthChanged?.Invoke(_currentHealth, MaxHealth);
        
        if (!enemyData) return;

        if (enemyData.MovementType == EnemyData.EnemyMovementType.Chase)
        {
            if (_movementRoutine != null)
            {
                StopCoroutine(_movementRoutine);
            }

            _movementRoutine = StartCoroutine(SpawnThenChase());
        }
    }

    private void ChasePlayer()
    {
        if(!playerTargetPoint) return;
        
        //get player position
        //keep z position but move to the player using data speed
        Vector3 targetPosition = playerTargetPoint.position;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, EnemyData.MoveSpeed * Time.deltaTime);
        
    }

    private IEnumerator SpawnThenChase()
    {

        _canChase = false; //start not detecting player

        if (!playerTargetPoint || !enemyData) yield break;

        Vector3 directionToPlayer = playerTargetPoint.position - transform.position;
        directionToPlayer.z = 0f;
        directionToPlayer.Normalize();

        float rowHeight = 1f;

        if (mainGrid)
        {
            Vector3 rowZero = mainGrid.GetGridLocation(0, 0);
            Vector3 rowOne = mainGrid.GetGridLocation(0, 1);
            rowHeight = Mathf.Abs(rowOne.y - rowZero.y);
        }

        Vector3 introTarget = transform.position + directionToPlayer * (rowHeight * enemyData.IntroRows);

        while (Vector3.Distance(transform.position, introTarget) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                introTarget,
                enemyData.IntroSpeed * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(enemyData.ChaseDelay);

        _canChase = true;
        

    }
    
    //ALWAYS make sure to call a soemthing to kill the coroutine

    private void StopMovementRoutine()
    {
        if (_movementRoutine != null) StopCoroutine(_movementRoutine); 
        _movementRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_isDying)return;
        if(!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth) playerHealth.TakeDamage(enemyData.DamageAmount);

        StopMovementRoutine();
        Destroy(gameObject);

    }

    public void TakeDamage(int damageAmount)
    {
        if (_isDying) return;
        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Clamp(_currentHealth,0,MaxHealth);
        HealthChanged?.Invoke(_currentHealth, MaxHealth);
        if (_currentHealth <= 0)
        {
            //death animation here
            DieBish();
        }
    }

    private void DieBish()
    {
        if (_isDying) return;
        _isDying = true;
        StopMovementRoutine();

        if (enemyData && enemyData.MovementType == EnemyData.EnemyMovementType.Boss && bossDeathSequence)
        {
            bossDeathSequence.PlayDeathSequence();
            return;
        }
        Destroy(gameObject);
    }
}
