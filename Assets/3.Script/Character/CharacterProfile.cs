using UnityEngine;
using CustomInspector;
using System.Collections.Generic;


public enum CharacterType
{
    Cat = 0,
    Rabbit,
    Mouse,
    Bear
}
[CreateAssetMenu(menuName = "Datas/CharacterProfile", fileName = "CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    [HorizontalLine("Attributes", color: FixedColor.CloudWhite), HideField] public bool _l4;
    [Tooltip("캐릭터 Type")] public CharacterType type;
    [Tooltip("이동 속도 (per Second)")] public float moveSpeed;
    [Tooltip("회전 속도")] public float rotateSpeed;
    [Tooltip("각력")] public float jumpForce;
    [Tooltip("체공 시간")] public float jumpDuration;
// TEMP //
    [Tooltip("숙이기 이동 증감비")] public float crawlMoveIncrese;
// TEMP //

    [Tooltip("무게 / 힘")] public float weight;


    [HorizontalLine("Abilities", color: FixedColor.CloudWhite), HideField] public bool _l2;
    public List<AbilityData> abilities;
    [HorizontalLine(color: FixedColor.CloudWhite), HideField] public bool _l3;
}