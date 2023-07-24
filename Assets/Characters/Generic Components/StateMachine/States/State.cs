public abstract class State
{
    public bool ShouldStateChange = false;

    public virtual void OnEnterState() { }

    public virtual void OnExitState() { }

    public virtual void OnUpdate() { }
}
