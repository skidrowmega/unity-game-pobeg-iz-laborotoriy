using UnityEngine;
using UnityEngine.UIElements;

public class Elevator: MonoBehaviour
{
    [SerializeField] private float activationDistance = 1.5f;
    private int count = 0;

    private void Update()
    {
        if (Player.player == null) return;

        float distance = Vector3.Distance(transform.position, Player.player.transform.position);

        if (distance <= activationDistance)
        {
            if (count < 5)
            {
                ActivateElevator();
                count++;
            }
            else
            {
                Victory();
            }
        }
    }

    private void ActivateElevator()
    {
        GameManager.Instance.LoadScene("MainGeneration", GameManager.GameState.Playing);
    }

    private void Victory()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Victory);
    }
}
