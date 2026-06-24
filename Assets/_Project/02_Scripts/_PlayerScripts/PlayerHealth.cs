using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealthBars
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private RunStateManager runStateManager;

    private int _currentHealth;
    private bool _isDead;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => maxHealth;
    public event Action<int, int> HealthChanged; // from the interface 

    void Awake()
    {
        if(!runStateManager) runStateManager = FindObjectOfType<RunStateManager>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _isDead = false;
        
        HealthChanged?.Invoke(_currentHealth, maxHealth);
        
    }

    public void TakeDamage(int damageAmount)
    {
        if(_isDead) return;

        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        HealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    
    //Player can also heal now!

    public void Heal(int healAmount)
    {
        if(_isDead) return;
        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        HealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    private void Die()
    {
        if(_isDead) return;

        _isDead = true;

        if (runStateManager)
        {
            runStateManager.GameOver();
        }
        else
        {
            Debug.Log("Player died - but there is no RunStateManager assigned!");
        }
    }

    
}
