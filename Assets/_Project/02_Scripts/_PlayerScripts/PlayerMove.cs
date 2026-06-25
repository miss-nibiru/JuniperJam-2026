using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private enum MoveState
    {
        Move, AbilityMove
    }

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float moveAbilityCooldown;

    [SerializeField] private MoveAbility moveAbility;

    private float cooldown;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Camera mainCamera;

    private MoveState moveState;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        cooldown = 0f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAbilityMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(PerformMoveAbility());
        }
    }

    void Update()
    {
        if(moveState == MoveState.Move)
        {
            Move();
        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        KeepPlayerInCameraBounds();
    }

    private void Move()
    {
        Vector2 moveDir = Vector2.ClampMagnitude(moveInput, 1f);
        rb.linearVelocity = moveDir * moveSpeed;  
    }

    private void KeepPlayerInCameraBounds()
    {
        if (!mainCamera) return;

        Vector2 playerPosition = rb.position;
        Vector2 playerSize = Vector2.zero;
        Vector2 playerOffset = Vector2.zero;

        if (playerCollider)
        {
            Bounds playerBounds = playerCollider.bounds;
            playerSize = playerBounds.extents;
            playerOffset = playerBounds.center - transform.position;
        }

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        Vector3 cameraPosition = mainCamera.transform.position;

        float minX = cameraPosition.x - cameraWidth - playerOffset.x + playerSize.x;
        float maxX = cameraPosition.x + cameraWidth - playerOffset.x - playerSize.x;
        float minY = cameraPosition.y - cameraHeight - playerOffset.y + playerSize.y;
        float maxY = cameraPosition.y + cameraHeight - playerOffset.y - playerSize.y;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(playerPosition.x, minX, maxX),
            Mathf.Clamp(playerPosition.y, minY, maxY)
        );

        if (clampedPosition == playerPosition) return;

        Vector2 velocity = rb.linearVelocity;

        if (clampedPosition.x != playerPosition.x) velocity.x = 0f;
        if (clampedPosition.y != playerPosition.y) velocity.y = 0f;

        rb.position = clampedPosition;
        rb.linearVelocity = velocity;
    }

    private IEnumerator PerformMoveAbility()
    {
        if (cooldown <= 0f)
        {
            moveState = MoveState.AbilityMove;
            yield return moveAbility.UseMoveAbility(this, moveInput.normalized);
            moveState = MoveState.Move;
            cooldown = moveAbilityCooldown;
        }
    }
}
