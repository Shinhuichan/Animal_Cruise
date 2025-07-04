using UnityEngine;

public class AbilityCrawl : Ability<AbilityCrawlData>
{
    Vector3 direction;
    private Transform cameraTransform;
    float horizontalInput;
    float verticalInput;

    // 생성자
    public AbilityCrawl(AbilityCrawlData data, CharacterControl owner) : base(data, owner)
    {
        // owner는 CharacterControl 스크립트 인스턴스라고 가정
        if (owner.Profile == null) return;

        // 해당 Character의 Profile의 데이터를 Data 속성에 대입
        Data.crawlmovePerSec = owner.Profile.moveSpeed * owner.Profile.crawlMoveIncrese;
        Data.rotatePerSec = owner.Profile.rotateSpeed;

        // 카메라 Transform 초기화 (생성자에서 하는 경우 카메라가 먼저 활성화되어 있어야 합니다)
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogWarning("씬에 'MainCamera' 태그를 가진 카메라가 없습니다. 웅크리기 중 이동이 제한될 수 있습니다.");
    }

    public override void Update()
    {
        var keyManager = KeySettingManager.Instance;
        // 카메라가 없다면 이동 입력을 처리하지 않음
        if (cameraTransform == null)
        {
            horizontalInput = 0f; // 입력값 초기화
            verticalInput = 0f;
            direction = Vector3.zero; // 방향 초기화
            return; // 카메라 없으면 Update 로직 종료
        }

        // ===> 이동 입력 값 계산 <===
        horizontalInput = 0f;
        verticalInput = 0f;

        if (Input.GetKey(keyManager.GetKey(BindingType.MoveUp))) verticalInput = 1f;
        else if (Input.GetKey(keyManager.GetKey(BindingType.MoveDown))) verticalInput = -1f;
        if (Input.GetKey(keyManager.GetKey(BindingType.MoveRight))) horizontalInput = 1f;
        else if (Input.GetKey(keyManager.GetKey(BindingType.MoveLeft))) horizontalInput = -1f;

        // ===> 웅크리기 상태를 결정 (키 입력 또는 강제 상태) <===
        // 왼쪽 컨트롤 키가 눌려 있거나 (Input.GetKey(KeyCode.LeftControl))
        // mustCrawling 변수가 true이면 (mustCrawling == true)
        // 캐릭터는 웅크려야 하는 상태라고 판단합니다.
        bool shouldBeCrawlingThisFrame = Input.GetKey(keyManager.GetKey(BindingType.Skill)) || owner.mustCrawling;

        // ===> 현재 상태와 목표 상태를 비교하여 상태 전환 <===
        // 만약 웅크려야 하는데 현재 웅크리기 상태가 아니라면 웅크리기 시작
        if (shouldBeCrawlingThisFrame && !owner.isCrowling && !owner.isJumping)
             SetCrawlState(true); // 웅크리기 상태로 설정
        // 만약 웅크릴 필요가 없는데 현재 웅크리기 상태라면 웅크리기 해제
        else if (!shouldBeCrawlingThisFrame && owner.isCrowling)
            SetCrawlState(false); // 기본 상태로 설정
        // 그 외의 경우 (이미 목표 상태와 같으면) 아무것도 하지 않음

        // ===> 이동 방향 계산 (현재 웅크리기 상태일 때만) <===
        // 위에서 SetCrawlState 호출로 owner.isCrowling 상태가 업데이트되었을 수 있습니다.
        if (owner.isCrowling)
        {
             Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
             Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;
             // 주의: 이전 코드에서 Data.crawlmovePerSec를 여기서 곱했는데,
             //       이것은 방향 벡터 계산이므로 속도(magnitude)는 CrawlMove에서 곱해야 합니다.
             direction = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized; // 방향만 사용
        }
        else
             // 웅크리기 상태가 아닐 때는 웅크리기 관련 이동 방향은 0으로 설정
             direction = Vector3.zero;
    }

    public override void FixedUpdate()
    {
        if (!owner.isLocalPlayer) return;
        // ===> 현재 웅크리기 상태일 때만 물리적 이동 및 회전 적용 <===
        if (owner.isCrowling && !owner.isJumping)
        {
            CrawlMove();
            CrawlRotate();
        }
        // 웅크리기 상태가 아니면 CrawlMove/CrawlRotate 호출 안 함
    }

