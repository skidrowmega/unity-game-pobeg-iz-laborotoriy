using UnityEngine;

public class WeaponSpinn : MonoBehaviour
{
    public Animator animator;
    public Vector3 rotationSpeed = new Vector3(0f, 1500f, 0f);
    private Quaternion originalRotation;
    private bool wasParrying = false;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Parry"))
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
            wasParrying = true;
        }
        else
        {
            if (wasParrying)
            {
                transform.localRotation = originalRotation;
                wasParrying = false;
            }
        }
    }
}
