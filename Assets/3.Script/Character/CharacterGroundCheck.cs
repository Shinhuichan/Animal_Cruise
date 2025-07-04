using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{
    [SerializeField] private CharacterControl character;

    void OnTriggerStay(Collider other)
    {
        if (character == null) return;
        else character.isGrounded = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (character == null) return;
        character.isCrowling = false;
        character.mustCrawling = false;
        character.isGrounded = false;
    }
}