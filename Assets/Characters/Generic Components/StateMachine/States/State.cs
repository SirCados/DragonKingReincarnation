using UnityEngine;

public abstract class State: MonoBehaviour
{
    public bool ShouldStateChange = false;

    public virtual void OnEnterState() { }

    public virtual void OnExitState() { }

    public virtual void OnUpdate() { }
}
