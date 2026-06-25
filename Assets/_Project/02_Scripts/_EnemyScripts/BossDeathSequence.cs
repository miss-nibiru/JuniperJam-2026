using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the final boss death sequence only and sends message to level end to show end screen
/// kill me why do i do this to myself... lets see...
/// </summary>

public class BossDeathSequence : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string deathTriggerName = "Death";
    [SerializeField] private float destroyDelay;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip deathYell;

    [Header("Camera and Boss Shake")]
    [SerializeField] private Transform cameraShake;
    [SerializeField] private Transform bossShakeTarget;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float cameraShakeStrength;
    [SerializeField] private float bossShakeStrength;

    [Header("Boss Parts")]
    [SerializeField] private Collider2D[] bossHitboxes;
    [SerializeField] private EnemyShooterController[] bossShooters;

    private bool _deathStarted;

    private void Awake()
    {
        
        if (!bossAnimator) bossAnimator = GetComponentInChildren<Animator>();
        if (!bossShakeTarget) bossShakeTarget = transform;
        if (!cameraShake && Camera.main) cameraShake = Camera.main.transform;
        if (!sfxSource) sfxSource = GetComponent<AudioSource>();
        if (!sfxSource) sfxSource = gameObject.AddComponent<AudioSource>();
        if (bossHitboxes == null || bossHitboxes.Length == 0) bossHitboxes = GetComponentsInChildren<Collider2D>(true);
        if (bossShooters == null || bossShooters.Length == 0) bossShooters = GetComponentsInChildren<EnemyShooterController>(true);
        
    }

    public void PlayDeathSequence()
    {
        if (_deathStarted) return;
        _deathStarted = true;
        StartCoroutine(DeathSequenceRoutine());
    }

    private IEnumerator DeathSequenceRoutine()
    {
        StopBossGameplay();

        yield return StartCoroutine(ShakeCameraAndBoss());

        PlayDeathYell();

        PlayDeathAnimation();

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }

    private void StopBossGameplay()
    {
        foreach (EnemyShooterController shooter in bossShooters) // all additional children thingys get deactivated
        {
            if (!shooter) continue;
            shooter.enabled = false;
        }

        foreach (Collider2D hitbox in bossHitboxes)
        {
            if (!hitbox) continue;
            hitbox.enabled = false;
        }
    }

    private IEnumerator ShakeCameraAndBoss()
    {
        Vector3 originalCameraPosition = cameraShake ? cameraShake.localPosition : Vector3.zero;
        Vector3 originalBossPosition = bossShakeTarget ? bossShakeTarget.localPosition : Vector3.zero;

        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            if (cameraShake)
            {
                float cameraX = Random.Range(-cameraShakeStrength, cameraShakeStrength);
                float cameraY = Random.Range(-cameraShakeStrength, cameraShakeStrength);
                cameraShake.localPosition = originalCameraPosition + new Vector3(cameraX, cameraY, 0f);
            }

            if (bossShakeTarget)
            {
                float bossX = Random.Range(-bossShakeStrength, bossShakeStrength);
                float bossY = Random.Range(-bossShakeStrength, bossShakeStrength);
                bossShakeTarget.localPosition = originalBossPosition + new Vector3(bossX, bossY, 0f);
            }

            yield return null;
        }

        if (cameraShake) cameraShake.localPosition = originalCameraPosition;
        if (bossShakeTarget) bossShakeTarget.localPosition = originalBossPosition;
    }

    private void PlayDeathYell()
    {
        if (!sfxSource) return;
        if (!deathYell) return;

        sfxSource.PlayOneShot(deathYell);
    }

    private void PlayDeathAnimation()
    {
        if (!bossAnimator) return;

        bossAnimator.SetTrigger(deathTriggerName);
    }
}