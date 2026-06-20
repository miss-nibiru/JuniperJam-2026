using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpinningWheel : MonoBehaviour
{
    [SerializeField] private float rotatePower;
    [SerializeField] private float stopPower;

    private bool inSpin;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.angularVelocity > 0)
        {
            rb.angularVelocity -= stopPower * Time.deltaTime;
        }
        if (rb.angularVelocity == 0 && inSpin)
        {
            GetWeaponChoice();
        }
    }

    public void Spin()
    {
        if (!inSpin)
        {
            rb.AddTorque(rotatePower);
            inSpin = true;
        }
    }

    private void GetWeaponChoice()
    {
        float rotation = transform.eulerAngles.z;


    }
}
