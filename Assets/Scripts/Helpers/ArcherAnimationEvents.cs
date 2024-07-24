using UnityEngine;

public class ArcherAnimationEvents : MonoBehaviour
{
    [SerializeField] private ArcherTriggerHandler _archerTriggerHandler;

    public void ShootArrow()
    {
        _archerTriggerHandler.TriggerArrowShootEvent();
    }
}
