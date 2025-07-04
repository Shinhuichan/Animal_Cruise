using Mirror;
using UnityEngine;
// Ability 클래스와 Ability_MoveData 클래스가 정의되어 있다고 가정
// CharacterControl 클래스에 rb (Rigidbody), isGrounded, animator 등이 정의되어 있다고 가정
// AnimatorHashes 클래스가 정의되어 있다고 가정

public class AbilityMove : Ability<AbilityMoveData> // Ability_MoveData 타입의 Data 속성을 가짐
{
    // WASD 입력으로 계산될 캐릭터의 이동 방향 벡터 (Update에서 설정, FixedUpdate에서 사용)
    Vector3 direction;

    // ===> 추가된 변수: 카메라 Transform <===
    private Transform cameraTransform;
    float horizontalInput;
    float verticalInput;

    // 생성자
    public AbilityMove(AbilityMoveData data, CharacterControl owner) : base(data, owner)
    {
        if (owner.Profile == null) return;

        // 해당 Character의 데이터를 직접 대입. (Data 속성은 Ability_MoveData 타입의 인스턴스)
        Data.movePerSec = owner.moveSpeed; // Profile에서 moveSpeed 데이터를 가져옴
        Data.rotatePerSec = owner.rotateSpeed; // Profile에서 rotateSpeed 데이터를 가져옴

        // ===> 추가된 코드: 메인 카메라 Transform 가져오기 <===
        // Start나 Awake에서 가져와도 되지만, Ability 생성 시점에서 가져올 수도 있어.
        // Camera.main은 씬에서 "MainCamera" 태그가 붙은 카메라를 찾아와.
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogWarning("씬에서 'MainCamera' 태그가 붙은 카메라를 찾을 수 없습니다! 카메라 기반 이동이 작동하지 않을 수 있습니다.");
    }

    // ===> 추가된 메소드: Update (입력 처리) <===
    // Ability 기본 클래스에 Update 메소드가 오버라이드 가능하도록 되어 있다고 가정
    public override void Update()
    {
        if (!owner.isLocalPlayer) return;
        var keyManager = KeySettingManager.Instance;
        // 카메라가 없다면 이동 입력을 처리하지 않음
        if (cameraTransform == null)
        {
            direction = Vector3.zero; // 카메라 없으면 움직임 방향 0으로 설정
            return;
        }

        if (Input.GetKey(keyManager.GetKey(BindingType.MoveUp)))
            verticalInput = 1f;
        else if (Input.GetKey(keyManager.GetKey(BindingType.MoveDown)))
            verticalInput = -1f;
        else 
            verticalInput = 0f;

        if (Input.GetKey(keyManager.GetKey(BindingType.MoveRight)))
            horizontalInput = 1f;
        else if (Input.GetKey(keyManager.GetKey(BindingType.MoveLeft)))
            horizontalInput = -1f;
        else
            horizontalInput = 0f;

        // 2. 카메라의 전방(Forward) 및 오른쪽(Right) 방향 벡터 가져오기
            // 카메라의 Y축 회전은 무시하고 수평 방향만 사용 (캐릭터가 위아래를 보며 움직이지 않도록)
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized; // 카메라의 오른쪽 방향

        // 3. 입력 값과 카메라 방향을 바탕으로 이동 방향 벡터 계산
        // verticalInput이 양수면 카메라 전방 방향으로, 음수면 반대 방향으로
        // horizontalInput이 양수면 카메라 오른쪽 방향으로, 음수면 반대 방향으로
        direction = (cameraForward * verticalInput + cameraRight * horizontalInput * owner.moveSpeed).normalized; // 정규화해서 방향만 사용
        // Debug.Log($"입력 방향: {direction}"); // 디버그용
    }


    // FixedUpdate는 물리 업데이트 주기에 맞춰 호출
    public override void FixedUpdate()
    {
        if (!owner.isLocalPlayer) return;
        if (!owner.isCrowling)
        {
            // Update에서 계산된 direction 값을 사용해서 이동과 회전 처리
            Movement();
            Rotate();
        }
    }

    void Movement()
    {
        Vector3 desiredVelocity = direction * Data.movePerSec; // 목표 속도 (방향 * 초당 이동 속도)

        Vector3 currentXZVelocity = new Vector3(owner.rb.velocity.x, 0f, owner.rb.velocity.z);
        Vector3 targetXZVelocity = new Vector3(desiredVelocity.x, 0f, desiredVelocity.z);

        // Lerp 속도 조절 (Time.fixedDeltaTime 사용 권장)
        float velocityLerpFactor = 10f; // 속도 전환 속도 (인스펙터 노출 변수로 만들면 좋음)
        Vector3 smoothedXZVelocity = Vector3.Lerp(currentXZVelocity, targetXZVelocity, Time.fixedDeltaTime * velocityLerpFactor);

        // Rigidbody 속도 업데이트 (Y 속도는 유지)
        owner.rb.velocity = new Vector3(smoothedXZVelocity.x, owner.rb.velocity.y, smoothedXZVelocity.z);

        // 애니메이터 관련 코드 (주석 처리되어 있으니 그대로 둠)
        if (owner.isGrounded)
        {
            float moveSpeed = Vector3.Distance(Vector3.zero, owner.rb.GetRelativePointVelocity(Vector3.zero));
            float targetSpeed = Mathf.Clamp01(moveSpeed / Data.movePerSec);

            //Mathf.Lerp(owner.animator.GetFloat("moveSpeed"), targetSpeed, Time.deltaTime * 10f);
            //owner.animator?.SetBool("Ground", false);
            //owner.animator?.SetFloat("moveSpeed", moveSpeed);

            owner.CMDAnimationPlay("moveSpeed", "Ground", moveSpeed, targetSpeed, Time.deltaTime * 10f, false);


        }

        if (horizontalInput == 0f && verticalInput == 0f)
            Stop();
    }

   
    void Rotate()
    {
        // Update에서 계산된 direction이 Vector3.zero가 아니면 회전 처리
        if (direction == Vector3.zero)
            // 움직임 입력이 없을 때는 현재 방향 유지
            return;

        // Atan2를 사용해서 이동 방향(direction)의 XZ 평면 각도 계산 (Y축 회전)
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float smoothAngle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, targetAngle, ref Data.rotatePerSec, 0.1f);

        // 계산된 부드러운 각도로 캐릭터의 Y축 회전 설정 (X와 Z 회전은 0으로 고정)
        owner.transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
    }
    void Stop()
    {
        direction = Vector3.zero;
        owner.rb.velocity = new Vector3(0f, owner.rb.velocity.y, 0f);
        owner.animator?.SetFloat("moveSpeed", 0f);
    }
}