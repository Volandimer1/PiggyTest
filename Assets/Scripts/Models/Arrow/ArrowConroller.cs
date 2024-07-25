using UnityEngine;

public class ArrowConroller : MonoBehaviour, IHaveDamage
{
    public ArrowData ArrowData { get; private set; }
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private ArrowPooler _pooler;

    public void Initialize(ArrowPooler pooler, Vector3 arrowStartPosition, float angle, float initialSpeed, ArrowData arrowData)
    {
        _pooler = pooler;
        transform.position = arrowStartPosition;
        this.ArrowData = arrowData;
        Vector2 velocity = new Vector2(initialSpeed * Mathf.Cos(angle), initialSpeed * Mathf.Sin(angle));
        _rigidbody2D.velocity = velocity;

        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }

    public int GetDamageValue()
    {
        DestroyArrow();
        return ArrowData.Damage;
    }

    public void DestroyArrow()
    {
        _pooler.ReturnArrow(this);
    }

    private void Update()
    {
        // Обновляем ориентацию стрелы в направлении её скорости
        float angle = Mathf.Atan2(_rigidbody2D.velocity.y, _rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        DestroyIfOutOfBounds();
    }

    private void DestroyIfOutOfBounds()
    {
        if (transform.position.x > 11f || transform.position.x < -11f ||
            transform.position.y > 5.5f || transform.position.x < -5.5f)
        {
            DestroyArrow();
        }
    }
}