using UnityEngine;

public class ArcherIdleState : AbstractArcherState
{
    private Transform _boneToRotateForAim;

    public ArcherIdleState(ArcherFSM fsm, Animator animator, Transform boneToRotateForAim) : base(fsm, animator) 
    {
        _boneToRotateForAim = boneToRotateForAim;
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("isShooting", false);
        _boneToRotateForAim.rotation = Quaternion.Euler(0, 0, 90);
    }
}