using System.Collections.Generic;
using UnityEngine;

public class CheckCharacterWeight : MonoBehaviour
{
    [SerializeField] GimmickWeightPanel weightPanel;
    private List<CharacterControl> characterInputList = new List<CharacterControl>();
    private CharacterControl inputCharacter;
    void OnTriggerEnter(Collider other)
    {
        inputCharacter = other.GetComponentInParent<CharacterControl>();
        if (inputCharacter != null)
        {
            if (!characterInputList.Contains(inputCharacter))
            {
                characterInputList.Add(inputCharacter);
                weightPanel.characterTotalWeight += inputCharacter.Profile.weight;
                inputCharacter.transform.SetParent(transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        inputCharacter = other.GetComponentInParent<CharacterControl>();
        if (inputCharacter != null)
        {
            if (characterInputList.Contains(inputCharacter))
            {
                characterInputList.Remove(inputCharacter);
                weightPanel.characterTotalWeight -= inputCharacter.Profile.weight;
                inputCharacter.transform.SetParent(null);
            }
        }
    }
}