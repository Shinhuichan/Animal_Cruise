using UnityEngine;
using CustomInspector;

// Unity에 내장된 키워드
[CreateAssetMenu(menuName = "Abilities/CROWL")]
public class AbilityCrawlData : AbilityData
{
    public override AbilityFlag Flag => AbilityFlag.CROWL;

    public override Ability CreateAbility(CharacterControl owner) => new AbilityCrawl(this, owner);

    [ReadOnly] public float crawlmovePerSec = 0f;
    [ReadOnly] public float rotatePerSec = 0f;
}
