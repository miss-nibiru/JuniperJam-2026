using System.Collections;
using UnityEngine;

public abstract class MoveAbility : MonoBehaviour
{
    public abstract IEnumerator UseMoveAbility(PlayerMove playerMove, Vector2 direction); 
}
