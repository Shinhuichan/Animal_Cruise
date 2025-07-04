using CustomInspector; // 기존에 사용하던 네임스페이스 유지
using UnityEngine;
using TMPro;

public class GimmickWeightPanel : MonoBehaviour
{
    
    [Header("Panel Setting")]
    [SerializeField] private float triggerWeight = 10f; // 패널이 움직이기 시작하는 임계 무게
    [SerializeField] private float panelMovingSpeed = 3f; // 패널이 이동하는 속도 (Lerp의 factor에 곱해짐)
    [SerializeField] private bool colliderOff = false;
    [Header("Target Setting")]
    [SerializeField] private Transform targetTransform; // 패널이 내려갈 목표 지점 Transform


    [ReadOnly] public float characterTotalWeight = 0f; // ReadOnly 속성 유지
    private Vector3 initialPosition;
    private bool trigger = false;


    void Awake()
    {
        initialPosition = transform.position;

        if (targetTransform == null)
            Debug.LogError("GimmickPanel 스크립트: targetTransform이 할당되지 않았습니다! 패널이 내려갈 목표 지점이 없습니다.", this);
    }
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        Vector3 targetPosition; // 이번 프레임에 패널이 이동할 목표 위치

        // 1. characterTotalWeight가 triggerWeight 이상인지 확인
        if (characterTotalWeight >= triggerWeight)
        {
            if (colliderOff)
                trigger = true;
            // 조건 충족: 패널을 targetTransform의 위치로 이동시켜야 합니다.
            if (targetTransform != null)
                targetPosition = targetTransform.position;
            else
                targetPosition = transform.position;
        }
        else
        {
            targetPosition = initialPosition;
            trigger = false;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panelMovingSpeed);
        if (colliderOff)
        {
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = !trigger;
        }
    }
}