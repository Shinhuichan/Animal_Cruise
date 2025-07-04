using UnityEngine;
using System;
using CustomInspector.Extensions;
// using UnityEngine.InputSystem;

// abstract : 자식 class는 반드시 해당 이름을 가진 클래스를 선언해야됨
// virtual : 자식 class는 해당 이름을 가진 클래스를 선언해도 됨.

// GAS (Game Ability System) : 언리얼
// Ability의 속성
[Flags]
public enum AbilityFlag
{
    // Bit Shift
    // A << B 
    // Shift할 크기 << Bit 자리
    NONE = 0,
    // Player 이동(Movement)
    MOVE = 1 << 0,
    // 공용
    JUMP = 1 << 1,
    CROWL = 2 << 1
}

// Ability의 지속성
public enum AbilityEffect
{
    INSTANCE,
    DURATION,
    INFINITE
}

// 데이터 담당
    // 역할 1 : Ability의 Type을 정한다.
    // 역할 2 : Ability Type에 맞게 생성한다.
public abstract class AbilityData : ScriptableObject
{
    // 읽기 전용 데이터
    public abstract AbilityFlag Flag { get; }
    public AbilityEffect Effect;
    public abstract Ability CreateAbility(CharacterControl owner);
}

// 행동 담당
public abstract class Ability
{
    // Ability 활성화
    public virtual void Activate(object obj = null) {}
    // Ability 비활성화
    public virtual void Deactivate() {}
    // Ability 활성화 및 갱신
    public virtual void Update() {}
    // 빠른 Update Ver
    public virtual void FixedUpdate() {}
}

public abstract class Ability<D> : Ability where D : AbilityData
{
    // 읽기 전용 데이터
    public D Data { get; }
    protected CharacterControl owner;

    // Ability에서 사용되는 owner를 사용한다는 의미
    // 덮어쓰기
    public Ability(D data, CharacterControl owner) 
    {
        this.Data = data;
        this.owner = owner;
    }
}