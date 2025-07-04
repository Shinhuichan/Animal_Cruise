using TMPro;
using UnityEngine;

public class CharacterBillboard : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private void Start()
    {
        TryGetComponent(out nameText);
    }
    public void SetCharacterInfo(string playerLabel)
    {
        nameText.text = $"{playerLabel}";
    }

    private void Update()
    {
        // 카메라 방향으로 항상 보이게
        if (Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
