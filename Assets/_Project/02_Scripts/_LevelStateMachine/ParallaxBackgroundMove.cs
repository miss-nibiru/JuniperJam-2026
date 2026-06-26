using UnityEngine;

/// <summary>
/// this is for moving UI rect transform - I had done a shader before for lava but didn't look right here.
/// Moving UI might be the best way to go
/// </summary>
public class ParallaxBackgroundMove : MonoBehaviour
{
    [SerializeField] private Vector2 moveDirection = Vector2.up;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    
    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if(_rectTransform) _startPosition = _rectTransform.anchoredPosition; // start where I've placed it?
        
    }

    private void Update()
    {
        if(!_rectTransform) return;
        Vector2 direction = moveDirection.normalized;
        float moveAmount = Mathf.Sin(Time.time * moveSpeed) * moveDistance; // first time using this function, i hate math
        _rectTransform.anchoredPosition = _startPosition + direction * moveAmount;
        
    }


}
