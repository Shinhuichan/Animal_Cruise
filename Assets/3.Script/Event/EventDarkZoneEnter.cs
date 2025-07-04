using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventDarkZoneEnter")]
public class EventDarkZoneEnter : GameEvent<EventDarkZoneEnter>
{
    public override EventDarkZoneEnter Item => this;
    public string animationName;

}
