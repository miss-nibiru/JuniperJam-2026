using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private EnemyData enemyData;
    
    //player health call here
    
    private int _currentHealth;

    public EnemyData EnemyData => enemyData;

    private void Start()
    {
        _currentHealth = enemyData.MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); --- when Allan gets the script ready, connect to this one
        //take damage by enemydata.damageamount
        
        Destroy(gameObject);

    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            DieBish();
        }
    }

    private void DieBish()
    {
        Destroy(gameObject);
    }
}