using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShootingState : AbstractArcherState
{
    private GameObject _tragectorySpherePrefab;
    private ArrowPooler _arrowPooler;
    private Transform _arrowStartPosition;
    private Transform _boneToRotateForAim;
    private float _initialSpeed;
    private List<GameObject> _targets;
    private float _shootingAngle;
    private GameObject[] _tragectory;
    private const float _gravity = 9.81f;
    private const float _tragectoriDeltaTimeStep = 0.2f;
    
    private const int _tragectoryResolution = 14;

    public ArcherShootingState(ArcherFSM fsm, Animator animator, Transform arrowStartPosition, float initialSpeed, List<GameObject> targets, ArcherTriggerHandler archerTriggerHandler,
                               Transform boneToRotateForAim, GameObject tragectorySpherePrefab, ArrowPooler arrowPooler)
        : base(fsm, animator)
    {
        _arrowStartPosition = arrowStartPosition;
        _arrowPooler = arrowPooler;
        _initialSpeed = initialSpeed;
        _targets = targets;
        _tragectorySpherePrefab = tragectorySpherePrefab;
        _tragectory = new GameObject[_tragectoryResolution];
        for (int i = 0; i < _tragectoryResolution; i++)
        {
            _tragectory[i] = UnityEngine.Object.Instantiate(_tragectorySpherePrefab);
        }
        _boneToRotateForAim = boneToRotateForAim;
        archerTriggerHandler.OnTargetChange += ChangeTarget;
        archerTriggerHandler.OnShootHappened += ShootArrow;
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("isShooting", true);
        ChangeTarget();
        SwitchTragectoryVisibility(true);
    }

    public override void Update()
    {
        base.Update();
        //ChangeTarget();


        /*
         * код имплементирующий плавный и постепенный поворот в сторону конечного угла выстрела с учетом 
         * длительности текущей анимации поделенной на её скорость (для точного определения промежутков 
         * в секундах между выстрелами), если понадобится.
         * Или например если цели по которым надо стрелять сами тоже двигаются и не статичные и надо
         * поддерживать непрерывное изменение угла выстрела (вместо этого можно вначале один раз просчитывать
         * упреждение куда стрелять что бы попасть), и другие возможные механики для которых может потребываться
         * постоянный апдейт в этом стейте.
         */
    }

    public override void Exit()
    {
        base.Exit();
        SwitchTragectoryVisibility(false);
    }

    private void ChangeTarget()
    {
        Vector2 targetPosition = _targets[0].transform.position;
        Vector2 startPosition = _arrowStartPosition.position;
        float bodyAngle = CalculateLaunchAngle(startPosition, targetPosition);
        _boneToRotateForAim.rotation = Quaternion.Euler(0, 0, 90 + bodyAngle * Mathf.Rad2Deg);
        startPosition = _arrowStartPosition.position;
        _shootingAngle = CalculateLaunchAngle(startPosition, targetPosition);
        SetNewTragectory();

        bool reachable = IsTargetReachable(startPosition, targetPosition);
        if (!reachable) 
        {
            Debug.Log($"With current starting arrow speed of {_initialSpeed} Target is not reachable");
        }
    }

    private void SetNewTragectory()
    {
        if (_tragectory == null)
        { 
            return;
        }

        for (int i = 0; i < _tragectoryResolution; i++)
        {
            if (_tragectory[i] == null)
            {
                continue;
            }
            else
            {
                float t = i * _tragectoriDeltaTimeStep;
                _tragectory[i].transform.position = CalculatePositionAtTime(t, _shootingAngle);
            }
        }
    }

    private void ShootArrow()
    {
        if (_targets.Count > 0)
        {
            Array values = Enum.GetValues(typeof(ArrowType));
            ArrowType randomArrowType = (ArrowType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            _arrowPooler.GetArrow(randomArrowType, _arrowStartPosition.position, _shootingAngle, _initialSpeed);
        }
    }

    private void SwitchTragectoryVisibility(bool isVisible)
    {
        if (_tragectory[0] == null)
        {
            return;
        }

        for (int i = 0; i < _tragectoryResolution; i++)
        {
            if (_tragectory[i] == null)
            {
                continue;
            }
            else
            {
                _tragectory[i].gameObject.SetActive(isVisible);
            }
        }
    }

    private float CalculateLaunchAngle(Vector2 startPosition, Vector2 targetPosition)
    {
        float distanceX = targetPosition.x - startPosition.x;
        float distanceY = targetPosition.y - startPosition.y;

        float velocitySquared = _initialSpeed * _initialSpeed;
        float underRoot = velocitySquared * velocitySquared - _gravity * (_gravity * distanceX * distanceX + 2 * distanceY * velocitySquared);

        if (underRoot < 0)
        {
            // Цель недостижима
            return Mathf.Atan2(distanceY, distanceX);// Возвращаем угол направления на цель
        }

        float root = Mathf.Sqrt(underRoot);
        float angle1 = Mathf.Atan((velocitySquared + root) / (_gravity * distanceX));
        float angle2 = Mathf.Atan((velocitySquared - root) / (_gravity * distanceX));

        // Возвращаем меньший угол (обычно он используется для стрельбы на меньшие расстояния)
        return Mathf.Min(angle1, angle2);
    }

    private Vector3 CalculatePositionAtTime(float t, float angle)
    {
        float x = _initialSpeed * t * Mathf.Cos(angle);
        float y = _initialSpeed * t * Mathf.Sin(angle) - 0.5f * _gravity * t * t;
        return new Vector3(_arrowStartPosition.position.x + x, _arrowStartPosition.position.y + y, 0);
    }

    bool IsTargetReachable(Vector2 startPosition, Vector2 targetPosition)
    {
        float distanceX = targetPosition.x - startPosition.x;
        float distanceY = targetPosition.y - startPosition.y;

        float velocitySquared = _initialSpeed * _initialSpeed;
        float gravitySquared = _gravity * _gravity;

        float discriminant = velocitySquared * velocitySquared - gravitySquared * (distanceX * distanceX + 2 * distanceY * velocitySquared / _gravity);

        return discriminant >= 0;
    }
}