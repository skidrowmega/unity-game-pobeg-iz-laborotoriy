using UnityEngine;
using UnityEngine.UIElements;

public class Elevator: MonoBehaviour
{
    [SerializeField] private float activationDistance = 1.5f;

    private void Update()
    {
        if (Player.player == null) return;

        float distance = Vector3.Distance(transform.position, Player.player.transform.position);

        if (distance <= activationDistance)
        {
            ActivateElevator();
        }
    }

    private void ActivateElevator()
    {
        GameManager.Instance.LoadScene("MainGeneration", GameManager.GameState.Playing);
    }
}
