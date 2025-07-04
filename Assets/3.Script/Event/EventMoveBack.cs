using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventMoveBack")]
public class EventMoveBack : GameEvent<EventMoveBack>
{
    public override EventMoveBack Item => this;

    [Space(20)]

    [Tooltip("Accelerative Moving")]  public bool isAccelerated = false;
}