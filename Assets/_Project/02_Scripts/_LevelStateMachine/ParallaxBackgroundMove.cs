using UnityEngine;

/// <summary>
/// this is for moving UI rect transform - I had done a shader before for lava but didn't look right here.
/// Moving UI might be the best way to go
/// </summary>
/// 
public class ParallaxBackgroundMove : MonoBehaviour
{
    [SerializeField] private Vector2 moveDirection = Vector2.down;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private bool loopMovement;
    [SerializeField] private float loopDistance = 500f;

    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private float _movedDistance;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform) _startPosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        if (!_rectTransform) return;
        _movedDistance = 0f;
        _rectTransform.anchoredPosition = _startPosition;
    }

    private void Update()
    {
        if (!_rectTransform) return;
        Vector2 direction = moveDirection.normalized;
        float moveAmount = moveSpeed * Time.deltaTime;

        _rectTransform.anchoredPosition += direction * moveAmount;
        _movedDistance += moveAmount;

        if (loopMovement && _movedDistance >= loopDistance)
        {
            _movedDistance = 0f;
            _rectTransform.anchoredPosition = _startPosition;
        }
    }


}
