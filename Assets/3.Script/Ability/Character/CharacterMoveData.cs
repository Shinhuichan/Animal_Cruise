using UnityEngine;
using CustomInspector;

// Unity에 내장된 키워드
[CreateAssetMenu(menuName = "Abilities/MOVE")]
public class AbilityMoveData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.MOVE;

    public override Ability CreateAbility(CharacterControl owner) => new AbilityMove(this, owner);
    [ReadOnly] public float movePerSec = 0f;
    [ReadOnly] public float rotatePerSec = 0f;
}