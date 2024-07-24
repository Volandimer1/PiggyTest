using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTriggerHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Updater _updater;
    [SerializeField] private string _targetTag = "Targetable";
    [SerializeField] private Transform _arrowStartPosition;
    [SerializeField] private Transform _boneToRotateForAim;
    [SerializeField] private ArrowConroller _arrowPrefab;
    [SerializeField] private float _initialSpeed = 10f;
    [SerializeField] private GameObject _tragectorySpherePrefab;

    public Action OnTargetChange;
    public Action OnShootHappened;

    private ArcherFSM _archerFSM;
    private ArrowPooler _arrowPooler;
    private List<GameObject> _targets = new List<GameObject>();

    public void TriggerArrowShootEvent()
    {
        OnShootHappened?.Invoke();
    }

    public void Initialize(Updater updater, ArrowPooler arrowPooler)
    {
        if (_archerFSM == null)
        {
            _arrowPooler = arrowPooler;
            _archerFSM = new ArcherFSM(_animator, _arrowStartPosition, _initialSpeed, _targets, this, _boneToRotateForAim, _tragectorySpherePrefab, _arrowPooler);
            _updater = updater;
            _updater.AddUpdatable(_archerFSM);
        }
        else
        {
            Debug.Log("U try to initialize Archer that already'v been initialized");
        }
    }

    private void Start()
    {
        if (_updater == null)
        {
            Debug.Log("U probably just placed manualy some Archer on a scene and forgot to give reference to an Updater object in its field");
        }
        else
        {
            if (_archerFSM == null)
            {
                ArrowFactory arrowFactory = new ArrowFactory(_arrowPrefab);
                ArrowPooler arrowPooler = new ArrowPooler(arrowFactory);
                arrowFactory.SetPooler(arrowPooler);
                _arrowPooler = arrowPooler;
                _archerFSM = new ArcherFSM(_animator, _arrowStartPosition, _initialSpeed, _targets, this, _boneToRotateForAim, _tragectorySpherePrefab, _arrowPooler);
                _updater.AddUpdatable(_archerFSM);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_targetTag))
        {
            AddTarget(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_targetTag))
        {
            RemoveTarget(other.gameObject);
        }
    }

    private void AddTarget(GameObject target)
    {
        if (!_targets.Contains(target))
        {
            _targets.Add(target);
            if (_archerFSM.CurrentState.GetType() != typeof(ArcherShootingState))
            {
                _archerFSM.TransitionToState<ArcherShootingState>();
            }
        }
    }

    private void RemoveTarget(GameObject target)
    {
        if (_targets.Contains(target))
        {
            int targetIndex = _targets.IndexOf(target);
            _targets.Remove(target);
            if (_targets.Count == 0)
            {
                _archerFSM.TransitionToState<ArcherIdleState>();
            }
            else
            if (targetIndex == 0)
            {
                OnTargetChange?.Invoke();
            }
        }
    }
}
