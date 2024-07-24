using UnityEngine;

public class ShroomsSpawner : MonoBehaviour
{
    [SerializeField] private float _boundsHeight = 5f;
    [SerializeField] private float _boundsWidth = 3f;
    [SerializeField] private float _spawnDelay = 2f;

    private ShroomsFactory _shroomsFactory;
    private float _timer = 0f;

    public void Initialize(ShroomsFactory shroomsFactory)
    {
        _shroomsFactory = shroomsFactory;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnDelay)
        {
            _timer = 0f;
            Vector3 randomSpawnPoint = new Vector3(Random.Range(transform.position.x - _boundsWidth, transform.position.x + _boundsWidth),
                                                   Random.Range(transform.position.y - _boundsHeight, transform.position.y + _boundsHeight),
                                                   0);
            _shroomsFactory.GetShroom(randomSpawnPoint);
        }
    }
}
