using Mirror.Examples.Basic;
using UnityEngine;

public class CharacterJumpCheck : MonoBehaviour
{
    [SerializeField] private CharacterControl character;

    void OnTriggerStay(Collider other)
    {
        if (character == null) return;
        else
        {
            if (other.gameObject.tag != "Player" && other.gameObject.tag != "Bomb" && other != gameObject.GetComponent<Collider>() )
                character.mustCrawling = true;
            character.canJumping = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        character.mustCrawling = false;
        if (character == null) return;
        character.canJumping = true;
    }
}