using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private RunStateManager runStateManager;

    private int _currentHealth;
    private bool _isDead;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        if(!runStateManager) runStateManager = FindObjectOfType<RunStateManager>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _isDead = false;
    }

    public void TakeDamage(int damageAmount)
    {
        if(!_isDead) return;
        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        if (_currentHealth <= 0) Die();
        
    }

    private void Die()
    {
        if(!_isDead) return;
        _isDead = true;

        //if (runStateManager) runStateManager.GameOver(); // player getting destroyed will be handled by the state manager or it will create nulls
        //else Debug.Log("Player died - But theres manager!!");
        
    }
}
