using System.Collections.Generic;
using UnityEngine;

public class ArrowPooler 
{
    private ArrowFactory _arrowFactory;
    private Dictionary<ArrowType, Queue<ArrowConroller>> _arrowPools;

    public ArrowPooler(ArrowFactory arrowFactory)
    {
        _arrowFactory = arrowFactory;
        _arrowPools = new Dictionary<ArrowType, Queue<ArrowConroller>>();
        foreach (ArrowType arrowType in System.Enum.GetValues(typeof(ArrowType)))
        {
            _arrowPools[arrowType] = new Queue<ArrowConroller>();
        }
    }

    public ArrowConroller GetArrow(ArrowType arrowType, Vector3 arrowStartPosition, float shootingAngle, float initialSpeed)
    {
        if (_arrowPools[arrowType].Count > 0)
        {
            ArrowConroller arrow = _arrowPools[arrowType].Dequeue();
            arrow.gameObject.SetActive(true);
            arrow.Initialize(this, arrowStartPosition, shootingAngle, initialSpeed, arrow.ArrowData);
            return arrow;
        }
        else
        {
            return _arrowFactory.GetArrow(arrowType, arrowStartPosition, shootingAngle, initialSpeed);
        }
    }

    public void ReturnArrow(ArrowConroller arrow)
    {
        arrow.gameObject.SetActive(false);
        _arrowPools[arrow.ArrowData.ArrowType].Enqueue(arrow);
    }
}