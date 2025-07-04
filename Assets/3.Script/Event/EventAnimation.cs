using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent/EventAnimation")]
public class EventAnimation : GameEvent<EventAnimation>
{
    public override EventAnimation Item => this;
    public string animationName;
    
}