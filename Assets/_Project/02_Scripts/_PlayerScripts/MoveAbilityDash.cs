using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMove))]
public class MoveAbilityDash : MoveAbility
{
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashForce;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override IEnumerator UseMoveAbility(PlayerMove playerMove, Vector2 direction)
    {
        rb.AddForce(direction * dashForce);
        yield return new WaitForSeconds(dashDuration);
    }
}
