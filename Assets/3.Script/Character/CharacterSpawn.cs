using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[System.Serializable]
public class PlayerUI
{
    public int playerIndex; // 0 = 1P, 1 = 2P ...
    public Image previewImage;
    public Transform spawnPoint;
    public List<GameObject> UiButtons;
}

public class CharacterSpawn : MonoBehaviour
{
    [Header("캐릭터 프리팹 및 이미지")]
    public List<GameObject> characterList;

    public List<Sprite> characterImages = new List<Sprite>();

    [Header("플레이어별 UI")]
    public List<PlayerUI> playerUIList = new List<PlayerUI>();

    private int[] currentIndexes = new int[4];
    private bool[] isConfirmed = new bool[4];
    private bool[] isCharacterTaken = new bool[4];

    private void Start()
    {
        for (int i = 0; i < playerUIList.Count; i++)
        {
            UpdatePreviewImage(i);
        }
    }

    // 1P 좌우 + 선택
    public void OnLeftClick_0() => OnSelectLeft(0);
    public void OnRightClick_0() => OnSelectRight(0);
    public void OnConfirmClick_0() => OnConfirm(0);
    public void OnCancelClick_0() => OnCancel(0);


    // 2P 좌우 + 선택
    public void OnLeftClick_1() => OnSelectLeft(1);
    public void OnRightClick_1() => OnSelectRight(1);
    public void OnConfirmClick_1() => OnConfirm(1);
    public void OnCancelClick_1() => OnCancel(1);

    // 3P 좌우 + 선택
    public void OnLeftClick_2() => OnSelectLeft(2);
    public void OnRightClick_2() => OnSelectRight(2);
    public void OnConfirmClick_2() => OnConfirm(2);
    public void OnCancelClick_2() => OnCancel(2);

    // 4P 좌우 + 선택
    public void OnLeftClick_3() => OnSelectLeft(3);
    public void OnRightClick_3() => OnSelectRight(3);
    public void OnConfirmClick_3() => OnConfirm(3);
    public void OnCancelClick_3() => OnCancel(3);

    private void OnSelectLeft(int playerIndex)
    {
        if (isConfirmed[playerIndex]) return;

        currentIndexes[playerIndex] =
            (currentIndexes[playerIndex] - 1 + characterImages.Count) % characterImages.Count;

        UpdatePreviewImage(playerIndex);
    }

    private void OnSelectRight(int playerIndex)
    {
        if (isConfirmed[playerIndex]) return;

        currentIndexes[playerIndex] =
            (currentIndexes[playerIndex] + 1) % characterImages.Count;

        UpdatePreviewImage(playerIndex);
    }


    public void OnConfirm(int playerIndex)
    {

        if (isConfirmed[playerIndex]) return;

        int charIndex = currentIndexes[playerIndex];

        if (isCharacterTaken[charIndex])
        {
            Debug.Log($"Player {playerIndex + 1}가 선택한 캐릭터는 이미 선택됨");
            return;
        }

        isConfirmed[playerIndex] = true;
        isCharacterTaken[charIndex] = true;


        GameObject character = Instantiate(
            characterList[charIndex],
            playerUIList[playerIndex].spawnPoint.position,
            playerUIList[playerIndex].spawnPoint.rotation);

        CharacterBillboard billboard = character.GetComponentInChildren<CharacterBillboard>();

        if (billboard != null)
        {
            string label = $"{playerIndex + 1}P";
            billboard.SetCharacterInfo(label);
        }

        Rigidbody rb = characterList[charIndex].GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        Debug.Log($"Player {playerIndex + 1} 선택 완료: {characterImages[charIndex].name}");

        foreach (var ui in playerUIList[playerIndex].UiButtons)
            ui.SetActive(false);

        if (playerUIList[playerIndex].previewImage != null)
            playerUIList[playerIndex].previewImage.gameObject.SetActive(false);


        Debug.Log($"playerIndex : {playerIndex} / charIndex : {charIndex}");


        var identity = NetworkClient.connection.identity;
        if (identity != null)
        {
            var roomPlayer = NetworkClient.localPlayer.GetComponent<MyRoomPlayer>();
            if (roomPlayer != null)
            {
                roomPlayer.CmdSetCharacterIndex(charIndex); // 서버로 인덱스 전달
            }
        }
    }

    private void UpdatePreviewImage(int playerIndex)
    {
        int charIndex = currentIndexes[playerIndex];

        if (playerUIList[playerIndex].previewImage != null && characterImages.Count > charIndex)
            playerUIList[playerIndex].previewImage.sprite = characterImages[charIndex];
    }

    private void OnCancel(int playerIndex)
    {
        if (!isConfirmed[playerIndex]) return; // 선택 안 했으면 무시

        int charIndex = currentIndexes[playerIndex];

        isConfirmed[playerIndex] = false;
        isCharacterTaken[charIndex] = false;

        // 캐릭터 삭제
        GameObject existing = GameObject.Find($"{characterImages[charIndex].name}(Clone)");
        if (existing != null)
            Destroy(existing);

        Debug.Log($" Player {playerIndex + 1} 선택 취소");

        // 다시 프리뷰 선택 가능하게 함
        UpdatePreviewImage(playerIndex);

        foreach (var ui in playerUIList[playerIndex].UiButtons)
            ui.SetActive(true);

        if (playerUIList[playerIndex].previewImage != null)
            playerUIList[playerIndex].previewImage.gameObject.SetActive(true);
    }
}