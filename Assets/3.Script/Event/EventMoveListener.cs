using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMoveListener : MonoBehaviour
{
    #region EVENTS
    [SerializeField] private EventMoveTo eventMoveTo;
    [SerializeField] private EventMoveBack eventMoveBack;

    #endregion

    private EventZone eventZone;
    private CharacterControl player;


    private Vector3 movestartPoint = Vector3.zero;
    private Vector3 moveendPoint = Vector3.zero;
    private float moveDistance;
    private Rigidbody rb = null;
    private float speed = 0f;

    private Coroutine co = null;


    void OnEnable()
    {
        eventMoveTo?.Register(OneventMoveTo);
        eventMoveBack?.Register(OneventMoveBack);


        EventSet();
    }
    void OnDisable()
    {
        eventMoveTo?.Unregister(OneventMoveTo);
        eventMoveBack?.Unregister(OneventMoveBack);
    }
    void EventSet()
    {
        TryGetComponent(out rb);
        movestartPoint = rb.position;
        moveendPoint = eventMoveTo.arrivalPoint;
        speed = eventMoveTo.speed;
        moveDistance = Vector3.Distance(movestartPoint, moveendPoint);
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("부딪힘");
        player = collision.gameObject.GetComponentInParent<CharacterControl>();
        if (player.tag == "Player")
        {
            player.gameObject.SetActive(false);
        }
        StopCoroutine(co);  
    }

    float _elapsedTime;
    IEnumerator MoveUpdate(Vector3 arrival, bool isaccel)
    {
        Vector3 dir = (arrival - rb.position).normalized;
        while (Vector3.Distance(rb.position, arrival) >= 0.2f)
        {

            if (!isaccel)
            {
                _elapsedTime += Time.fixedDeltaTime;
                //Vector3.MoveTowards(rb.position, arrival, 0.01f);
                Vector3 move = dir * speed * Time.fixedDeltaTime;
                rb.transform.LookAt(dir);
                rb.MovePosition(rb.position + move);
                yield return new WaitForFixedUpdate();
                if (_elapsedTime > moveDistance / move.magnitude * Time.fixedDeltaTime)
                    break;

            }
            else
            {
                _elapsedTime += Time.fixedDeltaTime;
                Vector3 move = dir * speed * Time.fixedDeltaTime * _elapsedTime;
                rb.transform.LookAt(dir);

                rb.MovePosition(rb.position + move);
                yield return new WaitForFixedUpdate();
                if (_elapsedTime > moveDistance / move.magnitude * Time.fixedDeltaTime)
                    break;
            }
        }

        _elapsedTime = 0f;


        Debug.Log("목적지에 도착했습니다.");
        yield return null;
        StopCoroutine(co);
        co = null;

    }

    void OneventMoveTo(EventMoveTo e)
    {
        // 예외처리 필요


        Debug.Log("움직이는 중");
        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(MoveUpdate(moveendPoint, e.isAccelerated));

    }
    void OneventMoveBack(EventMoveBack e)
    {
        Debug.Log("돌아가는 중");
        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(MoveUpdate(movestartPoint, e.isAccelerated));

    }
}
