using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerHealth))]
public class MoveAbilityDash : MoveAbility
{
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashForce;
    [SerializeField] private string dashAnimationTrigger;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private Sprite restingSprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimator = GetComponentInChildren<Animator>();

        if (playerAnimator)
        {
            playerSpriteRenderer = playerAnimator.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer) restingSprite = playerSpriteRenderer.sprite;

            playerAnimator.enabled = false;
        }
    }

    public override IEnumerator UseMoveAbility(PlayerMove playerMove, Vector2 direction)
    {
        SetDashInvulnerable(true);
        PlayDashAnimation();
        rb.AddForce(direction * dashForce);
        yield return new WaitForSeconds(dashDuration);
        StopDashAnimation();
        SetDashInvulnerable(false);
    }

    private void OnDisable()
    {
        SetDashInvulnerable(false);
    }

    private void SetDashInvulnerable(bool isInvulnerable)
    {
        if (playerHealth) playerHealth.SetInvulnerable(isInvulnerable);
    }

    private void PlayDashAnimation()
    {
        if (!playerAnimator) return;
        if (string.IsNullOrEmpty(dashAnimationTrigger)) return;

        playerAnimator.enabled = true;
        playerAnimator.SetTrigger(dashAnimationTrigger);
    }

    private void StopDashAnimation()
    {
        if (!playerAnimator) return;

        if (!string.IsNullOrEmpty(dashAnimationTrigger))
        {
            playerAnimator.ResetTrigger(dashAnimationTrigger);
        }

        playerAnimator.enabled = false;

        if (playerSpriteRenderer && restingSprite)
        {
            playerSpriteRenderer.sprite = restingSprite;
        }
    }
}
