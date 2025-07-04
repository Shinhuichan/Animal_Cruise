using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventMoveTo")]
public class EventMoveTo : GameEvent<EventMoveTo>
{
    public override EventMoveTo Item => this;

    [Space(20)]
    // 도착지
    [Tooltip("Move to")] public Vector3 arrivalPoint;
    // 걸리는 시간
    [Tooltip("Move speed")] public float speed = 5f;
    // 가속 유무
    [Tooltip("Accelerative Moving")]  public bool isAccelerated = false;
    
}