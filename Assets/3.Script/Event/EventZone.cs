using System.Collections;
using UnityEngine;


public class EventZone : MonoBehaviour
{

    #region EVENTS
    [SerializeField] private EventMoveTo eventMoveTo;
    [SerializeField] private EventMoveBack eventMoveBack;
    [SerializeField] private EventAnimation eventAnimation;
    [SerializeField] private EventAnimationBack eventAnimationBack;




    #endregion
    [SerializeField] GameObject InteractiveTarget;
   


     private Animator animationTarget;
    private CharacterControl player;






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
        player = other.gameObject.GetComponentInParent<CharacterControl>();
        if (player != null && other.tag == "Player")
        {
            eventMoveTo?.Raise();
            eventAnimation?.Raise();
       }

    }

    void OnTriggerStay(Collider other)
    {
        player = other.gameObject.GetComponentInParent<CharacterControl>();
        if (player != null && other.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        player = other.gameObject.GetComponentInParent<CharacterControl>();
        if (player != null && other.tag == "Player")
        {
            eventMoveBack?.Raise();
            eventAnimationBack?.Raise();
        }
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
