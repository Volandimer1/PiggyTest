using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    [SerializeField] private GameObject _tragectorySpherePrefab;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _boneToRotateForAim;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _arrowStartPosition;
    [SerializeField] private float _initialSpeed = 10f;
    [SerializeField] private int _tragectoryResolution = 30;

    private GameObject[] _tragectory;
    private float gravity = 9.81f;

    private void Start()
    {
        _tragectory = new GameObject[_tragectoryResolution];
        for (int i = 0; i < _tragectoryResolution; i++)
        {
            _tragectory[i] = Instantiate(_tragectorySpherePrefab);
            //_tragectory[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Vector2 targetPosition = _target.position;
        Vector2 startPosition = _arrowStartPosition.position;

        float angle = CalculateLaunchAngle(startPosition, targetPosition);
        _boneToRotateForAim.rotation = Quaternion.Euler(0, 0, 90 + angle * Mathf.Rad2Deg);

        for (int i = 0; i < _tragectoryResolution; i++)
        {
            float t = i / (float)_tragectoryResolution;
            _tragectory[i].transform.position = CalculatePositionAtTime(t, angle);
        }

        bool reachable = IsTargetReachable(startPosition, targetPosition);
        Debug.Log("Target is reachable = " + reachable);

        if (reachable && Input.GetMouseButtonDown(0))
        {
            LaunchArrow(angle);
        }
    }

    private float CalculateLaunchAngle(Vector2 startPosition, Vector2 targetPosition)
    {
        float distanceX = targetPosition.x - startPosition.x;
        float distanceY = targetPosition.y - startPosition.y;

        float velocitySquared = _initialSpeed * _initialSpeed;
        float underRoot = velocitySquared * velocitySquared - gravity * (gravity * distanceX * distanceX + 2 * distanceY * velocitySquared);

        if (underRoot < 0)
        {
            // Цель недостижима
            return Mathf.Atan2(distanceY, distanceX); // Возвращаем угол направления на цель
        }

        float root = Mathf.Sqrt(underRoot);
        float angle1 = Mathf.Atan((velocitySquared + root) / (gravity * distanceX));
        float angle2 = Mathf.Atan((velocitySquared - root) / (gravity * distanceX));

        // Возвращаем меньший угол (обычно он используется для стрельбы на меньшие расстояния)
        return Mathf.Min(angle1, angle2);
    }

    private Vector3 CalculatePositionAtTime(float t, float angle)
    {
        float x = _initialSpeed * t * Mathf.Cos(angle);
        float y = _initialSpeed * t * Mathf.Sin(angle) - 0.5f * gravity * t * t;
        return new Vector3(_arrowStartPosition.position.x + x, _arrowStartPosition.position.y + y, 0);
    }

    bool IsTargetReachable(Vector2 startPosition, Vector2 targetPosition)
    {
        float distanceX = targetPosition.x - startPosition.x;
        float distanceY = targetPosition.y - startPosition.y;

        float velocitySquared = _initialSpeed * _initialSpeed;
        float gravitySquared = gravity * gravity;

        float discriminant = velocitySquared * velocitySquared - gravitySquared * (distanceX * distanceX + 2 * distanceY * velocitySquared / gravity);

        return discriminant >= 0;
    }

    private void LaunchArrow(float angle)
    {
        GameObject arrow = Instantiate(_arrowPrefab, _arrowStartPosition.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        Vector2 velocity = new Vector2(_initialSpeed * Mathf.Cos(angle), _initialSpeed * Mathf.Sin(angle));
        rb.velocity = velocity;

        arrow.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        StartCoroutine(UpdateArrowRotation(arrow));
    }

    IEnumerator UpdateArrowRotation(GameObject arrow)
    {
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        while (arrow != null)
        {
            // Обновляем ориентацию стрелы в направлении её скорости
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }
    }
}