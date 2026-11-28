using UnityEngine;

public class Sin: Projectile
{
    [SerializeField] private float angleOffset = 90f;

    private void LateUpdate()
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, angleOffset, 0f);
        }
    }
}
