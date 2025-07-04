using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventAnimationBack")]
public class EventAnimationBack : GameEvent<EventAnimationBack>
{
    public override EventAnimationBack Item => this;
    public string animationName;

}