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

    private MoveState moveState;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void Move()
    {
        Vector2 moveDir = Vector2.ClampMagnitude(moveInput, 1f);
        rb.linearVelocity = moveDir * moveSpeed;  
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
