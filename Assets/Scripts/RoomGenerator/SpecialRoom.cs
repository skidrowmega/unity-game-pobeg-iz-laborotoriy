using UnityEngine;

public class SpecialRoom : Room
{
    [SerializeField] Transform TransformToRotate;
    public override void RemoveWall(Vector2Int direction)
    {
        Vector3 lookdir = new Vector3(direction.x,0,direction.y);
        if (TransformToRotate != null)
            TransformToRotate.rotation = Quaternion.LookRotation(lookdir);
        base.RemoveWall(direction);
    }
}
