using UnityEngine;

public class AbilityJump : Ability<AbilityJumpData> // Ability<T>는 MonoBehaviour를 상속받지 않음
{
    // ===> public으로 노출해서 다른 곳에서 점프 상태를 알 수 있도록 함 <===
    private float _elapsedTime = 0f; // 경과 시간

    // ===> 추가된 변수: 이전 프레임의 isGrounded 상태 저장 <===
    private bool wasGroundedLastFrame;


    // 생성자
    public AbilityJump(AbilityJumpData data, CharacterControl owner) : base(data, owner)
    {
        // owner는 CharacterControl 스크립트 인스턴스라고 가정
        if (owner.Profile == null) return;

        // 해당 Character의 Profile의 데이터를 Data 속성에 대입
        Data.jumpForce = owner.Profile.jumpForce;
        Data.jumpDuration = owner.Profile.jumpDuration;
    }

    // CharacterControl의 Start() 함수에서 이 Ability가 초기화될 때 호출되면 좋을 것 같습니다.
    public void Initialize()
    {
        // ===> 게임 시작 시 초기 isGrounded 상태 저장 <===.
        wasGroundedLastFrame = owner.isGrounded;
    }

    public override void Update()
    {
        if (!owner.isLocalPlayer) return;
        var keyManager = KeySettingManager.Instance;
        // ===> 점프 착지 감지 및 JumpDown 애니메이션 재생 <===
        if (owner.isJumping && !wasGroundedLastFrame && owner.isGrounded)
            JumpDown(); // JumpDown 로직 실행

        // ===> 점프 입력 감지 및 JumpUp 시작 <===
        if (Input.GetKeyDown(keyManager.GetKey(BindingType.Jump)) && owner.isGrounded && !owner.isJumping && !owner.isCrowling)
            JumpUp(); // 점프 시작 로직을 처리하는 메소드 호출

        // ===> 현재 프레임의 isGrounded 상태를 다음 프레임을 위해 저장 <===
        wasGroundedLastFrame = owner.isGrounded;
    }

    // FixedUpdate는 물리 업데이트 주기에 맞춰 호출 (CharacterControl에서 호출해 줄 거라고 가정)
    public override void FixedUpdate()
    {
        if (!owner.isLocalPlayer) return;
        if (owner.isJumping && owner.rb != null)
        {
            // FixedUpdate 주기를 따라 경과 시간 증가 (물리 연산과 동기화)
            _elapsedTime += Time.fixedDeltaTime;

            if (_elapsedTime < Data.jumpDuration)
            {
                float t = Mathf.Clamp01(_elapsedTime / Data.jumpDuration); // 0에서 1 사이의 비율 계산

                // AnimationCurve의 값에 jumpForce를 곱해서 원하는 Y 속도 계산
                // Curve의 Y축 값은 보통 0~1 사이의 값으로 설정하여 점프 높이/속도 형태를 만듦.
                float desiredJumpVelocityY = Data.jumpCurve.Evaluate(t) * Data.jumpForce;

                // Rigidbody의 현재 XZ 속도는 유지하고, Y 속도만 계산된 값으로 설정
                owner.rb.velocity = new Vector3(owner.rb.velocity.x, desiredJumpVelocityY, owner.rb.velocity.z);
            }
        }
    }

    // 점프 시작 시 호출되는 메소드
    public void JumpUp()
    {
        // owner.rb가 null이거나 이미 점프 중이거나 웅크리기 상태라면 시작하지 않음 (Update에서 체크했지만 안전 장치)
        if (owner.rb == null || owner.isJumping || owner.isCrowling) return;
        // owner.isGrounded 체크는 Update 입력 부분에서 이미 했으므로 여기서 굳이 다시 할 필요는 없습니다.

        // ===> 점프 시작! <===
        owner.isJumping = true; // 점프 중 플래그 켜기
        owner.canJumping = false; // JumpDown이 호출되기 전까지 점프 관련 상태 변화를 막기 위한 플래그 (필요하다면 사용)
        _elapsedTime = 0f; // 경과 시간 초기화

        owner.rb.velocity = new Vector3(owner.rb.velocity.x, 0f, owner.rb.velocity.z);

        // 점프 시작 애니메이션 재생
        //  owner.AnimateTrigger("JumpStart");
        owner.CMDAnimationTrigger("JumpStart");
    }

    // 점프 종료 시 호출되는 메소드 (착지 또는 JumpUp 애니메이션 종료 등)
    public void JumpDown()
    {
        // 이미 점프 중이 아니면 종료 처리하지 않음
        if (!owner.isJumping) return; // owner.canJumping 체크는 필요에 따라 유지하거나 삭제
                                      // owner.AnimateTrigger("JumpEnd");
        owner.CMDAnimationTrigger("JumpEnd");
        owner.isJumping = false; // 점프 중 플래그 끄기
        owner.canJumping = true; // JumpUp 상태로 다시 전환 가능하게 함 (필요에 따라 사용)
        _elapsedTime = 0f; // 경과 시간 초기화 (다음 점프를 위해)
    }
}