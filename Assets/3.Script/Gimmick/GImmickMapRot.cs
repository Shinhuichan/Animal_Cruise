using System.Collections;
using UnityEngine;

public class GimmickMapRot : MonoBehaviour
{
    public GameObject Map;                    // 회전시킬 맵 오브젝트
    public Vector3 tiltAngle = new Vector3(0, 0, 10); // 기울일 각도
    public float rotationSpeed = 2f;           // 기울어지는 속도 (걸리는 시간)
    public float waitTime = 10f;                 // 기울어진 상태에서 대기 시간

    private void Start()
    {
        StartCoroutine(MapRotation());
    }

    private IEnumerator MapRotation()
    {
        while (true)
        {
            // 현재 회전 상태 저장
            Quaternion startRot = Map.transform.rotation;
            Quaternion targetRot = Quaternion.Euler(tiltAngle);

            // 회전 시간 제어
            float elapsedTime = 0f;
            while (elapsedTime < rotationSpeed)
            {
                float t = elapsedTime / rotationSpeed;
                Map.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // 정확하게 목표 각도로 설정 (Lerp 부정확성 보정)
            Map.transform.rotation = targetRot;

            // 기울어진 상태로 대기
            yield return new WaitForSeconds(waitTime);

            // 다시 원래대로 복귀
            startRot = Map.transform.rotation;
            targetRot = Quaternion.identity; // 초기 회전 (Quaternion.identity == (0,0,0))

            elapsedTime = 0f;
            while (elapsedTime < rotationSpeed)
            {
                float t = elapsedTime / rotationSpeed;
                Map.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Map.transform.rotation = targetRot;

            // 원래대로 돌아온 후 대기
            yield return new WaitForSeconds(waitTime);
        }
    }
}
