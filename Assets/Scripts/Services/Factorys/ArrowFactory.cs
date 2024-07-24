using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory
{
    private ArrowPooler _pooler;
    private ArrowConroller _arrowPrefab;
    private Dictionary<ArrowType, ArrowData> _arrowDataDictionary;

    public ArrowFactory(ArrowConroller arrowPrefab)
    {
        _arrowPrefab = arrowPrefab;
        _arrowDataDictionary = new Dictionary<ArrowType, ArrowData>
        {
            { ArrowType.Default, new ArrowData(ArrowType.Default, 10) },
            { ArrowType.Heavy, new ArrowData(ArrowType.Heavy, 20) },
            { ArrowType.Fire, new ArrowData(ArrowType.Fire, 15) },
            { ArrowType.Ice, new ArrowData(ArrowType.Ice, 12) }
        };
    }

    public void SetPooler(ArrowPooler pooler)
    {
        _pooler = pooler;
    }

    public ArrowConroller GetArrow(ArrowType arrowType, Vector3 arrowStartPosition, float shootingAngle, float initialSpeed)
    {
        ArrowConroller arrow = null;

        if (_arrowDataDictionary.TryGetValue(arrowType, out ArrowData arrowData))
        {
            arrow = Object.Instantiate(_arrowPrefab).GetComponent<ArrowConroller>();
            arrow.Initialize(_pooler, arrowStartPosition, shootingAngle, initialSpeed, arrowData);
        }
        else
        {
            Debug.LogError($"ArrowData for type {arrowType} not found.");
        }

        return arrow;
    }
}