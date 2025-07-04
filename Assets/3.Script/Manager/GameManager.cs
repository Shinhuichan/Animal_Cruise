using CustomInspector;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    protected override bool IsDontDestroy() => true;

    // 게임에서 중요하게 다뤄질 데이터가 들어가야 함.
    [ReadOnly] public bool isGameover = false;
    [ReadOnly] public int currentStage = 0;

    [ReadOnly, Range(0, 3)] public int playerNumber = 0;
    [ReadOnly] public GameObject CharacterObject;

}