using UnityEngine;

public class SomeEntryPointSImulation : MonoBehaviour
{
    [SerializeField] private Updater _updater;
    [SerializeField] private ArcherTriggerHandler _archerPrefab;
    [SerializeField] private ArrowConroller _arrowPrefab;
    [SerializeField] private ShroomEnemy _shroomEnemyPrefab;
    [SerializeField] private ShroomsSpawner _shroomsSpawner;

    private void Start()
    {
        ArrowFactory arrowFactory = new ArrowFactory(_arrowPrefab);
        ArrowPooler arrowPooler = new ArrowPooler(arrowFactory);
        arrowFactory.SetPooler(arrowPooler);

        ArcherTriggerHandler archer = Instantiate(_archerPrefab);
        archer.Initialize(_updater, arrowPooler);

        ShroomsFactory shroomsFactory = new ShroomsFactory(_shroomEnemyPrefab);
        _shroomsSpawner.Initialize(shroomsFactory);
    }
}
