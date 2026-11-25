using System.Collections;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;



public class MoveOnTrigger : TriggerAction
{

    [SerializeField] float TimeToMove;
    [SerializeField] Vector3 Destination;
    [SerializeField] GameObject ObjectToMove;
    protected override void OnTriggerAction()
    {
        if (!ObjectToMove) ObjectToMove = gameObject;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Vector3 startposition = ObjectToMove.transform.position;
        float timepassed = 0;
        while (timepassed < TimeToMove)
        {
            ObjectToMove.transform.position = Vector3.Lerp(startposition, Destination, timepassed/TimeToMove);
            timepassed+= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ObjectToMove.transform.position = Destination;
    }
}
