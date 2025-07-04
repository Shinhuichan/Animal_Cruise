using UnityEngine;
using CustomInspector;

// Unity에 내장된 키워드
[CreateAssetMenu(menuName = "Abilities/JUMP")]
public class AbilityJumpData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.JUMP;

    public override Ability CreateAbility(CharacterControl owner) => new AbilityJump(this, owner);

    [ReadOnly] public float jumpForce = 20f;
    [ReadOnly] public float jumpDuration = 0.3f;
    public AnimationCurve jumpCurve;
}
