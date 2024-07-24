using System.ComponentModel.Design;
using UnityEngine;

public class ShroomEnemy : MonoBehaviour, ITargetable
{
    [SerializeField] private float _delayBeforeLeaving = 5f;

    private float _timer = 0f;

    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }

    public void ExecuteHitEction(int damage)
    {
        Debug.Log($"I took {damage} damage");
        Destroy(gameObject);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _delayBeforeLeaving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ArrowConroller arrowConroller;

        if (other.gameObject.TryGetComponent<ArrowConroller>(out arrowConroller))
        {
            ExecuteHitEction(arrowConroller.ArrowData.Damage);
            arrowConroller.DestroyArrow();
        }
    }
}
