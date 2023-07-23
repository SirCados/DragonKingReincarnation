using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public virtual void OnEnterState() { }

    public virtual void OnExitState() { }

    public virtual void OnUpdate() { }
}
