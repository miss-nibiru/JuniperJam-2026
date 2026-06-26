using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IHealthBars
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private RunStateManager runStateManager;

    [SerializeField] private Transform cameraShake;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float cameraShakeStrength;

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

        StartCoroutine(ShakeCamera());
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

    private IEnumerator ShakeCamera()
    {
        Vector3 originalCameraPosition = cameraShake ? cameraShake.localPosition : Vector3.zero;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            if (cameraShake)
            {
                float cameraX = UnityEngine.Random.Range(-cameraShakeStrength, cameraShakeStrength);
                float cameraY = UnityEngine.Random.Range(-cameraShakeStrength, cameraShakeStrength);
                cameraShake.localPosition = originalCameraPosition + new Vector3(cameraX, cameraY, 0f);
            }

            yield return null;
        }

        if (cameraShake) cameraShake.localPosition = originalCameraPosition;    }
}
