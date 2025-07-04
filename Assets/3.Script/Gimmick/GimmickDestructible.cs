using UnityEngine;
using System.Collections;
using Mirror; // 코루틴 사용을 위해 필요합니다.
public enum ExplosionType
{
    AllDestroy = 0,
    DestroyOnlySpecificType,
    OnlyDestroyPlayer,
    NoDestroy
}
// 이 스크립트는 특정 충격량 이상의 힘을 받으면 파괴될 오브젝트에 붙입니다.
[RequireComponent(typeof(Rigidbody))] // 이 스크립트가 붙은 오브젝트는 Rigidbody를 반드시 가져야 합니다.
[RequireComponent(typeof(Collider))] // 이 스크립트가 붙은 오브젝트는 Collider를 반드시 가져야 합니다.
public class GimmickDistructible : MonoBehaviour
{
    [SerializeField] private float impactSpeedThreshold = 10f; // 폭파에 필요한 최소 힘
    [SerializeField] ExplosionType type;
    [SerializeField] private GameObject explosionEffectPrefab; // 폭발 이펙트 Prefab

    // TAG로 부술 수 있는 오브젝트를 정할 수 있음.
    // [SerializeField] private string requiredColliderTag = "";

    private Rigidbody rb; // 이 오브젝트의 Rigidbody 컴포넌트
    private bool isBeingDestroyed = false; // 파괴 과정이 시작되었는지 체크하는 플래그

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic)
            Debug.LogWarning("GimmickDistructible 스크립트는 Kinematic Rigidbody와 함께 사용할 때 예상대로 동작하지 않을 수 있습니다.", this);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 이미 파괴 과정이 시작되었다면 중복 실행 방지
        if (isBeingDestroyed)
            return;

        // 특정 태그를 가진 오브젝트와만 충돌했을 때 파괴되게 하려면
        // if (!string.IsNullOrEmpty(requiredColliderTag) && !collision.gameObject.CompareTag(requiredColliderTag))
        //     return; // 요구하는 태그가 아니면 무시

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed >= impactSpeedThreshold)
        {
            // 조건 충족: 강한 충격! 파괴 과정을 시작합니다.
            Debug.Log(gameObject.name + "이(가) 강한 충격으로 파괴 과정 시작! 충격 속도: " + impactSpeed + " (임계값: " + impactSpeedThreshold + ")");

            // 파괴 과정 시작 플래그 설정
            isBeingDestroyed = true;

            // 오브젝트를 바로 파괴하는 대신, 이펙트 생성 후 대기 코루틴 시작 
            GameObject explosionEffectInstance = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

            // 오브젝트 자체는 일단 비활성화하거나 물리 효과를 멈춰서 충돌 등 추가 반응을 막을 수 있습니다.
            GetComponent<Collider>().enabled = false;
            if (rb != null) rb.isKinematic = true;
            StartCoroutine(WaitUntilEffectEndsAndDestroy(explosionEffectInstance));
        }
        else
            Debug.Log(gameObject.name + "이(가) 충돌했지만 충격이 약해 파괴되지 않습니다.");
    }

    private IEnumerator WaitUntilEffectEndsAndDestroy(GameObject effectGameObject)
    {
        // 이펙트 게임 오브젝트가 유효한지 확인 (혹시 먼저 파괴되었을 수도 있으니)
        if (effectGameObject != null)
        {
            // ===> 이펙트 오브젝트에서 ParticleSystem 컴포넌트 가져오기 <===
            ParticleSystem ps = effectGameObject.GetComponent<ParticleSystem>();

            if (ps != null)
                yield return new WaitWhile(() => ps.isPlaying);

            // 이펙트가 끝났다고 판단되면 원래 오브젝트를 파괴합니다.

            if (gameObject != null)
                Destroy(gameObject);
            Debug.Log(gameObject.name + " 오브젝트가 이펙트 종료 후 파괴되었습니다.");

            if (effectGameObject != null)
                Destroy(effectGameObject);
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (isBeingDestroyed)
        {
            if (other.tag == "Player" || type == ExplosionType.NoDestroy)
            {
                CharacterControl character = other.transform.parent.transform.parent.GetComponent<CharacterControl>();
                var localPlayer = NetworkClient.localPlayer?.GetComponent<GamePlayerRestart>();
                if (localPlayer == null)
                {
                    Debug.LogError("[OptionUI] GamePlayerRestart 컴포넌트를 찾을 수 없습니다!");
                    
                    // 대안: 씬에서 직접 찾기
                    localPlayer = FindFirstObjectByType<GamePlayerRestart>();
                    if (localPlayer == null)
                    {
                        Debug.LogError("[OptionUI] 씬에서도 GamePlayerRestart를 찾을 수 없습니다!");
                        return;
                    }
                }
                Debug.Log("[OptionUI] 종료 요청 전송");
                localPlayer.CmdRequestQuit();
            }
            else if (type == ExplosionType.DestroyOnlySpecificType && other.tag == "Destroy")
                Destroy(other.gameObject);
            else if (type == ExplosionType.AllDestroy)
                Destroy(other.gameObject);
        }
    }
}