using System.Collections;
using UnityEngine;


public class EventButton : MonoBehaviour
{

    #region EVENTS
    [SerializeField] private EventMoveTo eventMoveTo;
    [SerializeField] private EventMoveBack eventMoveBack;
    [SerializeField] private EventAnimation eventAnimation;
    [SerializeField] private EventAnimationBack eventAnimationBack;




    #endregion
    [SerializeField] GameObject InteractiveTarget;
    private CharacterControl player;
    [SerializeField] bool isAccelerated = false;


    private Vector3 movestartPoint = Vector3.zero;
    private Vector3 moveendPoint = Vector3.zero;
    private Rigidbody rb = null;
    private float speed = 0f;
    private float moveDistance;

    [SerializeField] private Animator animationTarget;


    private Coroutine co = null;


    void OnEnable()
    {

        eventAnimation?.Register(OneventPlayAnimation);
        eventAnimationBack?.Register(OneventPlayBackAnimation);


        EventSet();

    }

    void OnDisable()
    {
        eventAnimation?.Unregister(OneventPlayAnimation);
        eventAnimationBack?.Unregister(OneventPlayBackAnimation);



    }

    void EventSet()
    {


        InteractiveTarget.TryGetComponent(out animationTarget);
    }




    void OnTriggerEnter(Collider other)
    {
        
            eventMoveTo?.Raise();
            eventAnimation?.Raise();
        
    }

    void OnTriggerExit(Collider other)
    {
       
            eventMoveBack?.Raise();
            eventAnimationBack?.Raise();
        
    }





    void OneventPlayAnimation(EventAnimation e)
    {
        animationTarget?.CrossFadeInFixedTime(e.animationName, 0.1f);

    }
    void OneventPlayBackAnimation(EventAnimationBack e)
    {
        animationTarget?.CrossFadeInFixedTime(e.animationName, 0.1f);

    }


}
