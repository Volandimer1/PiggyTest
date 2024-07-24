using UnityEngine;

public class ShroomsFactory
{
    private ShroomEnemy _shroomEnemyPrefab;

    public ShroomsFactory(ShroomEnemy shroomEnemyPrefab)
    {
        _shroomEnemyPrefab = shroomEnemyPrefab;
    }

    public ShroomEnemy GetShroom(Vector3 position)
    {
        ShroomEnemy shroom = Object.Instantiate(_shroomEnemyPrefab);
        shroom.Initialize(position);

        return shroom;
    }
}
