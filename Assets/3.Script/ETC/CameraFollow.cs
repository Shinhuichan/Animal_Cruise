using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCam;

    public float cameraMaxPov = 1000f;
    [SerializeField] private float fovIncreaseSpeed = 2f;
    [SerializeField] private float fovDecreaseSpeed = 5f;
    [SerializeField] private float initialFOV = 60f;

    [SerializeField] private LayerMask obstacleLayer;

    // ===> 카메라의 Y축 위치 조절을 위한 변수 <===
    [SerializeField] private float cameraHeightOffset = 5f; // 캐릭터 평균 Y 위치에서 얼마나 높이 카메라가 위치할지 오프셋
    [SerializeField] private float cameraYMoveSpeed = 2f; // 카메라 Y축 이동 속도

    private float currentFOV;
    private float targetCameraY; // 카메라가 목표로 할 Y 위치

    void Awake()
    {
        // 씬에 "MainCamera" 태그가 붙은 카메라를 찾아옴
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("씬에서 'MainCamera' 태그가 붙은 카메라를 찾을 수 없습니다!");
            enabled = false;
            return;
        }

        // 초기 FOV 설정 (인스펙터에서 설정한 initialFOV 사용)
        currentFOV = initialFOV;
        mainCam.fieldOfView = currentFOV; // 카메라의 실제 FOV에 적용

        // ===> 초기 카메라 Y 위치 설정 <===
        // 시작 시 캐릭터들이 있다면 그들의 평균 Y 위치에 맞춰 초기 Y 위치 설정
        CharacterControl[] characters = FindObjectsByType<CharacterControl>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (characters.Length > 0)
        {
            float totalYAxis = 0f;
            foreach (var cha in characters)
            {
                 if (cha != null && cha.gameObject.activeInHierarchy)
                 {
                    totalYAxis += cha.gameObject.transform.position.y;
                 }
            }
            float averageYAxis = totalYAxis / characters.Length;
            targetCameraY = averageYAxis + cameraHeightOffset; // 캐릭터 평균 Y + 오프셋
             // 시작 시 카메라 Y 위치 바로 설정
             mainCam.gameObject.transform.position = new Vector3(mainCam.gameObject.transform.position.x, targetCameraY, mainCam.gameObject.transform.position.z);
        }
        else
        {
             // 캐릭터가 없을 때는 초기 Y 위치를 현재 카메라 Y 위치 + 오프셋으로 설정
             targetCameraY = mainCam.gameObject.transform.position.y; // 또는 원하는 기본값
        }
    }

    void Update()
    {
        // 매 프레임 카메라 추적 및 FOV 조절 로직 실행
        Follow();
    }

    void Follow()
    {
        CharacterControl[] characters = FindObjectsByType<CharacterControl>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        if (characters.Length == 0 || mainCam == null)
        {
             // 캐릭터가 없을 때는 FOV를 원래대로 되돌릴 수도 있어.
             currentFOV = Mathf.Lerp(currentFOV, initialFOV, Time.deltaTime * 10*fovDecreaseSpeed);
             mainCam.fieldOfView = Mathf.Clamp(currentFOV, initialFOV, cameraMaxPov);
             // ===> 캐릭터 없을 때 카메라 Y 위치도 원래 위치나 특정 위치로 되돌릴 수 있어 <===
             // targetCameraY = 원하는 기본 Y 위치 + cameraHeightOffset;
             mainCam.gameObject.transform.position = new Vector3(mainCam.gameObject.transform.position.x, Mathf.Lerp(mainCam.gameObject.transform.position.y, targetCameraY, Time.deltaTime * cameraYMoveSpeed), mainCam.gameObject.transform.position.z);

             return;
        }

        // 2. 캐릭터들의 평균 X, Y 위치 계산
        float totalXAxis = 0f;
        // ===> Y축 평균 계산을 위한 변수 추가 <===
        float totalYAxis = 0f;
        int activeCharacterCount = 0; // 활성화된 캐릭터의 실제 수를 세기 위한 변수

        foreach (var cha in characters)
        {
            // 혹시 비활성화된 캐릭터가 있다면 스킵 (FindObjectsInactive.Exclude 설정과 일치)
            if (cha != null && cha.gameObject.activeInHierarchy)
            {
                totalXAxis += cha.gameObject.transform.position.x;
                // ===> 활성화된 캐릭터의 Y 위치 누적 <===
                totalYAxis += cha.gameObject.transform.position.y;
                activeCharacterCount++; // 활성화된 캐릭터 카운트
            }
        }

        // 활성화된 캐릭터가 없다면 함수 종료 (이미 위에서 처리했지만 한 번 더 확인)
        if (activeCharacterCount == 0)
        {
             // 캐릭터 없을 때 처리 로직 다시 넣어주거나 위에서 확실히 처리
             currentFOV = Mathf.Lerp(currentFOV, initialFOV, Time.deltaTime * fovDecreaseSpeed);
             mainCam.fieldOfView = Mathf.Clamp(currentFOV, initialFOV, cameraMaxPov);
             // targetCameraY = 원하는 기본 Y 위치 + cameraHeightOffset;
             mainCam.gameObject.transform.position = new Vector3(mainCam.gameObject.transform.position.x, Mathf.Lerp(mainCam.gameObject.transform.position.y, targetCameraY, Time.deltaTime * cameraYMoveSpeed), mainCam.gameObject.transform.position.z);

             return;
        }


        float averageXAxis = totalXAxis / activeCharacterCount; // 실제 활성화된 캐릭터 수로 나눔
        // ===> Y축 평균 계산 <===
        float averageYAxis = totalYAxis / activeCharacterCount;

        // ===> 카메라 목표 Y 위치 계산 (평균 Y 위치에 오프셋 더하기) <===
        targetCameraY = averageYAxis + cameraHeightOffset;


        // ===> 카메라 X, Y 위치 업데이트 <===
        // 카메라의 현재 위치
        Vector3 currentCamPos = mainCam.gameObject.transform.position;
        // 목표 위치 (X는 평균 X, Y는 목표 Y, Z는 그대로)
        Vector3 targetCamPos = new Vector3(averageXAxis, targetCameraY, currentCamPos.z);

        // Lerp를 사용하여 카메라 위치를 부드럽게 이동
        // X축 이동 속도와 Y축 이동 속도를 다르게 적용하고 싶다면 각각 Lerp를 사용할 수 있습니다.
        // 여기서는 일단 Y축 이동 속도 변수를 사용하여 Y 위치만 부드럽게 따라가도록 합니다.
        mainCam.gameObject.transform.position = new Vector3(
            averageXAxis, // X축은 바로 평균 위치로 이동
            Mathf.Lerp(currentCamPos.y, targetCameraY, Time.deltaTime * cameraYMoveSpeed), // Y축은 부드럽게 이동
            currentCamPos.z // Z축은 그대로 유지
        );


        // ===> 3. 모든 캐릭터가 카메라 시야에 들어오는지 확인 및 FOV 조절 <===
        // FOV 조절 로직은 이전 코드와 동일하므로 그대로 유지합니다.
        // ... (이전 코드 복사 붙여넣기) ...
        int visibleCharacterCount = 0; // 시야 내에 있고 가려지지 않은 캐릭터 수

        foreach (var cha in characters)
        {
            // 혹시 비활성화된 캐릭터나 null인 경우 스킵
            if (cha == null || !cha.gameObject.activeInHierarchy) continue;

            // 3-1. 캐릭터 위치가 카메라의 시야 범위 (Frustum) 안에 있는지 확인
            Vector3 viewportPoint = mainCam.WorldToViewportPoint(cha.gameObject.transform.position);

            bool isWithinFrustum = viewportPoint.z > 0 &&
                                   viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                                   viewportPoint.y >= 0 && viewportPoint.y <= 1;

            // 3-2. 시야 범위 안에 있다면, 카메라에서 캐릭터까지 장애물이 있는지 Raycast로 확인
            if (isWithinFrustum)
            {
                 // Raycast 시작점: 카메라 위치
                 Vector3 rayOrigin = mainCam.transform.position;
                 // Ray 방향: 카메라에서 캐릭터 위치로
                 Vector3 rayDirection = cha.gameObject.transform.position - rayOrigin;
                 // Ray 길이: 카메라에서 캐릭터 위치까지의 거리
                 float rayDistance = rayDirection.magnitude;

                 RaycastHit hit;
                 // 만약 rayDistance가 0이면 (카메라와 캐릭터 위치가 같으면) Raycast는 항상 false를 반환하므로 체크 필요.
                 bool isBlocked = (rayDistance > 0.001f) && Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, obstacleLayer);

                 Color rayColor = isBlocked ? Color.red : Color.green;
                 Debug.DrawRay(rayOrigin, rayDirection, rayColor);


                 // 3-3. 시야 범위 안에 있고 장애물에 가려지지 않았다면 Visible 캐릭터로 간주
                 if (!isBlocked) // 장애물에 가려지지 않았다면
                     visibleCharacterCount++;
            }
        }

        // 4. 가시성 체크 결과에 따라 FOV 조절
        if (visibleCharacterCount < activeCharacterCount) // 시야 내에 있고 가려지지 않은 캐릭터 수가 전체 활성화된 캐릭터 수보다 적다면
            // FOV를 cameraMaxPov 쪽으로 넓힘
            currentFOV = Mathf.Lerp(currentFOV, cameraMaxPov, Time.deltaTime * fovIncreaseSpeed);
        else // 모든 캐릭터가 시야 내에 있고 가려지지 않았다면
            // FOV를 initialFOV 쪽으로 좁힘 (캐릭터들이 모였을 때)
            currentFOV = Mathf.Lerp(currentFOV, initialFOV, Time.deltaTime * fovDecreaseSpeed);

        // 5. 계산된 currentFOV 값을 실제 카메라 FOV에 적용하되, initialFOV ~ cameraMaxPov 범위로 제한
        mainCam.fieldOfView = Mathf.Clamp(currentFOV, initialFOV, cameraMaxPov);

    }
}