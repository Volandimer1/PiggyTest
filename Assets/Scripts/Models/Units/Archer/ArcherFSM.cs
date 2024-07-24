using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFSM : IUpdatable
{
    public Dictionary<Type, IArcherState> StatesDictionary { get; private set; }
    public IArcherState CurrentState {  get; private set; }

    public ArcherFSM(Animator animator, Transform arrowStartPosition, float initialSpeed, List<GameObject> targets, ArcherTriggerHandler archerTriggerHandler,
                      Transform boneToRotateForAim, GameObject tragectorySpherePrefab, ArrowPooler arrowPooler)
    {
        StatesDictionary = new Dictionary<Type, IArcherState>();
        InitializeStates(animator, arrowStartPosition, initialSpeed, targets, archerTriggerHandler, boneToRotateForAim, tragectorySpherePrefab, arrowPooler);
        TransitionToState<ArcherIdleState>();
    }

    public void Tick()
    {
        Update();
    }

    public void TransitionToState<T>() where T : IArcherState
    {
        Type stateType = typeof(T);

        if (!StatesDictionary.ContainsKey(stateType))
        {
            Debug.LogError($"State of type {stateType.Name} is not registered.");
            return;
        }

        CurrentState?.Exit();
        CurrentState = StatesDictionary[stateType];
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    private void InitializeStates(Animator animator, Transform arrowStartPosition, float initialSpeed, List<GameObject> targets, ArcherTriggerHandler archerTriggerHandler,
                                  Transform boneToRotateForAim, GameObject tragectorySpherePrefab, ArrowPooler arrowPooler)
    {
        StatesDictionary.Add(typeof(ArcherIdleState), new ArcherIdleState(this, animator, boneToRotateForAim));
        StatesDictionary.Add(typeof(ArcherShootingState), new ArcherShootingState(this, animator, arrowStartPosition, initialSpeed, targets, archerTriggerHandler,
                                                                                  boneToRotateForAim, tragectorySpherePrefab, arrowPooler));
    }
}