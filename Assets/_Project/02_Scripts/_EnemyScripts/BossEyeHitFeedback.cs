using System.Collections;
using UnityEngine;

/// <summary>
/// Visual feedback for boss eyes -- marker for player to know wher to shoot
/// </summary>
public class BossEyeHitFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eyeSprite;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashTime = 0.12f;
    [SerializeField] private float pulseScale = 1.25f;

    private Vector3 _startScale;
    private Coroutine _flashRoutine;

    private void Awake()
    {
        if (!eyeSprite) eyeSprite = GetComponent<SpriteRenderer>();
        _startScale = transform.localScale;
        if (eyeSprite) eyeSprite.color = normalColor;
    }

    public void Flash()
    {
        if (!eyeSprite) return;
        if (_flashRoutine != null) StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        eyeSprite.color = flashColor;
        transform.localScale = _startScale * pulseScale;

        yield return new WaitForSeconds(flashTime);

        eyeSprite.color = normalColor;
        transform.localScale = _startScale;

        _flashRoutine = null;
    }
}