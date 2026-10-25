using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public class Elevator: MonoBehaviour
{
    [SerializeField] private float activationDistance = 1.5f;
    private int count = 0;
    private bool isReady = true;

    private void Update()
    {
        if (Player.player == null) return;

        float distance = Vector3.Distance(transform.position, Player.player.transform.position);

        if (isReady && distance <= activationDistance)
        {
            if (GameManager.Instance.FloorChange < 5)
            {
                ChangeElevatorState();
                ActivateElevator();
                Invoke(nameof(ChangeElevatorState), 5f);
            }
            else
            {
                Victory();
            }
        }
    }

    private void ActivateElevator()
    {
        GameManager.Instance.FloorChange++;
        GameManager.Instance.LoadScene("MainGeneration", GameManager.GameState.Playing);
    }

    private void Victory()
    {
        GameManager.Instance.LoadScene("Scene_with_bober", GameState.Victory);
    }

    private void ChangeElevatorState()
    {
        isReady = !isReady;
    }
}
