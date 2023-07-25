using UnityEngine;

public interface IMobileEnemy
{
    public void MoveToTarget(Vector3 targetPosition);

    public void LookForTarget(Vector3 lastPositionOfTarget);

    public void RotatePivot(Vector3 movementVector);

    public void BeIdle();

    public void GoToSleep();
}