    // CrawlMove 함수 (웅크리기 상태일 때 물리 이동 처리)
    void CrawlMove()
    {

        // direction.magnitude > 0f: 이동 입력이 있을 때만 움직임 계산
        if (direction.magnitude > 0f)
        {
            Vector3 desiredVelocity = direction * Data.crawlmovePerSec; // 목표 속도 (방향 * 초당 이동 속도)

            Vector3 currentXZVelocity = new Vector3(owner.rb.velocity.x, 0f, owner.rb.velocity.z);
            Vector3 targetXZVelocity = new Vector3(desiredVelocity.x, 0f, desiredVelocity.z);

            float velocityLerpFactor = 10f; // 속도 전환 속도
            Vector3 smoothedXZVelocity = Vector3.Lerp(currentXZVelocity, targetXZVelocity, Time.fixedDeltaTime * velocityLerpFactor);

            // Rigidbody 속도 업데이트 (Y 속도는 유지)
            owner.rb.velocity = new Vector3(smoothedXZVelocity.x, owner.rb.velocity.y, smoothedXZVelocity.z);
        }
        else
        {
            // 움직임 입력이 없을 때는 XZ 속도를 0으로 부드럽게 감속
            Vector3 currentXZVelocity = new Vector3(owner.rb.velocity.x, 0f, owner.rb.velocity.z);
            Vector3 targetXZVelocity = Vector3.zero;

            float velocityLerpFactor = 10f;
            Vector3 smoothedXZVelocity = Vector3.Lerp(currentXZVelocity, targetXZVelocity, Time.fixedDeltaTime * velocityLerpFactor);

            owner.rb.velocity = new Vector3(smoothedXZVelocity.x, owner.rb.velocity.y, smoothedXZVelocity.z);
        }

    }

    // CrawlRotate 함수 (웅크리기 상태일 때 물리 회전 처리)
    void CrawlRotate()
    {
        if (direction == Vector3.zero || !owner.isCrowling)
            return;
        
            // direction == Vector3.zero: 움직임 입력이 없을 때는 회전 안 함
            // owner.isCrowling: FixedUpdate에서 이미 체크하지만 안전하게 다시 체크

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Mathf.SmoothDampAngle에 사용할 속도 변수는 함수 밖에서 관리하거나 다른 방법 고려 필요
            float currentVelocity = 0f; // 간단한 예시를 위해 지역 변수 사용

            float smoothAngle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.1f);
            owner.transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        
    }

    // ===> 웅크리기 상태를 실제로 변경하고 콜라이더/애니메이션을 설정하는 함수 <===
    // isCrawlingState 매개변수가 '목표 상태'를 나타냅니다.
    void SetCrawlState(bool targetIsCrawlingState)
    {
        // 이미 목표 상태와 같다면 아무것도 하지 않고 종료 (중복 호출 방지)
        if (owner.isCrowling == targetIsCrawlingState) return;

        // ===> 상태 변경 적용 <===
        owner.isCrowling = targetIsCrawlingState; // CharacterControl의 웅크리기 상태 업데이트

        // ===> 콜라이더 활성/비활성화 <===
        if (owner.normalCol != null && owner.crawlCol != null)
        {
            owner.normalCol.enabled = !targetIsCrawlingState; // 기본 콜라이더는 목표 상태의 반대로
            owner.crawlCol.enabled = targetIsCrawlingState; // 웅크리기 콜라이더는 목표 상태와 같게
        }
        else
            Debug.LogError("CharacterControl에 normalCol 또는 crawlCol이 할당되지 않았습니다.");

        // ===> Rigidbody 중력 사용 여부 변경 (게임 디자인에 따라 다름) <===
        // if (owner.rb != null)
        //     owner.rb.useGravity = !targetIsCrawlingState; // 웅크릴 때(true)는 중력 끄고, 기본 상태일 때(false)는 중력 켬


        // ===> 상태에 따라 애니메이션 재생 및 추가 처리 <===
        if (targetIsCrawlingState) // 웅크리기 상태로 전환될 때
                                   //owner.AnimateTrigger("Move2Crawl");
            owner.CMDAnimationTrigger("Move2Crawl");
        else // 기본 상태로 전환될 때 (웅크리기 해제)
        {
            //owner.AnimateTrigger("Crawl2Move");
            owner.CMDAnimationTrigger("Crawl2Move");

            if (owner.rb != null)
                owner.rb.velocity = new Vector3(0f, owner.rb.velocity.y, 0f);
        }
    }
}