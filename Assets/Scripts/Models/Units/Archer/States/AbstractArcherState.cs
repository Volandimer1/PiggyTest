using UnityEngine;

public abstract class AbstractArcherState :IArcherState
{
    protected ArcherFSM fsm;
    protected Animator animator;

    protected AbstractArcherState(ArcherFSM fsm, Animator animator)
    {
        this.fsm = fsm;
        this.animator = animator;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}